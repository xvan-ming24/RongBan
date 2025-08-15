using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class AiConsultRecord
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long PetId { get; set; }

    public string Symptoms { get; set; } = null!;

    public string? AiDiagnosis { get; set; }

    public DateTime? ConsultationTime { get; set; }

    public long ServiceId { get; set; }

    public virtual PetInfo Pet { get; set; } = null!;

    public virtual PetMedicalService Service { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
