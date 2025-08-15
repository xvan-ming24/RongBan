using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠物分类表，存储全品类宠物的品种分类（猫、狗、鸟等）
/// </summary>
public partial class PetCategory
{
    public long Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public long? ParentId { get; set; }

    public int? Sort { get; set; }

    public byte? IsHot { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual ICollection<PetInfo> PetInfos { get; set; } = new List<PetInfo>();
}
