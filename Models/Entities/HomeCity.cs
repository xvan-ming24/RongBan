using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 城市信息表，存储平台支持的城市数据
/// </summary>
public partial class HomeCity
{
    public long Id { get; set; }

    public string CityName { get; set; } = null!;

    public byte? IsDaily { get; set; }

    public byte? IsHot { get; set; }

    public int? Sort { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual ICollection<HomeActivity> HomeActivities { get; set; } = new List<HomeActivity>();

    public virtual ICollection<OrgInfo> OrgInfos { get; set; } = new List<OrgInfo>();
}
