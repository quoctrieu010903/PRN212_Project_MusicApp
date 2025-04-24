

using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;
using static PRN212.Assignment.DAL.Entities.PlayListSongs;

namespace PRN212.Assignment.BLL.Services
{
    public class PlayListService
    {
        private readonly PlayListRepo _repo = new PlayListRepo();

        // Lấy tất cả playlist
        public List<PlayList> GetPlaylist()
            => _repo.getAllPlayList();
        public List<Song> GetPlaylistSong() => _repo.getAllPlayListt();
        public List<Song> GetPlaylistSong(int playlistId)
        {
            return _repo.GetSongsByPlaylist(playlistId); // Truyền playlistId vào đây
        }
        // Tạo mới playlist
        public void CreatePlaylist(PlayList playList)
            => _repo.CreatePlayList(playList);

        // Cập nhật playlist
        public void UpdatePlaylist(PlayList playList)
            => _repo.UpdatePlayList(playList);

        // Xóa playlist
        public void DeletePlayList(PlayList playList)
            => _repo.DeletePlayList(playList);
        public List<PlaylistSong> getAllSong() => _repo.getAllPlayLists();
    }
}
