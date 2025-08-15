using Models.Dto.Personal;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IPersonalPetInfoService
    {
        /// <summary>
        /// 根据用户ID和宠物ID获取宠物信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="petId">宠物ID</param>
        /// <returns>宠物信息DTO</returns>
        Task<Response<PetInfoDto>> GetPetByUserIdAndPetIdAsync(long userId, long petId);

        /// <summary>
        /// 根据用户ID获取所有关联的宠物信息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>宠物信息DTO列表</returns>
        Task<Response<List<PetInfoDto>>> GetPetsByUserIdAsync(long userId);

    }
}
