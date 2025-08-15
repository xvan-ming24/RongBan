using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 商品信息表，存储全品类宠物用品的信息
/// </summary>
public partial class MallProduct
{
    public long Id { get; set; }

    public string ProductName { get; set; } = null!;

    public long CategoryId { get; set; }

    public string? ApplicablePetType { get; set; }

    public decimal Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    public int? Stock { get; set; }

    public int? SalesVolume { get; set; }

    public string? Specification { get; set; }

    public string? Description { get; set; }

    public string? CoverUrl { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public byte? SeckillStatus { get; set; }

    public decimal? SeckillPrice { get; set; }

    public int? SeckillStock { get; set; }

    public int? SeckillLimit { get; set; }

    public DateTime? SeckillStartTime { get; set; }

    public DateTime? SeckillEndTime { get; set; }

    public int? SeckillVersion { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductMedium> ProductMedia { get; set; } = new List<ProductMedium>();
}
