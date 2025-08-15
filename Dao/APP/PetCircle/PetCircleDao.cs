using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;

namespace Dao.APP.PetCircel
{
    public class PetCircleDao
    {

        private readonly PetPlatformDbContext _dbContext;

        public PetCircleDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 获取动态列表
        /// </summary>
        public async Task<List<PetMoment>> GetMomentListAsync(int pageIndex, int pageSize)
        {
            return await _dbContext.PetMoments
                .Include(m => m.User)
                .OrderByDescending(m => m.CreateTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        /// <summary>
        /// 获取动态详情
        /// </summary>
        public async Task<PetMoment> GetMomentByIdAsync(long momentId)
        {
            return await _dbContext.PetMoments
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == momentId);
        }

        /// <summary>
        /// 获取动态的评论
        /// </summary>
        public async Task<List<MomentComment>> GetCommentsByMomentIdAsync(long momentId)
        {
            return await _dbContext.MomentComments
                .Include(c => c.User)
                .Where(c => c.MomentId == momentId)
                .OrderBy(c => c.CreateTime)
                .ToListAsync();
        }
        /// <summary>
        /// 创建新动态
        /// </summary>
        /// <param name="petMoment">动态实体</param>
        /// <returns>创建成功的动态实体（包含自动生成的ID）</returns>
        public async Task<PetMoment> CreateAsync(PetMoment petMoment)
        {
            _dbContext.PetMoments.Add(petMoment);
            await _dbContext.SaveChangesAsync();
            return petMoment;
        }

    }
}
