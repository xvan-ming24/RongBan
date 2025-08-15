using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class PresenceType
{
    public byte Id { get; set; }

    public string PresenceName { get; set; } = null!;

    public string? Description { get; set; }

    public string? PresenceColor { get; set; }

    public byte? SortOrder { get; set; }

    public virtual ICollection<UserPresenceRecord> UserPresenceRecords { get; set; } = new List<UserPresenceRecord>();
}
