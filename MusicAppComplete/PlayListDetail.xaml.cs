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
        private PlayListService PlayListService = new PlayListService();    
        public PlayList EditOne { get; set; } = null;
        public PlayListDetail()
        {
            InitializeComponent();
         
        }

        private void FillElement()
        {
            IdTextBox.Text = EditOne.Id.ToString();
            PlaylistNameTextBox.Text  = EditOne.Name;
        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {

            PlayList playList = new PlayList(); // sau do gan gia 
            playList.Name = PlaylistNameTextBox.Text;
            if (EditOne == null)
            {
                PlayListService.CreatePlaylist(playList);

            }
            else
            {

                IdTextBox.IsEnabled = false;
                playList.Id = EditOne.Id;
                PlayListService.UpdatePlaylist(playList);

            }
            this.Hide();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditOne != null)
            {
                FillElement();
            }
        }
    }
}
