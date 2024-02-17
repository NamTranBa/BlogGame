using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int UserId { get; set; }

    public int GameId { get; set; }

    public bool IsLiked { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
