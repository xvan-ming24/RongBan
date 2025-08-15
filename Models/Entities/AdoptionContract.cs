using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class AdoptionContract
{
    public long Id { get; set; }

    public long AdoptionId { get; set; }

    public long UserId { get; set; }

    public long OrgId { get; set; }

    public string ContractContent { get; set; } = null!;

    public DateTime? SignTime { get; set; }

    public byte? Status { get; set; }

    public virtual AdoptionInfo Adoption { get; set; } = null!;

    public virtual OrgInfo Org { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
