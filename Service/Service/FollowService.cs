using Common;
using Models.Dto;
using Models.Dto.PetCircle;
using Rongban.Models.Entities;
using RongbanServeice;
using Service.IService;

namespace Service.Service
{
    public class FollowService : IFollowService
    {
        private readonly FollowDao _followDao;

        public FollowService(FollowDao followDao)
        {
            _followDao = followDao;
        }
        /// <summary>
        /// 关注/取消关注
        /// </summary>
        /// <param name="followerId"></param>
        /// <param name="followedId"></param>
        /// <returns></returns>

        public async Task<Response<(bool IsFollowing, string Message)>> ToggleFollowAsync(long followerId, long followedId)
        {
            if (followerId <= 0)
                return Response<(bool,string)>.Fail("当前用户ID无效");

            if (followedId <= 0)
                return Response<(bool,string)>.Fail("被关注用户ID无效");

            if (followerId == followedId)
            {
                return Response<(bool, string)>.Fail("不能关注自己");
            }
            try 
            {
                LogHelper.Info<FollowService>($"关注用户: {followedId}");
                //执行关注
                var follow = new UserFollow
                {
                    FollowerId = followerId,
                    FollowedId = followedId,
                    CreateTime = DateTime.Now
                };

                var result = await _followDao.ToggleFollowAsync(follow);
                var message = result.IsFollowing ? "关注成功" : "取消关注成功";
                return Response<(bool, string)>.Success((result.IsFollowing, message));
            }
            catch (Exception ex)
            {
                LogHelper.Error<FollowService>($"关注用户失败: {followedId}", ex);
                return Response<(bool, string)>.Fail($"关注失败：{ex.Message}");
            }


        }
        /// <summary>
        /// 根据id获取用户关注列表
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<Response<List<FollowedUserStatusDto>>> GetFollowedUsersStatusAsync(long currentUserId)
        {
            
            if (currentUserId <= 0)
            {
                return Response<List<FollowedUserStatusDto>>.Fail("无效的用户ID");
            }

            try
            {
                LogHelper.Info<FollowService>($"根据id获取用户关注列表");
                // 调用DAL层获取数据
                var result = await _followDao.GetFollowedUsersStatusAsync(currentUserId);
                return Response<List<FollowedUserStatusDto>>.Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<FollowService>($"获取用户关注列表: {currentUserId}", ex);
                return Response<List<FollowedUserStatusDto>>.Fail($"查询失败：{ex.Message}");
            }
        }
        /// <summary>
        /// 根据id获取用户粉丝列表
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<Response<List<FollowerUserDto>>> GetFollowersAsync(long currentUserId)
        {

            if (currentUserId <= 0)
            {
                return Response<List<FollowerUserDto>>.Fail("无效的用户ID");
            }

            try
            {
                LogHelper.Info<FollowService>($"根据id获取用户关注列表");
                // 调用DAL层获取数据
                var result = await _followDao.GetFollowersAsync(currentUserId);
                return Response<List<FollowerUserDto>>.Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<FollowService>($"获取用户粉丝列表: {currentUserId}", ex);
                return Response<List<FollowerUserDto>>.Fail($"查询失败：{ex.Message}");
            }
        }
    }
}
