using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 购物车商品项表，记录购物车中的具体商品及数量等信息
/// </summary>
public partial class CartItem
{
    public long Id { get; set; }

    public long CartId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int Quantity { get; set; }

    public byte? IsSelected { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;

    public virtual MallProduct Product { get; set; } = null!;
}
