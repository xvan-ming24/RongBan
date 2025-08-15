using Microsoft.EntityFrameworkCore;
using Models.Dto.PetStore;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.PetStore
{
    public class PetStoreDao
    {
        private readonly PetPlatformDbContext _dbContext;

        public PetStoreDao(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ProductCategory>> GetProduct()
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.ProductCategories.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取商品分类数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取商品媒体资源（图片视频等）
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ProductMedium>> GetProductMedia()
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.ProductMedia.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取宠物数据失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 获取商城商品
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<MallProduct>> GetMallProducts()
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.MallProducts.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取商品数据失败: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// 根据分类ID获取商城商品
        /// </summary>
        /// <param name="categoryId">要获取的分类的id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<MallProduct>> GetMallProductsByCategoryId(long categoryId)
        {
            try
            {
                await Task.Delay(100);
                var data = await _dbContext.MallProducts
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取商品数据失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 根据商品ID获取单个商品信息
        /// </summary>
        /// <param name="productId">要获取的商品的id</param>
        /// <returns>商品实体（MallProduct），若不存在则返回null</returns>
        /// <exception cref="Exception"></exception>
        public async Task<MallProduct> GetMallProductById(long productId)
        {
            try
            {
                await Task.Delay(100); // 保持与原有方法一致的延迟（模拟网络/数据库耗时）
                var data = await _dbContext.MallProducts
                    .FirstOrDefaultAsync(p => p.Id == productId); // 查询单个商品
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"根据ID获取商品数据失败: {ex.Message}", ex); // 异常信息与原有风格一致
            }
        }

        #region 秒杀相关新增方法

        /// <summary>
        /// 获取可参与秒杀的商品（状态为“秒杀中”、时间范围内、库存>0）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>秒杀商品信息（含秒杀库存、价格等）</returns>
        public async Task<MallProduct> GetSeckillProductAsync(long productId)
        {
            try
            {
                await Task.Delay(100); // 保持延迟风格
                var now = DateTime.Now;
                // 筛选条件：秒杀中 + 时间在范围内 + 库存充足
                return await _dbContext.MallProducts
                    .Where(p => p.Id == productId
                               && p.SeckillStatus == 1 // 1-秒杀中
                               && p.SeckillStartTime <= now
                               && p.SeckillEndTime >= now
                               && p.SeckillStock > 0)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"获取秒杀商品信息失败: {ex.Message}", ex);
            }
        }

        /// <summary>   
        /// 乐观锁扣减秒杀库存
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="currentVersion">当前版本号（用于乐观锁校验）</param>
        /// <returns>是否扣减成功（true=成功）</returns>
        public async Task<bool> ReduceSeckillStockAsync(long productId, int currentVersion)
        {
            try
            {
                await Task.Delay(100);
                // 执行SQL更新（EF Core直接写SQL更高效，避免先查后更的并发问题）
                // 修正：使用C#插值表达式，不手动拼接字符串
                // 使用 $@ 多行插值语法，确保识别为 FormattableString
                var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                                        UPDATE mall_product 
                                        SET seckill_stock = seckill_stock - 1, 
                                        seckill_version = seckill_version + 1 
                                        WHERE id = {productId} AND seckill_version = {currentVersion}");

                return rowsAffected > 0; // 影响行数>0表示扣减成功
            }
            catch (Exception ex)
            {
                throw new Exception($"扣减秒杀库存失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 查询用户对某商品的秒杀订单数量（用于限购校验）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>有效秒杀订单数量（排除已取消的）</returns>
        public async Task<int> CountUserSeckillOrdersAsync(long userId, long productId)
        {
            try
            {
                await Task.Delay(100);
                // 关联订单主表和订单商品表，统计用户已秒杀的有效订单数
                return await _dbContext.OrderMains
                    .Where(om => om.UserId == userId
                                 && om.IsSeckill == true // 秒杀订单
                                 && om.Status != 3) // 排除已取消状态（3-已取消）
                    .Join(
                        _dbContext.OrderItems,
                        om => om.Id,
                        oi => oi.OrderId,
                        (om, oi) => new { oi.ProductId }
                    )
                    .Where(join => join.ProductId == productId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"查询用户秒杀订单数量失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 恢复秒杀库存（用于订单取消等场景）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="currentVersion">当前秒杀版本号（用于乐观锁校验）</param>
        /// <returns>是否恢复成功</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RestoreSeckillStockAsync(long productId, int currentVersion)
        {
            try
            {
                await Task.Delay(100); // 保持与其他方法一致的延迟风格
                                       // 乐观锁更新：仅当版本号匹配时恢复库存，同时版本号+1
                var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE mall_product 
            SET seckill_stock = seckill_stock + 1, 
                seckill_version = seckill_version + 1 
            WHERE id = {productId} AND seckill_version = {currentVersion}
        ");
                return rowsAffected > 0; // 影响行数>0表示恢复成功
            }
            catch (Exception ex)
            {
                throw new Exception($"恢复秒杀库存失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 查询所有秒杀中且库存充足的商品
        /// </summary>
        /// <returns>秒杀商品列表</returns>
        public async Task<List<MallProduct>> GetSeckillProductsAsync()
        {
            try
            {
                var now = DateTime.Now; // 当前系统时间

                // 筛选条件：
                // 1. 秒杀状态为1（秒杀中）
                // 2. 秒杀库存>0
                // 3. 当前时间在秒杀开始和结束时间之间
                return await _dbContext.MallProducts
                    .Where(p =>
                        p.SeckillStatus == 1
                        && p.SeckillStock > 0
                        && now >= p.SeckillStartTime
                        && now <= p.SeckillEndTime
                    )
                    .OrderByDescending(p => p.CreateTime) // 按创建时间倒序，最新的在前
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"查询秒杀商品列表失败: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
