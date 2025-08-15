using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetCircle
{
    // 粉丝信息DTO
    public class FollowerUserDto
    {
        public long UserId { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
    }
}
