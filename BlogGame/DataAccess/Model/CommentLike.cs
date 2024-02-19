using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class CommentLike
{
    public int CommentLikeId { get; set; }

    public int CommentId { get; set; }

    public int UserId { get; set; }

    public DateTime LikeDate { get; set; }

    public virtual Comment Comment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
