using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 用户关注关系表，记录用户间的关注关系
/// </summary>
public partial class UserFollow
{
    public long Id { get; set; }

    public long FollowerId { get; set; }

    public long FollowedId { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual UserInfo Followed { get; set; } = null!;

    public virtual UserInfo Follower { get; set; } = null!;
}
