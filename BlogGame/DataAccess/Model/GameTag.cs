using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class GameTag
{
    public int GameId { get; set; }

    public int TagId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
