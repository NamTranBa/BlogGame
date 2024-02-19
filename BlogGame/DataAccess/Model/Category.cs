using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();
}
