using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

/// <summary>
/// 宠物多媒体表，存储宠物的图片和视频资源
/// </summary>
public partial class PetMedium
{
    public long Id { get; set; }

    public long PetId { get; set; }

    public byte MediaType { get; set; }

    public string MediaUrl { get; set; } = null!;

    public byte? IsCover { get; set; }

    public int? Sort { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual PetInfo Pet { get; set; } = null!;
}
