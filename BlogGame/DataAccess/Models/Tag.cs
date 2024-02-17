using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
