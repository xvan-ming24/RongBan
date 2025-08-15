using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using System;
using System.Threading.Tasks;

namespace RongbanDao.APP
{
    /// <summary>
    /// 订单主表数据访问实现类
    /// </summary>
    public class OrderRepository
    {
        private readonly PetPlatformDbContext _dbContext;

        public OrderRepository(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        public async Task<OrderMain> CreateOrderAsync(OrderMain order)
        {
            order.CreateTime = DateTime.Now;
            order.UpdateTime = DateTime.Now;

            _dbContext.OrderMains.Add(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        /// <summary>
        /// 根据订单号获取订单
        /// </summary>
        public async Task<OrderMain> GetOrderByNoAsync(string orderNo)
        {
            return await _dbContext.OrderMains
                .FirstOrDefaultAsync(o => o.OrderNo == orderNo);
        }

        /// <summary>
        /// 更新订单支付状态
        /// </summary>
        public async Task UpdatePaymentStatusAsync(long orderId, int payStatus, DateTime payTime)
        {
            var order = await _dbContext.OrderMains.FindAsync(orderId);
            if (order != null)
            {
                order.PayStatus = Convert.ToByte(payStatus);
                order.PayTime = payTime;
                order.Status = payStatus == 1 ? 1 : order.Status; // 已支付则更新订单状态为已发货
                order.UpdateTime = DateTime.Now;

                _dbContext.OrderMains.Update(order);
                await _dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 批量添加订单商品项
        /// </summary>
        public async Task BatchAddOrderItemsAsync(List<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                item.CreateTime = DateTime.Now;
            }

            _dbContext.OrderItems.AddRange(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据订单ID获取订单商品项
        /// </summary>
        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(long orderId)
        {
            return await _dbContext.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
        /// <summary>
        /// 根据订单编号和用户ID查询订单（带商品列表）
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="userId">用户ID（用于权限校验）</param>
        /// <returns>订单主信息+商品列表</returns>
        public async Task<(OrderMain order, List<OrderItem> items)> GetOrderWithItemsAsync(string orderNo, long userId)
        {
            // 先查询订单主信息（验证归属权）
            var order = await _dbContext.OrderMains
                .FirstOrDefaultAsync(o => o.OrderNo == orderNo && o.UserId == userId);
            if (order == null)
                return (null, null);

            // 再查询关联的商品项
            var items = await _dbContext.OrderItems
                .Where(i => i.OrderId == order.Id)
                .ToListAsync();

            return (order, items);
        }

        /// <summary>
        /// 更新订单状态为“已完成”
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="userId">用户ID（用于权限校验）</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateOrderToCompletedAsync(string orderNo, long userId)
        {
            var order = await _dbContext.OrderMains
                .FirstOrDefaultAsync(o => o.OrderNo == orderNo && o.UserId == userId);

            if (order == null)
                return false; // 订单不存在或不属于该用户

            // 只有“已发货”状态（status=1）可更新为“已完成”（status=2）
            if (order.Status != 1)
                return false;

            order.Status = 2; // 标记为已完成
            order.UpdateTime = DateTime.Now;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
