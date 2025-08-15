using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.Personal
{
    public class PersonalInfoDao
    {
        private readonly PetPlatformDbContext _dbContext;
        public PersonalInfoDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 获取用户基本信息
        public UserInfo GetUserInfo(long userId)
        {
            return _dbContext.UserInfos.Find(userId);
        }

        // 获取用户所有动态的获赞总数
        public long GetTotalLikeCount(long userId)
        {
            return _dbContext.PetMoments
                .Where(m => m.UserId == userId)
                .Sum(m => m.LikeCount) ?? 0;
        }

        // 获取粉丝数量
        public int GetFollowerCount(long userId)
        {
            return _dbContext.UserFollows
                .Where(f => f.FollowedId == userId)
                .Count();
        }

        // 获取关注数量
        public int GetFollowingCount(long userId)
        {
            return _dbContext.UserFollows
                .Where(f => f.FollowerId == userId)
                .Count();
        }

        // 获取互关数量
        public int GetMutualFollowCount(long userId)
        {
            // 查询既关注了当前用户，同时又被当前用户关注的用户数量
            return _dbContext.UserFollows
                .Where(f1 => f1.FollowedId == userId) // 关注我的人
                .Join(
                    _dbContext.UserFollows.Where(f2 => f2.FollowerId == userId), // 我关注的人
                    f1 => f1.FollowerId,
                    f2 => f2.FollowedId,
                    (f1, f2) => f1.FollowerId
                )
                .Distinct()
                .Count();
        }
    }
}
