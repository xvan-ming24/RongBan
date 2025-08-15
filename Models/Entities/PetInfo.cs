using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠物信息表，存储全品类宠物的详细信息
/// </summary>
public partial class PetInfo
{
    public long Id { get; set; }

    public string PetName { get; set; } = null!;

    public long CategoryId { get; set; }

    public string? Breed { get; set; }

    public int? Age { get; set; }

    public byte? Gender { get; set; }

    public decimal? Weight { get; set; }

    public string? Characteristic { get; set; }

    public byte? Sterilization { get; set; }

    public string? Vaccine { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<AdoptionInfo> AdoptionInfos { get; set; } = new List<AdoptionInfo>();

    public virtual ICollection<AiConsultRecord> AiConsultRecords { get; set; } = new List<AiConsultRecord>();

    public virtual PetCategory Category { get; set; } = null!;

    public virtual ICollection<PetMedium> PetMedia { get; set; } = new List<PetMedium>();

    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();

    public virtual ICollection<UserPetRelation> UserPetRelations { get; set; } = new List<UserPetRelation>();
}
