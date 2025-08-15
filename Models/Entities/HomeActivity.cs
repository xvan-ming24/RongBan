using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 活动信息表，存储平台举办的各类猫咪相关活动
/// </summary>
public partial class HomeActivity
{
    public long Id { get; set; }

    public string ActivityName { get; set; } = null!;

    public string? CoverUrl { get; set; }

    public long? CityId { get; set; }

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual HomeCity? City { get; set; }
}
