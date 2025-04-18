using System;
using System.Collections.Generic;

namespace PRN212.Assignment.DAL.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
