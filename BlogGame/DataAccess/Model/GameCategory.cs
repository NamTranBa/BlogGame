using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class GameCategory
{
    public int GameId { get; set; }

    public int CategoryId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
