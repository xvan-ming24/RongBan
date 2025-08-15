using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 系统配置表，存储平台各类配置信息
/// </summary>
public partial class SysConfig
{
    public long Id { get; set; }

    public string ConfigKey { get; set; } = null!;

    public string? ConfigValue { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }
}
