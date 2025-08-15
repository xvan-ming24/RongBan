using Microsoft.EntityFrameworkCore;
using Models.Dto.Personal;
using Rongban.Models.Entities;
using System.Drawing;


namespace Dao.APP.Personal
{
    public class PersonalPetInfoDao
    {
        private readonly PetPlatformDbContext _dbContext;

        public PersonalPetInfoDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 根据id获取用户宠物信息（详细）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="petId">宠物id</param>
        /// <returns></returns>
        public async Task<PetInfo> GetPetByUserIdAndPetIdAsync(long userId, long petId)
        {
            return await _dbContext.UserPetRelations
                .Where(upr => upr.UserId == userId && upr.PetId == petId)
                .Include(upr => upr.Pet)         
                .ThenInclude(pet => pet.Category) 
                .Select(upr => upr.Pet)
                .Where(pet => pet != null && pet.Category != null)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 获取用户全部宠物(详细)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PetInfo>> GetPetsByUserIdAsync(long userId)
        {
            return await _dbContext.UserPetRelations
                .Where(upr => upr.UserId == userId)
                .Include(upr => upr.Pet)
                .ThenInclude(pet => pet.Category)
                .Select(upr => upr.Pet)
                .Where(pet => pet != null && pet.Category != null)
                .ToListAsync();
        }

    }
}
