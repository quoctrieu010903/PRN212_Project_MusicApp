

using System.Windows;

using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;
using TagLib;


namespace MusicAppComplete
{
    /// <summary>
    /// Interaction logic for SongDetail.xaml
    /// </summary>
    public partial class SongDetail : Window
    {
        public SongService _service = new SongService();
        private ArtistService _artistService = new ArtistService();
        public Song EditedOne { get; set; } = null;
        public TimeSpan FileDuration { get; set; }


        public SongDetail()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
            if (EditedOne != null)
            {
                FillElement();
            }

        }
        private void SaveBtn(object sender, RoutedEventArgs e)
        {
            
            if (!ValidateInput())
            {
                return; // Exit if validation fails
            }

            Song song =  new Song(); // sau do gan gia 
            song.Title = TitleTextBox.Text;
            song.ArtistId = int.Parse(ArtistComboBox.SelectedValue.ToString());
            song.Duration = TimeSpan.Parse(DurationTextBox.Text); 
            song.Path = PathTextBox.Text;


            if (EditedOne == null)
            {
                _service.CreateSong(song);
                
            }
            else
            {
                song.Id = int.Parse(IdTextBox.Text);

                _service.UpdateSong(song);
            }
            this.Hide();

        }







        private bool ValidateInput()
        {
            

            // Validate Title
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Title cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate Duration
            // Validate Duration
            

            // Validate Path
            if (string.IsNullOrWhiteSpace(PathTextBox.Text))
            {
                MessageBox.Show("Path cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true; // All validations passed
        }

        private void TakeDataFromBrowser(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a Song File",
                Filter = "Audio Files (*.mp3;*.wav;*.flac)|*.mp3;*.wav;*.flac|All Files (*.*)|*.*"
            };
 
            if (openFileDialog.ShowDialog() == true)
            {
              
                PathTextBox.Text = openFileDialog.FileName;
                try
                {
                    var file = TagLib.File.Create(openFileDialog.FileName);
                
                    TitleTextBox.Text = string.IsNullOrWhiteSpace(file.Tag.Title) ? "Unknown Title" : file.Tag.Title;

                    DurationTextBox.Text = file.Properties.Duration.ToString(@"hh\:mm\:ss");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file metadata: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

     
        private void FillComboBox()
        {
            ArtistComboBox.ItemsSource = _artistService.GetArtists();
            //phải chọn cột muốn show 
            // chọn name nhung lấy id 
            ArtistComboBox.DisplayMemberPath = "Name";
            ArtistComboBox.SelectedValuePath = "ArtistId";
            // vào chấm SelectedValue 
            // lấy đyicwj id hoạcw set id vào để nhẩy 
        }

        private void FillElement()
        {

            IdTextBox.Text = EditedOne.Id.ToString();
            TitleTextBox.Text = EditedOne.Title;
            ArtistComboBox.SelectedValue = EditedOne.ArtistId;

            DurationTextBox.Text = EditedOne.Duration.ToString(@"hh\:mm\:ss");
            PathTextBox.Text = EditedOne.Path;

        }
    }
}
