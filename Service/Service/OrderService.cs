using Dao.APP.PetStore;
using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using Rongban.Models.ViewModels;
using RongbanDao.APP;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rongban.BLL.Services
{
    /// <summary>
    /// 订单服务实现类
    /// 处理订单创建、商品购买等核心业务逻辑，支持直接购买和购物车批量购买两种场景
    /// 依赖数据访问层组件完成数据库操作，并通过事务保证数据一致性
    /// </summary>
    public class OrderService : IOrderService
    {
        // 订单数据访问组件，处理订单主表CRUD
        private readonly OrderRepository _orderRepository;
        // 商品数据访问组件，提供商品信息查询
        private readonly PetStoreDao _petStoreDao;
        // 购物车项数据访问组件，处理购物车商品项操作
        private readonly CartItemRepository _cartItemRepository;
        // 购物车主表数据访问组件，处理购物车整体信息
        private readonly ShoppingCartRepository _shoppingCartRepository;
        // 数据库上下文，用于事务管理和实体更新
        private readonly PetPlatformDbContext _dbContext;

        /// <summary>
        /// 构造函数，通过依赖注入初始化所需组件
        /// </summary>
        /// <param name="orderRepository">订单数据访问实例</param>
        /// <param name="petStoreDao">商品数据访问实例</param>
        /// <param name="cartItemRepository">购物车项数据访问实例</param>
        /// <param name="shoppingCartRepository">购物车主表数据访问实例</param>
        /// <param name="dbContext">数据库上下文实例</param>
        public OrderService(
            OrderRepository orderRepository,
            PetStoreDao petStoreDao,
            CartItemRepository cartItemRepository,
            ShoppingCartRepository shoppingCartRepository,
            PetPlatformDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _petStoreDao = petStoreDao;
            _cartItemRepository = cartItemRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 直接购买商品（单品购买场景）
        /// 用于用户在商品详情页点击"立即购买"时创建订单，支持单个商品的快速购买
        /// </summary>
        /// <param name="userId">当前购买用户的ID（从登录信息中获取）</param>
        /// <param name="request">购买请求参数，包含商品ID、购买数量及收货信息</param>
        /// <returns>订单处理结果：成功时返回订单编号、支付链接等；失败时返回错误信息</returns>
        public async Task<OrderResult> BuyNowAsync(long userId, BuyNowRequest request)
        {
            // 1. 参数验证：确保购买数量合法
            if (request.Quantity <= 0)
                return new OrderResult { Success = false, Message = "购买数量必须大于0" };

            // 验证收货信息完整性
            if (request.Receiver == null || string.IsNullOrEmpty(request.Receiver.Name) ||
                string.IsNullOrEmpty(request.Receiver.Phone) || string.IsNullOrEmpty(request.Receiver.Address))
                return new OrderResult { Success = false, Message = "请完善收货信息（姓名、电话、地址均为必填）" };

            // 2. 查询商品信息并验证状态
            var products = await _petStoreDao.GetMallProducts(); // 获取所有商品列表
            // 筛选出"已上架"且ID匹配的商品（Status=1表示上架）
            var product = products.FirstOrDefault(p => p.Id == request.ProductId && p.Status == 1);

            // 验证商品是否存在或已下架
            if (product == null)
                return new OrderResult { Success = false, Message = "商品不存在或已下架" };

            // 验证库存是否充足
            if (product.Stock < request.Quantity)
                return new OrderResult { Success = false, Message = "商品库存不足，无法完成购买" };

            // 3. 生成唯一订单编号
            var orderNo = GenerateOrderNo(userId);

            // 4. 开启数据库事务：确保订单创建、库存扣减等操作的原子性
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 5. 创建订单主记录
                    var orderMain = new OrderMain
                    {
                        OrderNo = orderNo,         // 订单编号
                        UserId = userId,           // 关联用户
                        TotalAmount = product.Price * request.Quantity, // 计算总金额（单价×数量）
                        PayStatus = 0,             // 支付状态：0-未支付
                        Status = 0,                // 订单状态：0-待支付
                        ReceiverName = request.Receiver.Name,    // 收货人姓名
                        ReceiverPhone = request.Receiver.Phone,  // 收货人电话
                        ReceiverAddress = request.Receiver.Address // 收货地址
                    };

                    // 保存订单主表并获取创建后的订单ID
                    var createdOrder = await _orderRepository.CreateOrderAsync(orderMain);

                    // 6. 创建订单商品项记录（关联订单与商品）
                    var orderItem = new OrderItem
                    {
                        OrderId = createdOrder.Id,     // 关联订单主表
                        ProductId = product.Id,        // 关联商品
                        ProductName = product.ProductName, // 商品名称（冗余存储，避免后续商品名称变更影响订单）
                        ProductPrice = product.Price,  // 购买时的单价（冗余存储）
                        Quantity = request.Quantity,   // 购买数量
                        TotalPrice = product.Price * request.Quantity // 商品小计
                    };

                    // 批量添加订单商品项（此处为单个商品，仍用批量方法保持扩展性）
                    await _orderRepository.BatchAddOrderItemsAsync(new List<OrderItem> { orderItem });

                    // 7. 扣减商品库存
                    product.Stock -= request.Quantity; // 减少库存
                    _dbContext.MallProducts.Update(product); // 标记为已修改
                    await _dbContext.SaveChangesAsync(); // 提交库存更新

                    // 8. 提交事务：所有操作成功后确认提交
                    await transaction.CommitAsync();

                    // 9. 生成支付链接（实际项目需对接微信支付、支付宝等网关）
                    var payUrl = GeneratePaymentUrl(orderNo, createdOrder.TotalAmount);

                    // 返回成功结果
                    return new OrderResult
                    {
                        Success = true,
                        Message = "订单创建成功，请尽快支付",
                        OrderNo = orderNo,
                        TotalAmount = createdOrder.TotalAmount,
                        PayUrl = payUrl
                    };
                }
                catch (Exception ex)
                {
                    // 发生异常时回滚事务，确保数据一致性
                    await transaction.RollbackAsync();
                    return new OrderResult { Success = false, Message = $"创建订单失败: {ex.Message}" };
                }
            }
        }

        /// <summary>
        /// 从购物车一键购买（多商品批量购买场景）
        /// 用于用户在购物车中选择多个商品后，一次性完成购买流程
        /// </summary>
        /// <param name="userId">当前购买用户的ID（从登录信息中获取）</param>
        /// <param name="receiverInfo">收货信息，包含收货人姓名、电话、地址</param>
        /// <returns>订单处理结果：成功时返回订单编号、支付链接等；失败时返回错误信息</returns>
        public async Task<OrderResult> BuyFromCartAsync(long userId, ReceiverInfo receiverInfo)
        {
            // 1. 验证收货信息完整性
            if (receiverInfo == null || string.IsNullOrEmpty(receiverInfo.Name) ||
                string.IsNullOrEmpty(receiverInfo.Phone) || string.IsNullOrEmpty(receiverInfo.Address))
                return new OrderResult { Success = false, Message = "请完善收货信息（姓名、电话、地址均为必填）" };

            // 2. 获取用户购物车及选中的商品
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return new OrderResult { Success = false, Message = "购物车不存在，请先添加商品" };

            // 获取该购物车下的所有商品项
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
            // 筛选出用户选中的商品（isSelected=1表示选中）
            var selectedItems = cartItems.Where(ci => ci.IsSelected == 1).ToList();

            // 验证是否有选中的商品
            if (!selectedItems.Any())
                return new OrderResult { Success = false, Message = "请选择要购买的商品" };

            // 3. 获取所有商品信息，用于验证状态和库存
            var products = await _petStoreDao.GetMallProducts();

            // 4. 批量验证商品状态和库存
            foreach (var item in selectedItems)
            {
                // 查找对应的上架商品（Status=1）
                var product = products.FirstOrDefault(p => p.Id == item.ProductId && p.Status == 1);
                if (product == null)
                    return new OrderResult { Success = false, Message = $"商品《{item.ProductName}》不存在或已下架" };

                // 验证库存是否充足
                if (product.Stock < item.Quantity)
                    return new OrderResult { Success = false, Message = $"商品《{item.ProductName}》库存不足，当前库存: {product.Stock}" };
            }

            // 5. 生成唯一订单编号
            var orderNo = GenerateOrderNo(userId);

            // 6. 开启数据库事务：确保批量操作的原子性
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 7. 计算订单总金额（所有选中商品的小计之和）
                    decimal totalAmount = selectedItems.Sum(item => item.ProductPrice * item.Quantity);

                    // 8. 创建订单主记录
                    var orderMain = new OrderMain
                    {
                        OrderNo = orderNo,
                        UserId = userId,
                        TotalAmount = totalAmount,
                        PayStatus = 0, // 未支付
                        Status = 0,    // 待支付
                        ReceiverName = receiverInfo.Name,
                        ReceiverPhone = receiverInfo.Phone,
                        ReceiverAddress = receiverInfo.Address
                    };

                    var createdOrder = await _orderRepository.CreateOrderAsync(orderMain);

                    // 9. 批量创建订单商品项
                    var orderItems = selectedItems.Select(item => new OrderItem
                    {
                        OrderId = createdOrder.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName, // 冗余存储购物车中的商品名称
                        ProductPrice = item.ProductPrice, // 冗余存储加入购物车时的单价
                        Quantity = item.Quantity,
                        TotalPrice = item.ProductPrice * item.Quantity
                    }).ToList();

                    await _orderRepository.BatchAddOrderItemsAsync(orderItems);

                    // 10. 批量扣减商品库存
                    foreach (var item in selectedItems)
                    {
                        var product = products.First(p => p.Id == item.ProductId);
                        product.Stock -= item.Quantity; // 减少对应数量的库存
                        _dbContext.MallProducts.Update(product);
                    }
                    await _dbContext.SaveChangesAsync(); // 提交库存更新

                    // 11. 删除购物车中已购买的商品项
                    await _cartItemRepository.DeleteCartItemsByCartIdAsync(cart.Id);

                    // 12. 更新购物车信息（清空总数量和总金额）
                    cart.TotalCount = 0;
                    cart.TotalPrice = 0;
                    await _shoppingCartRepository.UpdateCartAsync(cart);

                    // 13. 提交事务
                    await transaction.CommitAsync();

                    // 14. 生成支付链接
                    var payUrl = GeneratePaymentUrl(orderNo, totalAmount);

                    // 返回成功结果
                    return new OrderResult
                    {
                        Success = true,
                        Message = "订单创建成功，请尽快支付",
                        OrderNo = orderNo,
                        TotalAmount = totalAmount,
                        PayUrl = payUrl
                    };
                }
                catch (Exception ex)
                {
                    // 异常时回滚事务
                    await transaction.RollbackAsync();
                    return new OrderResult { Success = false, Message = $"创建订单失败: {ex.Message}" };
                }
            }
        }

        /// <summary>
        /// 生成唯一订单编号
        /// 采用"前缀+时间戳+用户ID后4位+随机数"的规则，确保唯一性和可读性
        /// </summary>
        /// <param name="userId">用户ID，用于增强编号唯一性</param>
        /// <returns>格式化的订单编号字符串</returns>
        public string GenerateOrderNo(long userId)
        {
            // 格式示例：ORD2025080616304512345678（ORD+年月日时分秒+用户ID后4位+4位随机数）
            return $"ORD{DateTime.Now:yyyyMMddHHmmss}{userId.ToString().PadRight(4, '0').Substring(0, 4)}{new Random().Next(1000, 9999)}";
        }

        /// <summary>
        /// 生成支付链接（示例方法）
        /// 实际项目中需对接第三方支付网关（如微信支付、支付宝），调用其统一下单接口生成真实支付链接
        /// </summary>
        /// <param name="orderNo">订单编号，用于关联支付订单</param>
        /// <param name="amount">支付金额（单位：元）</param>
        /// <returns>支付页面的URL地址（示例）</returns>
        public string GeneratePaymentUrl(string orderNo, decimal amount)
        {
            // 示例链接：实际应替换为支付网关返回的链接，包含订单号、金额等参数
            return $"/payment?orderNo={orderNo}&amount={amount}";
        }

        /// <summary>
        /// 根据订单号查询订单详情（带权限校验）
        /// </summary>
        public async Task<OrderDetailResult> GetOrderByNoAsync(OrderQueryRequest request)
        {
            // 1. 验证参数
            if (string.IsNullOrEmpty(request.OrderNo))
                throw new ArgumentException("订单编号不能为空");

            // 2. 调用DAL查询订单及商品
            var (order, items) = await _orderRepository.GetOrderWithItemsAsync(request.OrderNo, request.UserId);
            if (order == null)
                return null; // 订单不存在或无权限

            // 3. 组装响应结果（包含状态文字描述）
            return new OrderDetailResult
            {
                OrderNo = order.OrderNo,
                TotalAmount = order.TotalAmount,
                PayStatus = (int)order.PayStatus,
                Status = (int)order.Status,
                StatusText = order.Status switch
                {
                    0 => "待支付",
                    1 => "已发货",
                    2 => "已完成",
                    3 => "已取消",
                    _ => "未知状态"
                },
                ReceiverName = order.ReceiverName,
                ReceiverPhone = order.ReceiverPhone,
                ReceiverAddress = order.ReceiverAddress,
                CreateTime = (DateTime)order.CreateTime,
                PayTime = order.PayTime,
                Items = items.Select(item => new OrderItemViewModel
                {
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                    // 如需商品封面图，可关联查询mall_product表的cover_url
                }).ToList()
            };
        }

        /// <summary>
        /// 用户确认收货，标记订单为已完成
        /// </summary>
        public async Task<OrderResult> ConfirmReceiptAsync(string orderNo, long userId)
        {
            // 1. 调用DAL更新状态
            var success = await _orderRepository.UpdateOrderToCompletedAsync(orderNo, userId);
            if (!success)
                return new OrderResult { Success = false, Message = "确认收货失败，订单状态异常或不存在" };

            return new OrderResult { Success = true, Message = "已确认收货，订单已完成" };
        }
    }
}