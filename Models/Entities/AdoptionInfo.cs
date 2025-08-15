using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 领养信息表，存储宠物领养信息（个人或机构发布）
/// </summary>
public partial class AdoptionInfo
{
    public long Id { get; set; }

    public long PetId { get; set; }

    public byte PublisherType { get; set; }

    public long PublisherId { get; set; }

    public string AdoptionRequirements { get; set; } = null!;

    public bool IsContractRequired { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual ICollection<AdoptionContract> AdoptionContracts { get; set; } = new List<AdoptionContract>();

    public virtual PetInfo Pet { get; set; } = null!;
}
