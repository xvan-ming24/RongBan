using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class PointsRecord
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int Points { get; set; }

    public byte SourceType { get; set; }

    public long? SourceId { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
