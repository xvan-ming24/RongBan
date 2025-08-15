using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 用户与宠物的关联表，记录用户所拥有或关联的宠物
/// </summary>
public partial class UserPetRelation
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long PetId { get; set; }

    public byte? IsOwner { get; set; }

    public string? RelationType { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual PetInfo Pet { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
