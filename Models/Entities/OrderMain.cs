using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 订单主表，存储订单整体信息
/// </summary>
public partial class OrderMain
{
    public long Id { get; set; }

    public string OrderNo { get; set; } = null!;

    public long UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public byte? PayStatus { get; set; }

    public DateTime? PayTime { get; set; }

    public byte? Status { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string ReceiverPhone { get; set; } = null!;

    public string ReceiverAddress { get; set; } = null!;

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public bool? IsSeckill { get; set; }

    public int? SeckillPayTimeout { get; set; }

    public bool? SeckillStockRecovered { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual UserInfo User { get; set; } = null!;
}
