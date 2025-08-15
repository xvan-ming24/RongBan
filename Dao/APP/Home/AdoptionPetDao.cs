using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.Home
{
    public class AdoptionPetDao
    {

        private readonly PetPlatformDbContext _dbContext;

        public AdoptionPetDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<AdoptionInfo>> GetAdoptionListAsync()
        {
            // 查询可领养的信息，并包含宠物详情
            return await _dbContext.AdoptionInfos
                .Where(a => a.Status == 1) // 只查询可领养状态
                .Include(a => a.Pet)
                .Include(a => a.Pet.PetMedia)
                .ToListAsync();
        }

        public async Task<AdoptionInfo> GetAdoptionByIdAsync(long id)
        {
            return await _dbContext.AdoptionInfos
                .Include(a => a.Pet)
                .Include(a => a.Pet.PetMedia)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
