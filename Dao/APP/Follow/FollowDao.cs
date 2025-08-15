using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Dto.PetCircle;
using Rongban.Models.Entities;

public class FollowDao
{
    private readonly PetPlatformDbContext _dbContext; // 注入EF Core上下文

    public FollowDao(PetPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 关注或取消关注用户（如果已关注则取消，未关注则添加）
    /// </summary>
    /// <param name="followerId">关注者ID</param>
    /// <param name="followedId">被关注者ID</param>
    /// <returns>操作结果，true表示操作成功，false表示失败；同时返回当前状态，true为已关注，false为已取消</returns>
    public async Task<(bool Success, bool IsFollowing)> ToggleFollowAsync(UserFollow userFollow)
    {
        // 查找现有关注关系
        var existingFollow = await _dbContext.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == userFollow.FollowerId && f.FollowedId == userFollow.FollowedId);

        if (existingFollow != null)
        {
            // 已关注，执行取消关注
            _dbContext.UserFollows.Remove(existingFollow);
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return (rowsAffected > 0, false); // 操作成功，当前状态为未关注
        }
        else
        {
            // 未关注，执行添加关注
            var newFollow = new UserFollow
            {
                FollowerId = userFollow.FollowerId,
                FollowedId = userFollow.FollowedId,
                CreateTime = DateTime.Now
            };
            _dbContext.UserFollows.Add(newFollow);
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return (rowsAffected > 0, true); // 操作成功，当前状态为已关注
        }
    }
    /// <summary>
    /// 获取用户关注列表
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public async Task<List<FollowedUserStatusDto>> GetFollowedUsersStatusAsync(long currentUserId)
    {
        // 构建查询（对应之前的SQL逻辑）
        var query = from uf in _dbContext.Set<UserFollow>()
                    join ui in _dbContext.Set<UserInfo>() on uf.FollowedId equals ui.Id
                    join upr in _dbContext.Set<UserPresenceRecord>()
                        on uf.FollowedId equals upr.UserId into presenceGroup
                    from upr in presenceGroup.DefaultIfEmpty() // 左连接，包含无状态记录的用户
                    join pt in _dbContext.Set<PresenceType>()
                        on upr.PresenceId equals pt.Id into typeGroup
                    from pt in typeGroup.DefaultIfEmpty()
                    where uf.FollowerId == currentUserId && ui.BannedStatus == 1
                    group new { ui, upr, pt } by new
                    {
                        ui.Id,
                        ui.Nickname,
                        ui.AvatarUrl,
                        pt.PresenceName,
                        pt.PresenceColor
                    } into g
                    orderby
                        // 排序：在线状态优先（假设PresenceName为"在线"时优先）
                        (g.Key.PresenceName == "在线" ? 1 : 0) descending,
                        g.Max(x => x.upr.LastActiveTime) descending
                    select new FollowedUserStatusDto
                    {
                        UserId = g.Key.Id,
                        Nickname = g.Key.Nickname,
                        AvatarUrl = g.Key.AvatarUrl,
                        PresenceName = g.Key.PresenceName ?? "离线", // 默认为离线
                        PresenceColor = g.Key.PresenceColor ?? "#999", // 默认灰色
                        LastActiveTime = g.Max(x => x.upr.LastActiveTime)
                    };

        return await query.ToListAsync();
    }
    /// <summary>
    /// 获取用户的粉丝列表（使用导航属性优化）
    /// </summary>
    /// <param name="currentUserId">当前用户ID</param>
    /// <returns>粉丝列表，包含用户ID、昵称和头像URL</returns>
    public async Task<List<FollowerUserDto>> GetFollowersAsync(long currentUserId)
    {
        // 利用导航属性查询，无需显式Join
        var followers = await _dbContext.UserFollows
            // 筛选条件：关注当前用户的记录
            .Where(follow => follow.FollowedId == currentUserId)
            // 预加载粉丝的用户信息（避免延迟加载导致的额外查询）
            .Include(follow => follow.Follower) // 假设UserFollow实体有Follower导航属性指向UserInfo
                                                // 过滤掉被封禁的用户
            .Where(follow => follow.Follower.BannedStatus == 1)
            // 投影到DTO
            .Select(follow => new FollowerUserDto
            {
                UserId = follow.Follower.Id,
                Nickname = follow.Follower.Nickname,
                AvatarUrl = follow.Follower.AvatarUrl
            })
            // 异步获取结果
            .ToListAsync();

        return followers;
    }

}