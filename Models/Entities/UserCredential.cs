using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class UserCredential
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public byte CredentialType { get; set; }

    public string CredentialValue { get; set; } = null!;

    public string? OpenId { get; set; }

    public string? UnionId { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public int? ExpiresIn { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
