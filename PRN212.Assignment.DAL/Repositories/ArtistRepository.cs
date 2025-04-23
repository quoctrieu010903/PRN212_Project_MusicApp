using System;
using System.Collections.Generic;
using System.Linq;
using PRN212.Assignment.DAL.Entities;

namespace PRN212.Assignment.DAL.Repositories
{
    public class ArtistRepository
    {
        private readonly MusicAppDbContext _context;

        public ArtistRepository(MusicAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Artist> getAllArtist()
        {
            return _context.Artists.ToList();
        }

        public void CreateArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            _context.SaveChanges();
        }

        public void UpdateArtist(Artist artist)
        {
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