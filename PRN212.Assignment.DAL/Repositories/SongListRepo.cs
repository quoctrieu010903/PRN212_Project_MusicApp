using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN212.Assignment.DAL.Entities;

namespace PRN212.Assignment.DAL.Repositories
{
    public class SongListRepo
    {
    
        private MusicAppDbContext _dbContext;

        public List<Song> getAllPlayList()
        {
            _dbContext = new();
            return _dbContext.Songs.Include(x=> x.Artist).ToList();
        }
        public void CreateSong(Song  song)
        {
            _dbContext = new();
            _dbContext.Songs.Add(song);
            _dbContext.SaveChanges();

        }
        public void UpdateSong(Song song)
        {
            _dbContext = new();
            _dbContext.Songs.Update(song);
            _dbContext.SaveChanges();
        }
        public void DeleteSong(Song song)
        {
            _dbContext = new();
            _dbContext.Songs.Remove(song);
            _dbContext.SaveChanges();

        }
        public int getTotalSong(Song song)
        {
            _dbContext= new();
            return _dbContext.Songs.Count();
        }
    }
}
