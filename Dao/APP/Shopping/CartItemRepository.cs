using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rongban.Models.Entities;
using System;

namespace RongbanDao.APP
{
    /// <summary>
    /// 购物车项数据访问实现类
    /// 实现购物车项表的数据操作
    /// </summary>
    public class CartItemRepository 
    {
        // 数据库上下文对象，用于数据库操作
        private readonly PetPlatformDbContext _dbContext;

        /// <summary>
        /// 构造函数，通过依赖注入获取数据库上下文
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public CartItemRepository(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据购物车ID获取所有购物车项
        /// </summary>
        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(long cartId)
        {
            return await _dbContext.CartItems
                .Where(i => i.CartId == cartId)
                .ToListAsync();
        }

        /// <summary>
        /// 根据购物车ID和商品ID获取特定购物车项
        /// </summary>
        public async Task<CartItem> GetCartItemByCartIdAndProductIdAsync(long cartId, long productId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
        }

        /// <summary>
        /// 添加新的购物车项
        /// </summary>
        public async Task<CartItem> AddCartItemAsync(CartItem item)
        {
            // 设置创建和更新时间
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;

            // 添加到数据库上下文
            _dbContext.CartItems.Add(item);
            // 保存更改
            await _dbContext.SaveChangesAsync();

            return item;
        }

        /// <summary>
        /// 更新购物车项信息
        /// </summary>
        public async Task UpdateCartItemAsync(CartItem item)
        {
            // 更新时间戳
            item.UpdateTime = DateTime.Now;

            // 标记为已修改
            _dbContext.CartItems.Update(item);
            // 保存更改
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据ID删除购物车项
        /// </summary>
        public async Task DeleteCartItemAsync(long itemId)
        {
            // 根据ID查找购物车项
            var item = await _dbContext.CartItems.FindAsync(itemId);
            if (item != null)
            {
                // 从数据库上下文中移除
                _dbContext.CartItems.Remove(item);
                // 保存更改
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 根据购物车ID删除所有购物车项
        /// </summary>
        public async Task DeleteCartItemsByCartIdAsync(long cartId)
        {
            // 查询该购物车下的所有购物车项
            var items = await _dbContext.CartItems
                .Where(i => i.CartId == cartId)
                .ToListAsync();

            // 批量移除
            _dbContext.CartItems.RemoveRange(items);
            // 保存更改
            await _dbContext.SaveChangesAsync();
        }
    }
}
