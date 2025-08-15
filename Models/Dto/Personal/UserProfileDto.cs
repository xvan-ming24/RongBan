using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Personal
{
    public class UserProfileDto
    {
        // 用户基本信息（精简版）
        public long Id { get; set; }
        public string AvatarUrl { get; set; }
        public string Nickname { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // 统计数据
        public long TotalLikeCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public int MutualFollowCount { get; set; }
    }
}
