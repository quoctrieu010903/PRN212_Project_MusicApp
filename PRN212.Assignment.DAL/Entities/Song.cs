using System;
using System.Collections.Generic;
using static PRN212.Assignment.DAL.Entities.PlayListSongs;

namespace PRN212.Assignment.DAL.Entities;

public partial class Song
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? ArtistId { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Path { get; set; }

    public virtual Artist? Artist { get; set; }
    public  ICollection<PlaylistSong> PlaylistSongs { get; set; }
}
