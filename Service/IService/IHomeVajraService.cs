using Models.Dto.Home;
using Models.Dto.Home.SignIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHomeVajraService
    {
        Task<Response<List<AdoptPetListDto>>> GetAdoptionListAsync();
        Task<Response<AdoptPetDetailDto>> GetAdoptionDetailAsync(long id);
        Task<Response<List<FosterOrgListDto>>> GetFosterOrgListAsync();
        // 用户签到
        Task<Response<CheckinResultDto>> CheckinAsync(long userId);
        Task<Response<List<UserTaskDto>>> GetTodayTasksAsync(long userId);
        Task<Response<TaskCompleteResultDto>> CompleteTaskAsync(long userId, long taskId);
    }
}
