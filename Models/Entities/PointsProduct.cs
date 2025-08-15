using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class PointsProduct
{
    public long Id { get; set; }

    public string ProductName { get; set; } = null!;

    public int PointsRequired { get; set; }

    public int Stock { get; set; }

    public string? CoverUrl { get; set; }

    public string? Description { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<PointsExchange> PointsExchanges { get; set; } = new List<PointsExchange>();
}
