using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 购物车主表，关联用户与购物车，记录购物车整体信息
/// </summary>
public partial class ShoppingCart
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public decimal? TotalPrice { get; set; }

    public int? TotalCount { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual UserInfo User { get; set; } = null!;
}
