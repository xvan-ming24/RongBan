using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetCircle
{
    /// <summary>
    /// 动态详情DTO
    /// </summary>
    public class MomentDetailDto
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Content { get; set; }
        public long LikeCount { get; set; }
        public long CommentCount { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
