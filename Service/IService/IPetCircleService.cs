using Models.Dto.PetCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Dto.PetCircle.PublishUpdatesDto;

namespace Service.IService
{
    public interface IPetCircleService
    {
        Task<Response<List<MomentListDto>>> GetMomentListAsync(int pageIndex, int pageSize);
        Task<Response<MomentDetailDto>> GetMomentDetailAsync(long momentId);
        /// <summary>
        /// 创建新动态
        /// </summary>
        /// <param name="userId">发布用户ID</param>
        /// <param name="dto">创建动态的参数</param>
        /// <returns>创建成功的动态DTO</returns>
        Task<Response<PetMomentDto>> CreateMomentAsync(long userId, CreatePetMomentDto dto);
    }
}
