using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home
{
    public class HomeSelectedDto
    {

        public string? Nickname { get; set; }

        public string? AvatarUrl { get; set; }

        public long Id { get; set; }

        public long UserId { get; set; }

        public string Content { get; set; } = null!;

        public List<string> ImageUrls { get; set; }

        public long? LikeCount { get; set; }

        public long? CommentCount { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
