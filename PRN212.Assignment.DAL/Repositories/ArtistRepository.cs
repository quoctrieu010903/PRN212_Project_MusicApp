using System;
using System.Collections.Generic;
using System.Linq;
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
            if (artist == null || artist.ArtistId <= 0)
            {
                throw new ArgumentException("Thông tin nghệ sĩ không hợp lệ hoặc thiếu ArtistId.");
            }

            var existingArtist = _context.Artists.Find(artist.ArtistId);
            if (existingArtist != null)
            {
                existingArtist.Name = artist.Name;
                existingArtist.Bio = artist.Bio; // Cập nhật Bio
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Nghệ sĩ không tồn tại trong cơ sở dữ liệu.");
            }
        }

        public void DeleteArtist(Artist artist)
        {
            _context = new();
            var existingArtist = _context.Artists.Find(artist.ArtistId);
            if (existingArtist != null)
            {
                _context.Artists.Remove(existingArtist);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Nghệ sĩ không tồn tại.");
            }
        }
    }
}