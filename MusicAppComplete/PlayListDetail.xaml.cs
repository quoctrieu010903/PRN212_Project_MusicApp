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
using PRN212.Assignment.DAL.Entities;

namespace MusicAppComplete
{
    /// <summary>
    /// Interaction logic for PlayListDetail.xaml
    /// </summary>
    public partial class PlayListDetail : Window
    {
        public PlayList EditOne { get; set; } = null;
        public PlayListDetail()
        {
            InitializeComponent();
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            SongDetail songDetail = new SongDetail();
            songDetail.ShowDialog();
        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {

        }
    }
}
