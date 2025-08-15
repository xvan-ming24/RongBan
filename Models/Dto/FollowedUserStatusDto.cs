using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto
{
    public class FollowedUserStatusDto
    {
        public long UserId { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public string PresenceName { get; set; } // 在线/离线/忙碌等
        public string PresenceColor { get; set; }
        public DateTime? LastActiveTime { get; set; }
    }
}
