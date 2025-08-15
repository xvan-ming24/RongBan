using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserTaskRecord
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long TaskId { get; set; }

    public int? CompleteTimes { get; set; }

    public DateTime? LastCompleteTime { get; set; }

    public virtual UserTask Task { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
