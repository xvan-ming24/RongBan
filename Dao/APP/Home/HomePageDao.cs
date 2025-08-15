using Microsoft.EntityFrameworkCore;
using Models.Dto.Home;
using Rongban.Models.Entities;

namespace Dao.APP.Home
{
    public class HomePageDao
    {
        private readonly PetPlatformDbContext _dbContext;
        private static readonly List<string> EmptyImageList = new List<string>();
        public HomePageDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取首页轮播图
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<HomeCarousel>> GetHomeCarousels()
        {
            try
            {
                await Task.Delay(100);

                var data = await _dbContext.HomeCarousels.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取首页直播
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<HomeLiveDto>> GetHomeLives()
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.HomeLives
                    .Include(h => h.Host)
                    .Select(h => new HomeLiveDto
                    {
                        Id = h.Id,
                        HostId = h.HostId,
                        HostNickName = h.Host.Nickname,
                        LiveTitle = h.LiveTitle,
                        LiveUrl = h.LiveUrl,
                        OnlineNum = h.OnlineNum,
                        Sort = h.Sort,
                        LiveStatus = h.Status,
                        CreateTime = h.CreateTime,
                    })
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取首页精选
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<HomeSelectedDto>> GetHomeSelectedDto()
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.PetMoments
                    .Include(h => h.User)
                    .Select(h => new HomeSelectedDto
                    {
                        Id = h.Id,
                        UserId = h.UserId,
                        Content = h.Content,
                       // ImageUrls = h.ImageUrls,
                        ImageUrls = string.IsNullOrEmpty(h.ImageUrls)
                        ? EmptyImageList
                        : h.ImageUrls.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                        LikeCount = h.LikeCount,
                        CommentCount = h.CommentCount,
                        CreateTime = h.CreateTime,
                        AvatarUrl = h.User.AvatarUrl,
                        Nickname = h.User.Nickname
                    }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取数据失败: {ex.Message}", ex);
            }


        }
    }
}
