using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;
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
        public ObservableCollection<PlayList> Playlists { get; set; } = new ObservableCollection<PlayList>();
        public PlayList selectedPlayList { get; set; }

        // Play Control
        private MediaElement mediaPlayer = new MediaElement();
        private DispatcherTimer playbackTimer;
        private bool isDraggingSlider = false;
        private Song currentSong;
        private bool isPlaying = false;
        private bool isRepeating = false; // Tracks repeat mode
        private bool isSequential = true; // Tracks sequential playback mode (default: true)

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



            playListDetail.ShowDialog();

        }

        private void LoadPlaylist()
        { // khởi tạo service
            var playList = _playListService.GetPlaylist(); // gọi db

            Playlists = new ObservableCollection<PlayList>(playList);
           
            var songs = _songService.getAllSong();
            SongListBox.ItemsSource = songs;
            DataContext = this;


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

            // 1. Check if a playlist is selected
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
                return; // User canceled the deletion
            }

            // 3. Delete the playlist
            _songService.DeleteSong(selectedSong);// Replace with your actual method

            // 4. Refresh the UI
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


        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong == null)
            {
                MessageBox.Show("Please select a song to play.", "No Song Selected");
                return;
            }

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
            if (SongListBox.Items.Count == 0)
                return;

            int currentIndex = SongListBox.SelectedIndex;
            int newIndex = currentIndex - 1;

            if (newIndex < 0)
            {
                newIndex = SongListBox.Items.Count - 1; // Wrap around to last song
            }

            SongListBox.SelectedIndex = newIndex;
        }

        private void NextTrack_Click(object sender, RoutedEventArgs e)
        {
            if (SongListBox.Items.Count == 0)
                return;

            int currentIndex = SongListBox.SelectedIndex;
            int newIndex = currentIndex + 1;

            if (newIndex >= SongListBox.Items.Count)
            {
                newIndex = 0; // Wrap around to first song
            }

            SongListBox.SelectedIndex = newIndex;

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
            NextTrack_Click(sender, e); // Auto-play next track
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
        }
    }
}


