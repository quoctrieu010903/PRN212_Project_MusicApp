
using Microsoft.EntityFrameworkCore;
using PRN212.Assignment.DAL.Entities;

namespace PRN212.Assignment.DAL.Repositories
{
    public class PlayListRepo
    {
        private MusicAppDbContext _dbContext;

        public List<PlayList> getAllPlayList()
        {
            _dbContext = new();
            return _dbContext.Playlists.Include(x=>x.PlaylistSongs).ToList();
        }
        public void CreatePlayList(PlayList playList)
        {
            _dbContext.Playlists.Add(playList);
            _dbContext.SaveChanges();

        }
        public void UpdatePlayList(PlayList playList)
        {
            _dbContext.Playlists.Update(playList);
            _dbContext.SaveChanges();
        }
        public void DeletePlayList(PlayList playList)
        {
            _dbContext.Playlists.Remove(playList);
            _dbContext.SaveChanges();

        } 
    }
}
