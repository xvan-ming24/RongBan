using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 预约主表，统一管理医疗、寄养、美容等各类预约
/// </summary>
public partial class ReservationMain
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public byte ReservationType { get; set; }

    public long RelatedServiceId { get; set; }

    public DateOnly ReservationDate { get; set; }

    public TimeOnly ReservationTime { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();

    public virtual UserInfo User { get; set; } = null!;
}
