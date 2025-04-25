using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;

namespace MusicAppComplete
{
    /// <summary>
    /// Interaction logic for PlayListDetail.xaml
    /// </summary>
    public partial class PlayListDetail : Window
    {
        public PlayList EditOne { get; set; } = null;
        private readonly PlayListService _playlistService = new PlayListService();

        public PlayListDetail()
        {
            InitializeComponent();
            Loaded += PlayListDetail_Loaded;
        }

        private void PlayListDetail_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditOne != null)
            {
                PlaylistNameTextBox.Text = EditOne.Name;
            }
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            SongDetail songDetail = new SongDetail();
            songDetail.ShowDialog();
        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {
            if (EditOne != null)
            {
                string newName = PlaylistNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Playlist name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                EditOne.Name = newName;

                // Gọi service để cập nhật DB
                _playlistService.UpdatePlaylist(EditOne);

                DialogResult = true;
                Close();
            }
        }


        private void PlaylistNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Optional: xử lý khi người dùng gõ vào textbox
        }
    }
}
