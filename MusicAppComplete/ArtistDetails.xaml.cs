using System;
using System.Windows;
using System.Windows.Controls;
using PRN212.Assignment.BLL.Services;
using PRN212.Assignment.DAL.Entities;

namespace MusicAppComplete
{
    public partial class ArtistDetails : Window
    {
        private readonly ArtistService _artistService = new();
        private Artist _selectedArtist = null;

        public ArtistDetails()
        {
            InitializeComponent();
            //_artistService = new ArtistService(new MusicAppDbContext());
            LoadArtists();
        }

        // Tải danh sách nghệ sĩ từ ArtistService
        private void LoadArtists()
        {
            ArtistListBox.ItemsSource = _artistService.GetArtists();
        }

        // Xử lý sự kiện khi nhấn nút Thêm nghệ sĩ
        private void AddArtist_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ArtistNameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nghệ sĩ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newArtist = new Artist
            {
                Name = ArtistNameTextBox.Text,
                Bio = ArtistBioTextBox.Text
            };
            try
            {
                _artistService.AddArtist(newArtist);
                ClearForm();
                LoadArtists();
                MessageBox.Show("Thêm nghệ sĩ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nghệ sĩ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút Sửa nghệ sĩ
        private void EditArtist_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Artist artist)
            {
                _selectedArtist = artist;
                ArtistNameTextBox.Text = artist.Name;
                ArtistBioTextBox.Text = artist.Bio;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nghệ sĩ để chỉnh sửa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút Lưu cập nhật
        private void SaveArtist_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedArtist == null)
            {
                MessageBox.Show("Vui lòng chọn một nghệ sĩ để chỉnh sửa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(ArtistNameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nghệ sĩ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _selectedArtist.Name = ArtistNameTextBox.Text;
            _selectedArtist.Bio = ArtistBioTextBox.Text;
            try
            {
                _artistService.UpdateArtist(_selectedArtist);
                ClearForm();
                LoadArtists();
                MessageBox.Show("Cập nhật nghệ sĩ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nghệ sĩ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút Xóa nghệ sĩ
        private void DeleteArtist_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Artist artist)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nghệ sĩ {artist.Name}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _artistService.DeleteArtist(artist);
                        ClearForm();
                        LoadArtists();
                        MessageBox.Show("Xóa nghệ sĩ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa nghệ sĩ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Xóa form sau khi thêm, cập nhật hoặc xóa
        private void ClearForm()
        {
            ArtistNameTextBox.Clear();
            ArtistBioTextBox.Clear();
            _selectedArtist = null;
        }
    }
}