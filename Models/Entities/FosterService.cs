using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class FosterService
{
    public long Id { get; set; }

    public long OrgId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ApplicablePetType { get; set; } = null!;

    public decimal PricePerDay { get; set; }

    public string? Description { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual OrgInfo Org { get; set; } = null!;
}
