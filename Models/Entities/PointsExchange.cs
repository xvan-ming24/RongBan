using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class PointsExchange
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ProductId { get; set; }

    public int PointsUsed { get; set; }

    public DateTime? ExchangeTime { get; set; }

    public byte? Status { get; set; }

    public virtual PointsProduct Product { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
