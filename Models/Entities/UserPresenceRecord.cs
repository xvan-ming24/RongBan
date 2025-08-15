using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserPresenceRecord
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public byte PresenceId { get; set; }

    public string DeviceId { get; set; } = null!;

    public string? DeviceName { get; set; }

    public string? IpAddress { get; set; }

    public DateTime? LoginTime { get; set; }

    public DateTime? LastActiveTime { get; set; }

    public virtual PresenceType Presence { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
