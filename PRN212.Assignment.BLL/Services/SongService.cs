using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;

namespace PRN212.Assignment.BLL.Services
{
    public class SongService
    {
        private SongListRepo _repo = new() ;
        public void CreateSong(Song song)
        {
            _repo.CreatePlayList(song) ;
        }
        public void UpdateSong(Song song)
        {
            _repo.UpdatePlayList(song);
        }
        public void DeleteSong(Song song)
        {
            _repo.DeletePlayList(song);
        }
        public List<Song> getAllSong() => _repo.getAllPlayList();
    }
}
