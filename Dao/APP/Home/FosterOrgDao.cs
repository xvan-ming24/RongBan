using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.Home
{
    public class FosterOrgDao
    {
        private readonly PetPlatformDbContext _dbContext;
        public FosterOrgDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取提供寄养服务的门店列表
        /// </summary>
        /// <returns>门店列表</returns>
        public async Task<List<OrgInfo>> GetFosterOrgsAsync()
        {
            // 查询正常营业且提供可用寄养服务的门店
            return await _dbContext.OrgInfos
                .Where(oi => oi.Status == 1) // 正常营业
                .Include(oi => oi.FosterServices)
                .Where(oi => oi.FosterServices.Any(fs => fs.Status == 1)) // 有可用的寄养服务
                .Distinct()
                .ToListAsync();
        }
    }
}
