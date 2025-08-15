using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 用户信息表，存储平台用户的基本信息，使用手机号或邮箱登录
/// </summary>
public partial class UserInfo
{
    public long Id { get; set; }

    public string? Nickname { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Bio { get; set; }

    public byte? Gender { get; set; }

    public string? City { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public byte? BannedStatus { get; set; }

    public DateTime? RegisterTime { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public virtual ICollection<AdoptionContract> AdoptionContracts { get; set; } = new List<AdoptionContract>();

    public virtual ICollection<AiConsultRecord> AiConsultRecords { get; set; } = new List<AiConsultRecord>();

    public virtual ICollection<HomeLive> HomeLives { get; set; } = new List<HomeLive>();

    public virtual ICollection<MomentComment> MomentComments { get; set; } = new List<MomentComment>();

    public virtual ICollection<OrderMain> OrderMains { get; set; } = new List<OrderMain>();

    public virtual ICollection<PetMoment> PetMoments { get; set; } = new List<PetMoment>();

    public virtual ICollection<PointsExchange> PointsExchanges { get; set; } = new List<PointsExchange>();

    public virtual ICollection<PointsRecord> PointsRecords { get; set; } = new List<PointsRecord>();

    public virtual ICollection<ReservationMain> ReservationMains { get; set; } = new List<ReservationMain>();

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public virtual ICollection<UserCredential> UserCredentials { get; set; } = new List<UserCredential>();

    public virtual ICollection<UserFollow> UserFollowFolloweds { get; set; } = new List<UserFollow>();

    public virtual ICollection<UserFollow> UserFollowFollowers { get; set; } = new List<UserFollow>();

    public virtual UserLevel? UserLevel { get; set; }

    public virtual ICollection<UserPetRelation> UserPetRelations { get; set; } = new List<UserPetRelation>();

    public virtual UserPoint? UserPoint { get; set; }

    public virtual ICollection<UserPresenceRecord> UserPresenceRecords { get; set; } = new List<UserPresenceRecord>();

    public virtual ICollection<UserTaskRecord> UserTaskRecords { get; set; } = new List<UserTaskRecord>();
}
