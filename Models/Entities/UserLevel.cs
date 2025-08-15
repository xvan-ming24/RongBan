using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserLevel
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int CutenessValue { get; set; }

    public byte Level { get; set; }

    public int NextLevelRequired { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
