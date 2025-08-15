using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Rongban.Models.Entities;
using System;

namespace RongbanDao.APP
{
    /// <summary>
    /// 购物车数据访问实现类
    /// 实现购物车主表的数据操作
    /// </summary>
    public class ShoppingCartRepository 
    {
        // 数据库上下文对象，用于数据库操作
        private readonly PetPlatformDbContext _dbContext;

        /// <summary>
        /// 构造函数，通过依赖注入获取数据库上下文
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public ShoppingCartRepository(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据用户ID获取购物车，包含其所有购物车项
        /// </summary>
        public async Task<ShoppingCart> GetCartByUserIdAsync(long userId)
        {
            return await _dbContext.ShoppingCarts
                .Include(c => c.CartItems) // 关联查询购物车项
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// 创建新购物车
        /// </summary>
        public async Task<ShoppingCart> CreateCartAsync(ShoppingCart cart)
        {
            // 设置创建和更新时间
            cart.CreateTime = DateTime.Now;
            cart.UpdateTime = DateTime.Now;

            // 添加到数据库上下文
            _dbContext.ShoppingCarts.Add(cart);
            // 保存更改
            await _dbContext.SaveChangesAsync();

            return cart;
        }

        /// <summary>
        /// 更新购物车信息
        /// </summary>
        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            // 更新时间戳
            cart.UpdateTime = DateTime.Now;

            // 标记为已修改
            _dbContext.ShoppingCarts.Update(cart);
            // 保存更改
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        public async Task DeleteCartAsync(long cartId)
        {
            // 根据ID查找购物车
            var cart = await _dbContext.ShoppingCarts.FindAsync(cartId);
            if (cart != null)
            {
                // 从数据库上下文中移除
                _dbContext.ShoppingCarts.Remove(cart);
                // 保存更改
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
