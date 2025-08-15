using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 商品分类表，存储全品类宠物用品的分类信息
/// </summary>
public partial class ProductCategory
{
    public long Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public long? ParentId { get; set; }

    public int? Sort { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<MallProduct> MallProducts { get; set; } = new List<MallProduct>();
}
