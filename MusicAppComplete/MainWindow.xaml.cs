using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;


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
        public MainWindow()
        {
            InitializeComponent();
            LoadPlaylist();
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

            songDetail.ShowDialog();

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
    }
}


