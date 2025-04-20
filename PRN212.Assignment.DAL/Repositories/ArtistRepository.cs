using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212.Assignment.DAL.Entities;

namespace PRN212.Assignment.DAL.Repositories
{
    public class ArtistRepository
    {
        private  MusicAppDbContext _context;
        public List<Artist> getAllArtist()
        {
            _context = new MusicAppDbContext();
            return _context.Artists.ToList();

        }
        public void CreateArtist(Artist artist)
        {
            _context = new();
            _context.Artists.Add(artist);
            _context.SaveChanges();
        }
        public void UpdateArtist(Artist artist)
        {
            _context = new();
            _context.Artists.Remove(artist);
            _context.SaveChanges();
        }
        public void DeleteArtist(Artist artist)
        {
            _context = new();
            _context.Artists.Remove(artist);
            _context.SaveChanges();
        }
    }
}
