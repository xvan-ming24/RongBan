using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserPoint
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int Points { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LastCheckinDate { get; set; }

    public int? ContinuousCheckinDays { get; set; }

    public int? TotalCheckinDays { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
