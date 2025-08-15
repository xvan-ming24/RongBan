using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class OrderItem
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual OrderMain Order { get; set; } = null!;

    public virtual MallProduct Product { get; set; } = null!;
}
