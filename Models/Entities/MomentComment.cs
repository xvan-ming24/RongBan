using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠友圈评论表，记录用户对动态的评论
/// </summary>
public partial class MomentComment
{
    public long Id { get; set; }

    public long MomentId { get; set; }

    public long UserId { get; set; }

    public string Content { get; set; } = null!;

    public long? ParentId { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual PetMoment Moment { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
