using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.Home
{
    public class UserPointsDao
    {
        private readonly PetPlatformDbContext _dbContext;
        public UserPointsDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 获取用户积分信息，若不存在则自动创建
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户积分实体</returns>
        public async Task<UserPoint> GetOrCreateUserPointsAsync(long userId)
        {
            // 查询用户积分记录
            var points = await _dbContext.UserPoints.FirstOrDefaultAsync(u => u.UserId == userId);

            // 若记录不存在，初始化新记录
            if (points == null)
            {
                points = new UserPoint
                {
                    UserId = userId,
                    Points = 0,               // 初始积分
                    TotalPoints = 0,          // 累计积分
                    UpdateTime = DateTime.Now // 记录创建时间
                };
                _dbContext.UserPoints.Add(points);
                await _dbContext.SaveChangesAsync(); // 保存新记录
            }

            return points;
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 获取用户的等级信息，若不存在则自动创建初始等级记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户等级实体</returns>
        public async Task<UserLevel> GetOrCreateUserLevelAsync(long userId)
        {
            // 查询用户已有的等级记录
            var level = await _dbContext.UserLevels
                .FirstOrDefaultAsync(u => u.UserId == userId);

            // 若用户无等级记录，初始化新的等级信息
            if (level == null)
            {
                level = new UserLevel
                {
                    UserId = userId,
                    CutenessValue = 0,         // 初始萌力值为0
                    Level = 1,                 // 初始等级为v1
                    NextLevelRequired = 1001,  // v1升级到v2需要的萌力值
                    UpdateTime = DateTime.Now  // 记录创建时间
                };
                _dbContext.UserLevels.Add(level);
                await _dbContext.SaveChangesAsync();  // 保存新创建的等级记录
            }

            return level;
        }

        /// <summary>
        /// 更新用户等级信息并保存到数据库
        /// </summary>
        /// <param name="level">需要更新的用户等级实体</param>
        public async Task UpdateUserLevelAsync(UserLevel level)
        {
            // 更新记录的最后修改时间
            level.UpdateTime = DateTime.Now;

            // 标记实体为已修改状态
            _dbContext.UserLevels.Update(level);

            // 异步保存变更到数据库
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 添加积分变动记录并保存到数据库
        /// </summary>
        /// <param name="record">积分变动记录实体（包含用户ID、变动积分、来源等信息）</param>
        public async Task AddPointsRecordAsync(PointsRecord record)
        {
            // 将记录添加到数据库上下文
            _dbContext.PointsRecords.Add(record);
            // 异步保存到数据库，完成记录持久化
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 更新用户积分信息并保存到数据库
        /// </summary>
        /// <param name="point">需要更新的用户积分实体</param>
        public async Task UpdateUserPointsAsync(UserPoint point)
        {
            // 更新积分记录的最后修改时间
            point.UpdateTime = DateTime.Now;

            // 标记积分实体为已修改状态
            _dbContext.UserPoints.Update(point);

            // 异步保存变更到数据库，完成积分信息更新
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 获取所有可用的任务列表
        /// </summary>
        /// <returns>可用任务实体集合集合</returns>
        public async Task<List<UserTask>> GetAllActiveTasksAsync()
        {
            // 查询状态为"有效"（1表示有效）的所有任务
            return await _dbContext.UserTasks
                .Where(t => t.Status == 1) // 筛选状态正常的任务
                .ToListAsync(); // 异步转换为列表并返回
        }

        /// <summary>
        /// 获取指定用户的所有任务完成记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户的任务完成记录列表</returns>
        public async Task<List<UserTaskRecord>> GetUserTaskRecordsAsync(long userId)
        {
            // 查询指定用户的所有任务记录
            return await _dbContext.UserTaskRecords
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// 获取用户的特定任务记录，若不存在则自动创建新记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="taskId">任务ID</param>
        /// <returns>用户的任务记录实体</returns>
        public async Task<UserTaskRecord> GetOrCreateUserTaskRecordAsync(long userId, long taskId)
        {
            // 查询用户针对该任务的已有记录
            var record = await _dbContext.UserTaskRecords
                .FirstOrDefaultAsync(r => r.UserId == userId && r.TaskId == taskId);

            // 若记录不存在，初始化新的任务记录
            if (record == null)
            {
                record = new UserTaskRecord
                {
                    UserId = userId,
                    TaskId = taskId,
                    CompleteTimes = 0 // 初始完成次数为0
                };
                _dbContext.UserTaskRecords.Add(record);
                await _dbContext.SaveChangesAsync(); // 保存新记录
            }

            return record;
        }

        /// <summary>
        /// 更新用户任务记录并保存到数据库
        /// </summary>
        /// <param name="record">需要更新的任务记录实体（包含完成次数、最后完成时间等信息）</param>
        public async Task UpdateUserTaskRecordAsync(UserTaskRecord record)
        {
            // 标记任务记录为已修改状态
            _dbContext.UserTaskRecords.Update(record);

            // 异步保存变更到数据库，持久化更新后的任务记录
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据任务ID查询任务详情
        /// </summary>
        /// <param name="taskId">任务唯一标识ID</param>
        /// <returns>任务实体（不存在时返回null）</returns>
        public async Task<UserTask> GetTaskByIdAsync(long taskId)
        {
           
            return await _dbContext.UserTasks
                .FirstOrDefaultAsync(t => t.Id == taskId); 
        }
    }
}
