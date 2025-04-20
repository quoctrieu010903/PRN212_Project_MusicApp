using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;

namespace PRN212.Assignment.BLL.Services
{
    public class ArtistService
    {
        private readonly ArtistRepository _repository = new ArtistRepository();
        public List<Artist> GetArtists()
        {
            return _repository.getAllArtist();
        }
    }
}
