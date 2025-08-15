using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 商品媒体表，存储商品的图片和视频资源
/// </summary>
public partial class ProductMedium
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public byte MediaType { get; set; }

    public string MediaUrl { get; set; } = null!;

    public int? Sort { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual MallProduct Product { get; set; } = null!;
}
