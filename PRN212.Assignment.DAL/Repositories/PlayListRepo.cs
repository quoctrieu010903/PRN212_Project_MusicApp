
using Microsoft.EntityFrameworkCore;
using PRN212.Assignment.DAL.Entities;
using static PRN212.Assignment.DAL.Entities.PlayListSongs;

namespace PRN212.Assignment.DAL.Repositories
{
    public class PlayListRepo
    {
      
        private MusicAppDbContext _dbContext;
       
        public List<Song> GetSongsByPlaylist(int playlistId)
        {
            _dbContext = new();
            return _dbContext.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Include(ps => ps.Song)  // Áp dụng Include trước khi Select
                .ThenInclude(s => s.Artist)  // Sau đó lấy Artist của bài hát
                .Select(ps => ps.Song)  // Chọn chỉ bài hát sau khi đã include Artist
                .ToList();
        }
        public List<PlayList> getAllPlayList()
        {
            _dbContext = new();
            return _dbContext.Playlists.Include(x=>x.PlaylistSongs).ToList();
        }
        public List<Song> getAllPlayListt()
        {
            _dbContext = new();
            return _dbContext.Songs.Include(x => x.Artist).ToList();
        }
        public List<PlaylistSong> getAllPlayLists()
        {
            _dbContext = new();
            return _dbContext.PlaylistSongs.ToList();
        }
        public void CreatePlayList(PlayList playList)
        {
            _dbContext = new();
            _dbContext.Playlists.Add(playList);
            _dbContext.SaveChanges();

        }
        public void UpdatePlayList(PlayList playList)
        {
            _dbContext = new();
            _dbContext.Playlists.Update(playList);
            _dbContext.SaveChanges();
        }
        public void DeletePlayList(PlayList playList)
        {
            _dbContext = new();
            _dbContext.Playlists.Remove(playList);
            _dbContext.SaveChanges();

        } 
    }
}
