using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212.Assignment.DAL.Entities
{
    public class PlayListSongs
    {
        public class PlaylistSong
        {
            public int PlaylistId { get; set; }
            public PlayList Playlist { get; set; }

            public int SongId { get; set; }
            public Song Song { get; set; }
        }
    }
}
