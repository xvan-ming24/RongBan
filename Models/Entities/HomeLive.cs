using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 直播信息表，存储猫咪相关直播内容
/// </summary>
public partial class HomeLive
{
    public long Id { get; set; }

    public long? HostId { get; set; }

    public string HostName { get; set; } = null!;

    public string LiveTitle { get; set; } = null!;

    public string LiveUrl { get; set; } = null!;

    public int? OnlineNum { get; set; }

    public int? Sort { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual UserInfo? Host { get; set; }
}
