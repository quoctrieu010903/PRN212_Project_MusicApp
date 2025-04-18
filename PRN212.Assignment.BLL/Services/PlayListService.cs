

using PRN212.Assignment.DAL.Entities;
using PRN212.Assignment.DAL.Repositories;

namespace PRN212.Assignment.BLL.Services
{
    public class PlayListService
    {
        private PlayListRepo _repo = new();

        public List<PlayList> GetPlaylist() => _repo.getAllPlayList();
    }
}
