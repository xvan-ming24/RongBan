using Dao.APP.Home;
using Models.Dto.Home;
using Models.Dto.Home.SignIn;
using Models.Dto.PetCircle;
using Rongban.Models.Entities;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class HomeVajraService: IHomeVajraService
    {
        private readonly AdoptionPetDao _adoptionPetDao;
        private readonly FosterOrgDao _fosterOrgDao;
        private readonly UserPointsDao _userPointsDao;
        

        public HomeVajraService(AdoptionPetDao adoptionPetDao, FosterOrgDao fosterOrgDao, UserPointsDao userPointsDao)
        {
            _adoptionPetDao = adoptionPetDao;
            _fosterOrgDao = fosterOrgDao;
            _userPointsDao = userPointsDao;
        }
        public async Task<Response<List<AdoptPetListDto>>> GetAdoptionListAsync()
        {
            var adoptionInfos = await _adoptionPetDao.GetAdoptionListAsync();

            var res = adoptionInfos.Select(a => new AdoptPetListDto
            {
                AdoptionId = a.Id,
                AdoptionRequirements = a.AdoptionRequirements,
                PetNickname = a.Pet.PetName,
                PetAge = a.Pet.Age,
                PetGender = a.Pet.Gender,
                PetCharacteristics = a.Pet.Characteristic,
                IsSterilized = a.Pet.Sterilization,
                VaccineStatus = a.Pet.Vaccine,
                // 处理宠物照片：筛选图片类型(1)，优先选择封面(1)，按排序升序
                PetPhotoUrl = a.Pet.PetMedia
                    .Where(m => m.MediaType == 1) // 只取图片类型
                    .OrderByDescending(m => m.IsCover) // 封面图优先
                    .ThenBy(m => m.Sort) // 然后按排序号
                    .FirstOrDefault()?.MediaUrl, // 取第一个的URL
                
            }).ToList();
            return Response<List<AdoptPetListDto>>.Success(res, "获取成功");
        }

        public async Task<Response<AdoptPetDetailDto>> GetAdoptionDetailAsync(long id)
        {
            var adoptionInfo = await _adoptionPetDao.GetAdoptionByIdAsync(id);

            if (adoptionInfo == null)
                return null;

            // 获取所有宠物图片并按排序号排序
            var petPhotos = adoptionInfo.Pet.PetMedia
                .Where(m => m.MediaType == 1) // 只取图片类型
                .OrderBy(m => m.Sort) // 按排序号排序
                .Select(m => m.MediaUrl) // 提取URL
                .ToList();
            var res =  new AdoptPetDetailDto
            {
                // 领养信息ID
                Id = adoptionInfo.Id,

                // 宠物信息
                PetId = adoptionInfo.PetId,
                PetNickname = adoptionInfo.Pet.PetName,
                PetAge = adoptionInfo.Pet.Age,
                PetGender = adoptionInfo.Pet.Gender,
                PetCharacteristics = adoptionInfo.Pet.Characteristic,
                IsSterilized = adoptionInfo.Pet.Sterilization,
                VaccineStatus = adoptionInfo.Pet.Vaccine,
                PetPhotoUrls = petPhotos, // 所有图片URL列表

                // 领养信息详情
                AdoptionRequirements = adoptionInfo.AdoptionRequirements,
                PublisherType = adoptionInfo.PublisherType,
                PublisherId = adoptionInfo.PublisherId,
                IsContractRequired = adoptionInfo.IsContractRequired,
                Status = adoptionInfo.Status
            };
            return Response<AdoptPetDetailDto>.Success(res, "获取成功");
        }
        /// <summary>
        /// 获取可寄养的门店列表
        /// </summary>
        /// <returns>门店列表DTO</returns>
        public async Task<Response<List<FosterOrgListDto>>> GetFosterOrgListAsync()
        {
            var orgs = await _fosterOrgDao.GetFosterOrgsAsync();

            // 转换为DTO
            var res = orgs.Select(org => new FosterOrgListDto
            {
                Id = org.Id,
                OrgName = org.OrgName,
                Address = org.Address
            }).ToList();
            return Response<List<FosterOrgListDto>>.Success(res, "获取成功");
        }


        /// <summary>
        /// 处理用户签到逻辑，返回签到结果
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>包含签到状态、奖励等信息的结果DTO</returns>
        public async Task<Response<CheckinResultDto>> CheckinAsync(long userId)
        {
            // 获取获取或创建用户积分记录
            var userPoints = await _userPointsDao.GetOrCreateUserPointsAsync(userId);
            var today = DateTime.Today;
            var res= new CheckinResultDto();

            // 检查是否已签到（今日）
            if (userPoints.LastCheckinDate.HasValue && userPoints.LastCheckinDate.Value == today)
            {
                res = new CheckinResultDto
                {
                    Success = false,
                    ContinuousDays = userPoints.ContinuousCheckinDays,
                    CurrentPoints = userPoints.Points,
                    CurrentCuteness = (await _userPointsDao.GetOrCreateUserLevelAsync(userId)).CutenessValue
                };
                return Response<CheckinResultDto>.Success(res, "今日已签到");
            }

            // 计算连续签到天数（若前一天已签则累加，否则重置为1）
            int continuousDays = 1;
            if (userPoints.LastCheckinDate.HasValue &&
                userPoints.LastCheckinDate.Value == today.AddDays(-1))
            {
                continuousDays = (int)(userPoints.ContinuousCheckinDays + 1);
            }

            // 计算积分奖励（基础10分，连续5/7天有额外奖励）
            int pointsRewarded = 10;
            if (continuousDays == 5) pointsRewarded += 100;
            if (continuousDays == 7) pointsRewarded += 200;

            // 计算萌力值奖励（基础10点，连续5/7天有额外奖励）
            int cutenessRewarded = 10;
            if (continuousDays == 5) cutenessRewarded += 100;
            if (continuousDays >= 7) cutenessRewarded += 200;

            // 更新用户积分信息
            userPoints.Points += pointsRewarded;
            userPoints.TotalPoints += pointsRewarded;
            userPoints.ContinuousCheckinDays = continuousDays;
            userPoints.TotalCheckinDays += 1;
            userPoints.LastCheckinDate = today;
            await _userPointsDao.UpdateUserPointsAsync(userPoints);

            // 记录积分变动日志（来源类型1表示签到）
            await _userPointsDao.AddPointsRecordAsync(new PointsRecord
            {
                UserId = userId,
                Points = pointsRewarded,
                SourceType = 1,
                Remark = $"连续签到{continuousDays}天奖励"
            });

            // 更新用户萌力值（含积分转化部分：每100积分兑换5点萌力值）
            var userLevel = await _userPointsDao.GetOrCreateUserLevelAsync(userId);
            userLevel.CutenessValue += cutenessRewarded;
            userLevel.CutenessValue += (pointsRewarded / 100) * 5;
            UpdateUserLevel(userLevel); // 更新等级信息
            await _userPointsDao.UpdateUserLevelAsync(userLevel);

            // 提交所有数据变更
            await _userPointsDao.CommitAsync();

             res = new CheckinResultDto
            {
                Success = true,
                PointsRewarded = pointsRewarded,
                CutenessRewarded = cutenessRewarded,
                ContinuousDays = continuousDays,
                CurrentPoints = userPoints.Points,
                CurrentCuteness = userLevel.CutenessValue
            };
            return Response<CheckinResultDto>.Success(res, "签到成功");
        }

        /// <summary>
        /// 根据萌力值更新用户等级及下一等级所需值
        /// </summary>
        /// <param name="userLevel">用户等级实体</param>
        private void UpdateUserLevel(UserLevel userLevel)
        {
            // 等级规则：v1(0-1000) → v2(1001-3000) → v3(3001-6000) → v4(6001-10000) → v5(10000+)
            if (userLevel.CutenessValue <= 1000)
            {
                userLevel.Level = 1;
                userLevel.NextLevelRequired = 1001;
            }
            else if (userLevel.CutenessValue <= 3000)
            {
                userLevel.Level = 2;
                userLevel.NextLevelRequired = 3001;
            }
            else if (userLevel.CutenessValue <= 6000)
            {
                userLevel.Level = 3;
                userLevel.NextLevelRequired = 6001;
            }
            else if (userLevel.CutenessValue <= 10000)
            {
                userLevel.Level = 4;
                userLevel.NextLevelRequired = 10001;
            }
            else
            {
                userLevel.Level = 5;
                userLevel.NextLevelRequired = 0; // v5为最高等级，无需再升级
            }
        }

        /// <summary>
        /// 获取用户今日可执行的任务列表（含完成状态）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>任务列表DTO</returns>
        public async Task<Response<List<UserTaskDto>>> GetTodayTasksAsync(long userId)
        {
            var allTasks = await _userPointsDao.GetAllActiveTasksAsync(); // 获取所有有效任务
            var userTaskRecords = await _userPointsDao.GetUserTaskRecordsAsync(userId); // 获取用户任务记录
            var today = DateTime.Today;

            var res = allTasks.Select(task =>
            {
                var record = userTaskRecords.FirstOrDefault(r => r.TaskId == task.Id);

                // 每日任务重置：若最后完成时间不是今天，则重置完成次数
                if (record != null && record.LastCompleteTime?.Date < today)
                {
                    record.CompleteTimes = 0;
                    _userPointsDao.UpdateUserTaskRecordAsync(record);
                }

                // 构建任务DTO（判断是否已完成：今日完成次数达到上限）
                return new UserTaskDto
                {
                    TaskId = task.Id,
                    TaskName = task.TaskName,
                    Reward = task.PointsReward,
                    IsCompleted = record != null &&
                                  record.CompleteTimes >= task.MaxCompleteTimes &&
                                  record.LastCompleteTime?.Date == today
                };
            }).ToList();
            return Response<List<UserTaskDto>>.Success(res, "获取任务列表成功");
        }

        /// <summary>
        /// 处理任务完成逻辑，返回完成结果
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="taskId">任务ID</param>
        /// <returns>包含任务完成状态、奖励等信息的结果DTO</returns>
        public async Task<Response<TaskCompleteResultDto>> CompleteTaskAsync(long userId, long taskId)
        {
            // 验证任务有效性（存在且状态为有效）
            var task = await _userPointsDao.GetTaskByIdAsync(taskId);
            if (task == null || task.Status != 1)
            {
                return Response<TaskCompleteResultDto>.SuccessWithoutData("任务不存在或已失效");
            }

            // 获取用户该任务的记录（不存在则创建）
            var record = await _userPointsDao.GetOrCreateUserTaskRecordAsync(userId, taskId);
            var today = DateTime.Today;

            // 检查是否已完成今日任务（达到最大完成次数）
            if (record.CompleteTimes >= task.MaxCompleteTimes &&
                record.LastCompleteTime?.Date == today)
            {
                return Response<TaskCompleteResultDto>.SuccessWithoutData("任务完成成功");
            }

            // 更新任务完成记录（次数+1，更新最后完成时间）
            record.CompleteTimes++;
            record.LastCompleteTime = DateTime.Now;
            await _userPointsDao.UpdateUserTaskRecordAsync(record);

            // 增加任务奖励的萌力值
            var userLevel = await _userPointsDao.GetOrCreateUserLevelAsync(userId);
            userLevel.CutenessValue += task.PointsReward;
            await _userPointsDao.UpdateUserLevelAsync(userLevel);

            // 提交数据变更
            await _userPointsDao.CommitAsync();

            var res = new TaskCompleteResultDto
            {
                Success = true,
                Reward = task.PointsReward,
                TotalCuteness = userLevel.CutenessValue
            };
            return Response<TaskCompleteResultDto>.Success(res, "任务完成成功");
        }
    }
}
