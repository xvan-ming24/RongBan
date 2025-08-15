using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class PetInsurance
{
    public long Id { get; set; }

    public string InsuranceName { get; set; } = null!;

    public string Coverage { get; set; } = null!;

    public decimal Premium { get; set; }

    public int Duration { get; set; }

    public long ServiceId { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual PetMedicalService Service { get; set; } = null!;
}
