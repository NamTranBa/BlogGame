using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
}
