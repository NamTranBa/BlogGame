using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? ReleaseDate { get; set; }

    public string? Developer { get; set; }

    public string? Publisher { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();

    public virtual ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
