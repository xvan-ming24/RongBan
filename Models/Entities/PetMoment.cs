using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠友圈动态表，存储用户发布的全品类宠物相关动态
/// </summary>
public partial class PetMoment
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? ImageUrls { get; set; }

    public long? LikeCount { get; set; }

    public long? CommentCount { get; set; }

    public long? ShareCount { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual ICollection<MomentComment> MomentComments { get; set; } = new List<MomentComment>();

    public virtual UserInfo User { get; set; } = null!;
}
