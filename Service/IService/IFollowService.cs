using Models.Dto;
using Models.Dto.PetCircle;

namespace Service.IService
{
    public interface IFollowService
    {
        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="followerId">关注者ID（当前登录用户）</param>
        /// <param name="followedId">被关注用户ID</param>
        /// <returns>操作结果</returns>
        Task<Response<(bool IsFollowing, string Message)>> ToggleFollowAsync(long followerId, long followedId);
        /// <summary>
        /// 查询关注列表
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Task<Response<List<FollowedUserStatusDto>>> GetFollowedUsersStatusAsync(long currentUserId);
        Task<Response<List<FollowerUserDto>>> GetFollowersAsync(long currentUserId);
    }
}
