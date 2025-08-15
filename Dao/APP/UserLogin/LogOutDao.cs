using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RongbanDao
{
    public class LogOutDao
    {
        private PetPlatformDbContext _dbContext;
        public LogOutDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int>  LogOut(long userId)
        {
            var user = await _dbContext.UserPresenceRecords
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.PresenceId = 0;
                _dbContext.Update(user);
               
            }
            return _dbContext.SaveChanges();
        }
    }
}
