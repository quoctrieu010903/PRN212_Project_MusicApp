using System;
using System.Collections.Generic;
using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;

namespace PRN212.Assignment.BLL.Services
{
    public class ArtistService
    {
        private readonly ArtistRepository _repository;

        public ArtistService(MusicAppDbContext context)
        {
            _repository = new ArtistRepository(context);
        }

        public ArtistService()
        {
        }

        public List<Artist> GetArtists()
        {
            return _repository.getAllArtist();
        }

        public void AddArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist), "Thông tin nghệ sĩ không được để trống.");
            }
            _repository.CreateArtist(artist);
        }

        public void UpdateArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist), "Thông tin nghệ sĩ không được để trống.");
            }
            _repository.UpdateArtist(artist);
        }

        public void DeleteArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist), "Thông tin nghệ sĩ không được để trống.");
            }
            _repository.DeleteArtist(artist);
        }
    }
}