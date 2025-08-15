using Common;
using Dao.APP.PetCircel;
using Dao.APP.PetStore;
using Models.Dto.PetCircle;
using Rongban.Models.Entities;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Dto.PetCircle.PublishUpdatesDto;

namespace Service.Service
{
    public class PetCircleService : IPetCircleService
    {
        private readonly PetCircleDao _petCircleDao;
        private const int DefaultPageIndex = 1;
        private const int DefaultPageSize = 20;
        private const int MaxPageSize = 100;
        private static readonly List<string> EmptyImageList = new List<string>();

        public PetCircleService(PetCircleDao petCircleDao)
        {
            _petCircleDao = petCircleDao;
        }
        /// <summary>
        /// 获取动态列表
        /// </summary>
        public async Task<Response<List<MomentListDto>>> GetMomentListAsync(int pageIndex, int pageSize)
        {
            pageIndex = Math.Max(pageIndex, DefaultPageIndex);
            pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

            try
            {
                var moments = await _petCircleDao.GetMomentListAsync(pageIndex, pageSize);

                var res = moments.Select(m => new MomentListDto
                {
                    Id = m.Id,
                    Nickname = m.User?.Nickname ?? string.Empty,
                    AvatarUrl = m.User?.AvatarUrl ?? string.Empty,
                    CreateTime = m.CreateTime ?? DateTime.MinValue,
                    ImageUrls = string.IsNullOrEmpty(m.ImageUrls)
                        ? EmptyImageList
                        : m.ImageUrls.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Content = m.Content ?? string.Empty,
                    LikeCount = m.LikeCount ?? 0,
                    CommentCount = m.CommentCount ?? 0
                }).ToList();
                return Response<List<MomentListDto>>.Success(res,"查询成功");
            }
            catch (Exception ex) 
            {
                LogHelper.Error<FollowService>("查询发生错误", ex);
                return Response<List<MomentListDto>>.Fail($"查询发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// 获取动态详情
        /// </summary>
        public async Task<Response<MomentDetailDto>> GetMomentDetailAsync(long momentId)
        {
            try
            {
                if (momentId <= 0)
                    return null;

                var moment = await _petCircleDao.GetMomentByIdAsync(momentId);
                if (moment == null)
                    return null;

                var comments = await _petCircleDao.GetCommentsByMomentIdAsync(momentId);

                var res = new MomentDetailDto
                {
                    Id = moment.Id,
                    Nickname = moment.User?.Nickname ?? string.Empty,
                    AvatarUrl = moment.User?.AvatarUrl ?? string.Empty,
                    CreateTime = moment.CreateTime ?? DateTime.MinValue,
                    ImageUrls = string.IsNullOrEmpty(moment.ImageUrls)
                        ? new List<string>()
                        : moment.ImageUrls.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Content = moment.Content ?? string.Empty,
                    LikeCount = moment.LikeCount ?? 0,
                    CommentCount = moment.CommentCount ?? 0,
                    // 直接内联转换逻辑，避免依赖外部类型
                    Comments = comments?.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Nickname = c.User?.Nickname ?? string.Empty,
                        AvatarUrl = c.User?.AvatarUrl ?? string.Empty,
                        Content = c.Content ?? string.Empty,
                        ParentId = c.ParentId ?? 0,
                        CreateTime = c.CreateTime ?? DateTime.MinValue
                    }).ToList() ?? new List<CommentDto>()
                };
                return Response<MomentDetailDto>.Success(res, "查询成功");
            }
            catch(Exception ex)
            {
                LogHelper.Error<PetCircleService>("查询发生错误", ex);
                return Response<MomentDetailDto>.Fail($"查询发生错误：{ex.Message}");
                
            }

        }

        public async Task<Response<PetMomentDto>> CreateMomentAsync(long userId,CreatePetMomentDto dto)
        {
            // 1. 验证业务规则：检查用户是否存在
            var user = await _petCircleDao.GetMomentByIdAsync(userId);
            if (user == null)
            {
              return Response<PetMomentDto>.Fail("用户不存在");
            }

            // 2. 验证业务规则：检查动态内容是否为空
            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return Response<PetMomentDto>.Fail("动态内容不能为空");
            }

            // 3. 转换DTO为实体对象
            var petMoment = new PetMoment
            {
                UserId = userId,                  // 发布用户ID
                Content = dto.Content.Trim(),     // 动态内容（去除首尾空格）
                ImageUrls = dto.ImageUrls?.Trim(),// 图片URLs（去除首尾空格，可为空）
                CreateTime = DateTime.Now         // 发布时间设为当前时间
                // 点赞数、评论数、分享数默认为0，不需要显式设置
            };

            // 4. 调用仓库保存到数据库
            var createdMoment = await _petCircleDao.CreateAsync(petMoment);

            // 5. 转换实体为DTO并返回
            var res = new PetMomentDto
            {
                Id = createdMoment.Id,
                UserId = userId,
                Nickname = user.User.Nickname,        
                UserAvatar = user.User.AvatarUrl,      
                Content = createdMoment.Content,
                ImageUrls = createdMoment.ImageUrls,
                LikeCount = createdMoment.LikeCount,
                CommentCount = createdMoment.CommentCount,
                ShareCount = createdMoment.ShareCount,
                CreateTime = createdMoment.CreateTime
            };
            return Response<PetMomentDto>.Success(res, "动态发布成功");
        }
    }
}
