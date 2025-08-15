using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class VerificationCode
{
    public long Id { get; set; }

    public string Target { get; set; } = null!;

    public byte CodeType { get; set; }

    public string Code { get; set; } = null!;

    public DateTime ExpireTime { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? CreatedTime { get; set; }
}
