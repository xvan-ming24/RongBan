using System;
using System.Collections.Generic;

namespace Rongban.Models.Entities;

public partial class HomeCarousel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string? LinkUrl { get; set; }

    public int? SortOrder { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
