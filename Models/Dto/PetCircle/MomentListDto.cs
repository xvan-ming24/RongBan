using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetCircle
{
    /// <summary>
    /// 动态列表DTO
    /// </summary>
    public class MomentListDto
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Content { get; set; }  // 作为标题/内容
        public long LikeCount { get; set; }
        public long CommentCount { get; set; }
    }
}
