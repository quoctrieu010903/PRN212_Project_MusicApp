using System;
using System.Collections.Generic;
using static PRN212.Assignment.DAL.Entities.PlayListSongs;

namespace PRN212.Assignment.DAL.Entities;

public partial class PlayList
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
}
