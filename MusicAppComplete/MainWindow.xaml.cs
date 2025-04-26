using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;
using Unosquare.FFME.Common;


namespace MusicAppComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayListService _playListService { get; set; } = new();
        private SongService _songService { get; set; } = new() { };
        public ObservableCollection<PlayList> Playlists { get; set; }
        public PlayList selectedPlayList { get; set; }

        // Play Control
        private MediaElement mediaPlayer = new MediaElement();
        private DispatcherTimer playbackTimer;
        private bool isDraggingSlider = false;
        private Song currentSong;
        private bool isPlaying = false;
        private bool isRepeating = false; // Tracks repeat mode
        private bool isSequential = true; // Tracks sequential playback mode (default: true)
        private List<Song> shuffleSongList = new List<Song>();
        private int currentSongIndex { get; set; } = -1;  // index in song List OR shuffled list
        public MainWindow()
        {
            InitializeComponent();
            LoadPlaylist();
            InitializeMediaPlayer();
            InitializePlaybackTimer();
        }
        private void InitializeMediaPlayer()
        {

            mediaPlayer.LoadedBehavior = MediaState.Manual;
            mediaPlayer.UnloadedBehavior = MediaState.Manual;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            Grid.SetRow(mediaPlayer, 0);
            Grid.SetColumn(mediaPlayer, 0);
            MainGrid.Children.Add(mediaPlayer);
        }

        private void MediaPlayer_MediaFailed(object? sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show($"Error playing song: {e.ErrorException.Message}", "Playback Error");
            ResetPlaybackState();
        }

        private void InitializePlaybackTimer()
        {
            playbackTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            playbackTimer.Tick += PlaybackTimer_Tick;
        }

        private void AddPlaylistBtn(object sender, RoutedEventArgs e)
        {



            PlayListDetail playListDetail = new PlayListDetail();
            playListDetail.IdStackPanel.Visibility = Visibility.Collapsed;


            playListDetail.ShowDialog();
            LoadPlaylist();

        }

        private void LoadPlaylist()
        { // khởi tạo service
            var playList = _playListService.GetPlaylist();

     
            PlaylistListBox.ItemsSource = null; 
            PlaylistListBox.ItemsSource = playList;

            var songs = _songService.getAllSong();

            SongListBox.ItemsSource = songs;
            shuffleSongList = songs.ToList();

            TotalSongTextBlock.Text = "Total Songs: " + _songService.getAllSong().Count.ToString();
            DataContext = this;


        }
        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var selectedPlaylist = (sender as ListBox).SelectedItem as PlayList;

            if (selectedPlaylist != null)
            {
                var songs = _playListService.GetPlaylistSong(selectedPlaylist.Id);
                BackButton.Visibility = Visibility.Visible;
                TitlePlayList.Text = selectedPlaylist.Name;

                SongListBox.ItemsSource = songs;

                // Cập nhật số lượng bài hát
                TotalSongTextBlock.Text = $"   Total Songs: {selectedPlaylist.PlaylistSongs.Count}";
            }
            else
            {
              
                SongListBox.ItemsSource = null;
              //  TotalSongsTextBlocks.Text = "Total Songs: 0";
            }
        }
        private void AddSongBtn(object sender, RoutedEventArgs e)
        {


            SongDetail songDetail = new SongDetail();
            songDetail.SongDetailLable.Content = "Create a new Song";
            songDetail.IdStackPanel.Visibility = Visibility.Collapsed;
            songDetail.ShowDialog();
            LoadPlaylist();

        }

        private void AddFolderbtn(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSong_Click(object sender, RoutedEventArgs e)
        {

            
            var menuItem = sender as MenuItem;
            Song selectedSong = menuItem?.DataContext as Song;
            if (selectedSong == null)
            {
                MessageBox.Show("Please select a playlist before deleting", "Select a Song", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. Confirm deletion
            MessageBoxResult answer = MessageBox.Show("Do you want to delete this Song?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return; 
            }

           
            _songService.DeleteSong(selectedSong);

           
            LoadPlaylist();
        }

        private void EditSong_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            Song selectedSong = menuItem?.DataContext as Song;

            SongDetail songDetail = new SongDetail();
            songDetail.SongDetailLable.Content = "Update Song";
            songDetail.EditedOne = selectedSong;


            songDetail.ShowDialog();
            LoadPlaylist();

        }
        private void EditPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            PlayList selectedPlaylist = menuItem?.DataContext as PlayList;
            if (selectedPlaylist == null)
            {
                MessageBox.Show("Please select a playlist to edit.", "No Playlist Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PlayListDetail detail= new PlayListDetail();
            detail.TitleBox.Text = "Update Playlist";

            detail.EditOne = selectedPlaylist;

            detail.ShowDialog();
            LoadPlaylist(); 
        }
        private void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            // Lấy playlist đang được chọn
            var selectedPlaylist = PlaylistListBox.SelectedItem as PlayList;
            if (selectedPlaylist == null)
            {
                MessageBox.Show("Please select a playlist first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mở cửa sổ chọn bài hát
            SongDetail songDetailWindow = new SongDetail();
            if (songDetailWindow.ShowDialog() == true)
            {
                // Lấy bài hát được tạo hoặc chỉnh sửa
                var selectedSong = songDetailWindow.SelectedSong;

                if (selectedSong != null)
                {
                    var repo = new PlayListRepo();

                    // Kiểm tra bài hát đã tồn tại trong playlist chưa
                    var existingSongs = repo.GetSongsByPlaylist(selectedPlaylist.Id);
                    if (existingSongs.Any(s => s.Id == selectedSong.Id))
                    {
                        MessageBox.Show("This song is already in the selected playlist.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    // Thêm bài hát vào playlist
                    var playlistSong = new PlayListSongs.PlaylistSong
                    {
                        PlaylistId = selectedPlaylist.Id,
                        SongId = selectedSong.Id
                    };
                    repo.AddSongToPlaylist(playlistSong);

                    // Refresh danh sách bài hát hiển thị
                    var updatedSongs = repo.GetSongsByPlaylist(selectedPlaylist.Id);
                    SongListBox.ItemsSource = updatedSongs;
 
                
                }
                LoadPlaylist();
            } 
        }

        private void DeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            PlayList selectedPlaylist = menuItem?.DataContext as PlayList;
            if (selectedPlaylist == null)
            {
                MessageBox.Show("Please select a playlist to delete.", "No Playlist Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Do you want to delete this Playlist?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            _playListService.DeletePlayList(selectedPlaylist); 
            LoadPlaylist(); 
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SongListBox.Items.Count == 0)
            {
                MessageBox.Show("No songs available to play.", "No Songs");
                return;
            }

            if (currentSong == null)
            {

                if (isSequential)
                {
                    currentSongIndex = 0;
                    currentSong = SongListBox.Items[0] as Song;
                }
                else
                {
                    currentSongIndex = new Random().Next(0, shuffleSongList.Count);
                    currentSong = shuffleSongList[currentSongIndex];
                }
                SongListBox.SelectedItem = currentSong;
            }
            try
            {

                if (mediaPlayer.Source == null || mediaPlayer.Source.OriginalString != currentSong.Path)
                {
                    mediaPlayer.LoadedBehavior = MediaState.Manual;
                    mediaPlayer.Source = new Uri(currentSong.Path);
                    MessageBox.Show(mediaPlayer.Source.ToString());
                    mediaPlayer.Play();
                    isPlaying = true;
                    PlayButton.Content = "⏸";
                    playbackTimer.Start();
                }
                else
                {
                    if (isPlaying)
                    {
                        mediaPlayer.Pause();
                        isPlaying = false;
                        PlayButton.Content = "▶";
                        playbackTimer.Stop();
                    }
                    else
                    {
                        mediaPlayer.Play();
                        isPlaying = true;
                        PlayButton.Content = "⏸";
                        playbackTimer.Start();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Failed to play song: {ex.Message}", "Playback Error");
                ResetPlaybackState();
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingSlider && mediaPlayer.Position.TotalSeconds <= PlaybackSlider.Maximum)
            {
                PlaybackSlider.Value = mediaPlayer.Position.TotalSeconds;
                UpdatePlaybackTimeText();
            }
        }
        private void PreviousTrack_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong == null)
                return;

            IList<Song> sourceList;
            ListBox activeListBox;

            if (SongListBox.Items.Contains(currentSong))
            {
                sourceList = SongListBox.Items.Cast<Song>().ToList();
                activeListBox = SongListBox;
            }
            else if (SongListBox.Items.Contains(currentSong))
            {
                sourceList = SongListBox.Items.Cast<Song>().ToList();
                activeListBox = SongListBox;
            }
            else
            {
                return;
            }

            if (isSequential)
            {
                currentSongIndex = sourceList.IndexOf(currentSong);
                currentSongIndex = currentSongIndex <= 0 ? sourceList.Count - 1 : currentSongIndex - 1;
                currentSong = sourceList[currentSongIndex];
            }
            else
            {
                currentSongIndex = currentSongIndex <= 0 ? shuffleSongList.Count - 1 : currentSongIndex - 1;
                currentSong = shuffleSongList[currentSongIndex];
            }

            activeListBox.SelectedItem = currentSong;
        }

        private void NextTrack_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong == null)
                return;

            IList<Song> sourceList;
            ListBox activeListBox;

            if (SongListBox.Items.Contains(currentSong))
            {
                sourceList = SongListBox.Items.Cast<Song>().ToList();
                activeListBox = SongListBox;
            }
            else if (SongListBox.Items.Contains(currentSong))
            {
                sourceList = SongListBox.Items.Cast<Song>().ToList();
                activeListBox = SongListBox;
            }
            else
            {
                return;
            }

            if (isSequential)
            {
                currentSongIndex = sourceList.IndexOf(currentSong);
                currentSongIndex = currentSongIndex >= sourceList.Count - 1 ? 0 : currentSongIndex + 1;
                currentSong = sourceList[currentSongIndex];
            }
            else
            {
                currentSongIndex = currentSongIndex >= shuffleSongList.Count - 1 ? 0 : currentSongIndex + 1;
                currentSong = shuffleSongList[currentSongIndex];
            }

            activeListBox.SelectedItem = currentSong;
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            mediaPlayer.Volume = VolumeSlider.Value;
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            PlaybackSlider.Maximum = mediaPlayer.NaturalDuration.HasTimeSpan
                ? mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds
                : 0;
            UpdatePlaybackTimeText();
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (isRepeating)
            {
                // Replay the same song
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            else
            {
                // Play next song based on mode
                NextTrack_Click(sender, e);
            }
        }


        private void UpdatePlaybackTimeText()
        {
            TimeSpan currentPosition = mediaPlayer.Position;
            TimeSpan totalDuration = mediaPlayer.NaturalDuration.HasTimeSpan
                ? mediaPlayer.NaturalDuration.TimeSpan
                : TimeSpan.Zero;

            string currentText = $"{currentPosition.Minutes:D2}:{currentPosition.Seconds:D2}";
            string totalText = $"{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
            PlaybackTimeTextBlock.Text = $"{currentText} / {totalText}";
        }

        private async Task ResetPlaybackState()
        {
            mediaPlayer.Stop();
            mediaPlayer.Source = null;
            PlaybackSlider.Value = 0;
            PlayButton.Content = "▶";
            playbackTimer.Stop();
            PlaybackTimeTextBlock.Text = "0:00 / 0:00";
        }

        private void PlaybackSlider_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDraggingSlider = true;

        }

        private void PlaybackSlider_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(PlaybackSlider.Value);
            UpdatePlaybackTimeText();
        }

        private void PlaybackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDraggingSlider)
            {
                TimeSpan newPosition = TimeSpan.FromSeconds(PlaybackSlider.Value);
                TimeSpan totalDuration = mediaPlayer.NaturalDuration.HasTimeSpan
                    ? mediaPlayer.NaturalDuration.TimeSpan
                    : TimeSpan.Zero;
                string currentText = $"{newPosition.Minutes:D2}:{newPosition.Seconds:D2}";
                string totalText = $"{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
                PlaybackTimeTextBlock.Text = $"{currentText} / {totalText}";
            }

        }

        private void SongListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SongListBox.SelectedItem is Song selectedSong)
            {
                currentSong = selectedSong;
                ResetPlaybackState();
                PlayBtn_Click(sender, new RoutedEventArgs());
            }
        }
      
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {

            isRepeating = !isRepeating;
            RepeatButton.Content = isRepeating ? "🔁" : "🔁"; // Update button content


        }

        private void SequentialButton_Click(object sender, RoutedEventArgs e)
        {
            isSequential = !isSequential;
            SequentialButton.Content = isSequential ? "➡️" : "🔀"; // Update button content
            if (!isSequential && SongListBox.Items.Count > 0)
            {
                // Shuffle songs when switching to random mode
                shuffleSongList = SongListBox.Items.Cast<Song>().OrderBy(x => Guid.NewGuid()).ToList();
                currentSongIndex = currentSong != null ? shuffleSongList.IndexOf(currentSong) : -1;
            }
            else
            {
                // Reset to original order for sequential mode
                shuffleSongList = SongListBox.Items.Cast<Song>().ToList();
                currentSongIndex = SongListBox.SelectedIndex;
            }
        }
        private void AddArtist_Click(object sender, RoutedEventArgs e)
        {
            var artistDetails = new ArtistDetails();
            artistDetails.ShowDialog(); // Mở cửa sổ ArtistDetails dưới dạng modal
        }

     

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TitlePlayList.Text = "All Songs";
            BackButton.Visibility = Visibility.Collapsed;
            LoadPlaylist();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlaylist();
        }
    }
}


