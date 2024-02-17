using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int GameId { get; set; }

    public int UserId { get; set; }

    public int? Score { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
