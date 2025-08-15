using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RongbanDao.APP
{
    public  class UserOnlineDao
    {
        private readonly PetPlatformDbContext _dbContext;
        private readonly TimeSpan _onlineThreshold; // 在线判断阈值

        // 构造函数注入数据库上下文，可配置阈值（默认5分钟）
        public UserOnlineDao(PetPlatformDbContext dbContext, int onlineThresholdMinutes = 1)
        {
            _dbContext = dbContext;
            _onlineThreshold = TimeSpan.FromMinutes(onlineThresholdMinutes);
        }
        /// <summary>
        /// 登录获取用户在线状态
        /// </summary>
        /// <param name="userOnlineDto"></param>
        /// <returns></returns>
        public async Task<int> RecordLoginStatusAsync(UserOnlineDto userOnlineDto)
        {

            var now = DateTime.Now;
            var existingRecord = await _dbContext.UserPresenceRecords
                .FirstOrDefaultAsync(r => r.UserId == userOnlineDto.userId && r.DeviceId == userOnlineDto.deviceId);

            if (existingRecord != null)
            {
                // 更新已有设备记录
                existingRecord.PresenceId = 1; // 在线状态
                existingRecord.LastActiveTime = now;
                existingRecord.IpAddress = userOnlineDto.ipAddress;
                _dbContext.UserPresenceRecords.Update(existingRecord);
            }
            else
            {
                // 创建新设备记录
                _dbContext.UserPresenceRecords.Add(new UserPresenceRecord
                {
                    UserId = userOnlineDto.userId,
                    DeviceId = userOnlineDto.deviceId,
                    DeviceName = userOnlineDto.deviceName,
                    IpAddress = userOnlineDto.ipAddress,
                    PresenceId = 1, // 在线状态
                    LoginTime = now,
                    LastActiveTime = now,
                    //StatusChangeTime = now
                });
            }

            // 更新用户最后登录时间
            var user = await _dbContext.UserInfos.FindAsync(userOnlineDto.userId);
            if (user != null)
            {
                user.LastLoginTime = now;
                _dbContext.UserInfos.Update(user);
            }

             return await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 处理客户端心跳，更新用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<int> UpdateUserActivityAsync(int userId, string deviceId, string ipAddress)
        {
            var session = await _dbContext.UserPresenceRecords
                .FirstOrDefaultAsync(s => s.UserId == userId && s.DeviceId == deviceId);

            if (session == null)
            {
                // 新会话
                _dbContext.UserPresenceRecords.Add(new UserPresenceRecord
                {
                    UserId = userId,
                    DeviceId = deviceId,
                    IpAddress = ipAddress,
                    LastActiveTime = DateTime.Now
                });
            }
            else
            {
                // 更新现有会话
                session.LastActiveTime = DateTime.Now;
                session.IpAddress = ipAddress; // 更新IP（可选）
            }

           return await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 判断用户在线状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserOnlineAsync(int userId)
        {

            var thresholdTime = DateTime.Now - _onlineThreshold;

            // 检查是否有任何设备满足：状态为在线 + 最后活跃时间在阈值内
            return await _dbContext.UserPresenceRecords
                .AnyAsync(r =>
                    r.UserId == userId
                    && r.PresenceId == 1 // 1 表示在线（对应presence_type表的id）
                    && r.LastActiveTime >= thresholdTime
                );
        }

        /// <summary>
        /// 获取用户详细状态（在线/离线/隐身+最后活跃时间）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetUserStatusAsync(int userId)
        {
            // 查询用户最新的活跃记录
            var latestRecord = await _dbContext.UserPresenceRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.LastActiveTime)
                .FirstOrDefaultAsync();

            if (latestRecord == null)
            {
                return "从未上线";
            }

            // 判断是否在线
            bool isOnline = latestRecord.PresenceId == 1
                && latestRecord.LastActiveTime >= DateTime.Now - _onlineThreshold;

            if (isOnline)
            {
                return "在线";
            }

            // 离线情况下，返回具体状态和最后活跃时间
            var presenceType = await _dbContext.PresenceTypes
                .FirstOrDefaultAsync(t => t.Id == latestRecord.PresenceId);

            string statusName = presenceType?.PresenceName ?? "未知状态";

            return $"{statusName}（最后活跃：{latestRecord.LastActiveTime:yyyy-MM-dd HH:mm}）";
        }
    }
}
