using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class SequenceCounter
{
    public int Id { get; set; }

    public long CurrentValue { get; set; }

    public string SequenceName { get; set; } = null!;

    public string? Description { get; set; }
}
