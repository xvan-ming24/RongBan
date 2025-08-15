using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 机构信息表，存储宠物店、宠物医院等机构信息
/// </summary>
public partial class OrgInfo
{
    public long Id { get; set; }

    public string OrgName { get; set; } = null!;

    public byte OrgType { get; set; }

    public string Address { get; set; } = null!;

    public long CityId { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public string? ContactPerson { get; set; }

    public string ContactPhone { get; set; } = null!;

    public string? LicenseUrl { get; set; }

    public string? OpeningHours { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<AdoptionContract> AdoptionContracts { get; set; } = new List<AdoptionContract>();

    public virtual HomeCity City { get; set; } = null!;

    public virtual ICollection<FosterService> FosterServices { get; set; } = new List<FosterService>();

    public virtual ICollection<PetBeautyService> PetBeautyServices { get; set; } = new List<PetBeautyService>();

    public virtual ICollection<PetMedicalService> PetMedicalServices { get; set; } = new List<PetMedicalService>();
}
