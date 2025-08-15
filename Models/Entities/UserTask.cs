using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserTask
{
    public long Id { get; set; }

    public string TaskName { get; set; } = null!;

    public int PointsReward { get; set; }

    public byte TaskType { get; set; }

    public int? MaxCompleteTimes { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual ICollection<UserTaskRecord> UserTaskRecords { get; set; } = new List<UserTaskRecord>();
}
