using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class ReservationDetail
{
    public long Id { get; set; }

    public long ReservationId { get; set; }

    public long PetId { get; set; }

    public string? Notes { get; set; }

    public decimal ServiceFee { get; set; }

    public byte? PaymentStatus { get; set; }

    public virtual PetInfo Pet { get; set; } = null!;

    public virtual ReservationMain Reservation { get; set; } = null!;
}
