﻿using System.Collections.ObjectModel;

using System.Windows;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;

namespace MusicAppComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayListService playListService { get; set; } = new();
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
            var playList = playListService.GetPlaylist(); // gọi db
           
            Playlists = new ObservableCollection<PlayList>(playList);
            DataContext = this; // rất quan trọng để Binding hoạt động
        }

        private void AddSongBtn(object sender, RoutedEventArgs e)
        {

            SongDetail songDetail = new SongDetail();
            songDetail.ShowDialog();

        }
    }
   
}
