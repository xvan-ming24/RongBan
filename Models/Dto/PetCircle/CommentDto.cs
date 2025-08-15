using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetCircle
{
    /// <summary>
    /// 评论DTO
    /// </summary>
    public class CommentDto
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public string Content { get; set; }
        public long ParentId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
