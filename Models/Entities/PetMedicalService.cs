using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠物医疗服务表，包含四大医疗服务板块
/// </summary>
public partial class PetMedicalService
{
    public long Id { get; set; }

    public byte ServiceType { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public long? OrgId { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<AiConsultRecord> AiConsultRecords { get; set; } = new List<AiConsultRecord>();

    public virtual OrgInfo? Org { get; set; }

    public virtual ICollection<PetInsurance> PetInsurances { get; set; } = new List<PetInsurance>();
}
