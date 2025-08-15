

using Dao.APP.Personal;
using Models.Dto.Personal;
using Service.IService;

namespace Service.Service
{
    public class PersonalInfoService: IPersonalInfoService
    {
        private readonly PersonalInfoDao _personalInfoDao;
        public PersonalInfoService(PersonalInfoDao personalInfoDao)
        {
            _personalInfoDao = personalInfoDao;
        }
        /// <summary>
        /// 根据id获取用户基本信息、社交信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<UserProfileDto>> GetUserProfile(long userId)
        {
            if (userId <= 0)
            {
                return Response<UserProfileDto>.Fail("当前用户ID无效");
            }
            var userInfo = _personalInfoDao.GetUserInfo(userId);
            if (userInfo == null)
            { 
                return Response<UserProfileDto>.Fail("当前用户不存在");
            }
            var res =  new UserProfileDto
            {
                Id = userInfo.Id,
                AvatarUrl = userInfo.AvatarUrl,
                Nickname = userInfo.Nickname,
                Bio = userInfo.Bio,
                City = userInfo.City,
                Gender = userInfo.Gender,
                Phone = userInfo.Phone,
                Email = userInfo.Email,

                TotalLikeCount = _personalInfoDao.GetTotalLikeCount(userId),
                FollowerCount = _personalInfoDao.GetFollowerCount(userId),
                FollowingCount = _personalInfoDao.GetFollowingCount(userId),
                MutualFollowCount = _personalInfoDao.GetMutualFollowCount(userId)
            };
            return Response<UserProfileDto>.Success(res,"查询成功");
        }
    }
}
