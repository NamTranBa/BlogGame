using System;
using System.Collections.Generic;

namespace DataAccess.Model;

public partial class Comment
{
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int GameId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
