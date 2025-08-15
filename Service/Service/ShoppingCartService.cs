using System;
using System.Linq;
using System.Threading.Tasks;
using Rongban.Models.Entities;
using RongbanDao.APP;
using ShoppingCartApi.Models.Dtos;
namespace ShoppingCartApi.BLL
{
    /// <summary>
    /// 购物车业务逻辑实现类
    /// 处理购物车相关的核心业务逻辑
    /// </summary>
    public class ShoppingCartService : IShoppingCartService
    {
        // 购物车数据访问对象
        private readonly ShoppingCartRepository _cartRepository;
        // 购物车项数据访问对象
        private readonly CartItemRepository _cartItemRepository;

        /// <summary>
        /// 构造函数，通过依赖注入获取数据访问层对象
        /// </summary>
        public ShoppingCartService(ShoppingCartRepository cartRepository,
                                  CartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// 获取用户购物车
        /// 如果用户没有购物车，自动创建一个新的
        /// </summary>
        public async Task<ShoppingCartDto> GetUserShoppingCartAsync(long userId)
        {
            // 根据用户ID获取购物车
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // 如果用户没有购物车，创建一个新的
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    TotalPrice = 0.00m,
                    TotalCount = 0
                };
                cart = await _cartRepository.CreateCartAsync(cart);
            }

            // 转换为DTO并返回
            return MapToDto(cart);
        }

        /// <summary>
        /// 添加商品到购物车
        /// 如果商品已存在，则增加数量；否则添加新商品
        /// </summary>
        public async Task<ShoppingCartDto> AddItemToCartAsync(long userId, CartItemDto itemDto)
        {
            // 验证输入：数量必须大于0
            if (itemDto.Quantity <= 0)
                throw new ArgumentException("商品数量必须大于0");

            // 获取或创建用户购物车
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                      ?? await _cartRepository.CreateCartAsync(new ShoppingCart { UserId = userId });

            // 检查商品是否已在购物车中
            var existingItem = await _cartItemRepository
                .GetCartItemByCartIdAndProductIdAsync(cart.Id, itemDto.ProductId);

            if (existingItem != null)
            {
                // 商品已存在，更新数量
                existingItem.Quantity += itemDto.Quantity;
                existingItem.UpdateTime = DateTime.Now;
                await _cartItemRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                // 商品不存在，添加新项
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = itemDto.ProductId,
                    ProductName = itemDto.ProductName,
                    ProductPrice = itemDto.ProductPrice,
                    Quantity = itemDto.Quantity,
                    IsSelected = itemDto.IsSelected
                };
                await _cartItemRepository.AddCartItemAsync(newItem);
            }

            // 更新购物车统计信息（总金额和总数量）
            await UpdateCartStatistics(cart.Id);

            // 返回更新后的购物车
            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId);
            return MapToDto(updatedCart);
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        public async Task<ShoppingCartDto> UpdateCartItemQuantityAsync(long userId, long productId, int quantity)
        {
            // 验证输入：数量必须大于0
            if (quantity <= 0)
                throw new ArgumentException("商品数量必须大于0");

            // 获取用户购物车，如果不存在则抛出异常
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                      ?? throw new KeyNotFoundException("购物车不存在");

            // 查找购物车中的商品项，如果不存在则抛出异常
            var item = await _cartItemRepository
                .GetCartItemByCartIdAndProductIdAsync(cart.Id, productId)
                      ?? throw new KeyNotFoundException("购物车中不存在该商品");

            // 更新商品数量
            item.Quantity = quantity;
            await _cartItemRepository.UpdateCartItemAsync(item);

            // 更新购物车统计信息
            await UpdateCartStatistics(cart.Id);

            // 返回更新后的购物车
            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId);
            return MapToDto(updatedCart);
        }

        /// <summary>
        /// 更新购物车商品选中状态
        /// </summary>
        public async Task<ShoppingCartDto> UpdateCartItemSelectionAsync(long userId, long productId, byte isSelected)
        {
            // 验证输入：选中状态只能是0或1
            if (isSelected != 0 && isSelected != 1)
                throw new ArgumentException("选中状态只能是0或1");

            // 获取用户购物车，如果不存在则抛出异常
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                      ?? throw new KeyNotFoundException("购物车不存在");

            // 查找购物车中的商品项，如果不存在则抛出异常
            var item = await _cartItemRepository
                .GetCartItemByCartIdAndProductIdAsync(cart.Id, productId)
                      ?? throw new KeyNotFoundException("购物车中不存在该商品");

            // 更新选中状态
            item.IsSelected = isSelected;
            await _cartItemRepository.UpdateCartItemAsync(item);

            // 更新购物车统计信息（只计算选中的商品）
            await UpdateCartStatistics(cart.Id);

            // 返回更新后的购物车
            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId);
            return MapToDto(updatedCart);
        }

        /// <summary>
        /// 从购物车移除商品
        /// </summary>
        public async Task<ShoppingCartDto> RemoveItemFromCartAsync(long userId, long productId)
        {
            // 获取用户购物车，如果不存在则抛出异常
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                      ?? throw new KeyNotFoundException("购物车不存在");

            // 查找购物车中的商品项，如果不存在则抛出异常
            var item = await _cartItemRepository
                .GetCartItemByCartIdAndProductIdAsync(cart.Id, productId)
                      ?? throw new KeyNotFoundException("购物车中不存在该商品");

            // 删除商品项
            await _cartItemRepository.DeleteCartItemAsync(item.Id);

            // 更新购物车统计信息
            await UpdateCartStatistics(cart.Id);

            // 返回更新后的购物车
            var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId);
            return MapToDto(updatedCart);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        public async Task ClearShoppingCartAsync(long userId)
        {
            // 获取用户购物车
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                // 删除该购物车下的所有商品项
                await _cartItemRepository.DeleteCartItemsByCartIdAsync(cart.Id);

                // 重置购物车统计信息
                cart.TotalPrice = 0.00m;
                cart.TotalCount = 0;
                await _cartRepository.UpdateCartAsync(cart);
            }
        }

        /// <summary>
        /// 更新购物车的总金额和总数量（只计算选中的商品）
        /// </summary>
        private async Task UpdateCartStatistics(long cartId)
        {
            // 获取该购物车下的所有商品项
            var items = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);

            // 只计算选中的商品
            var selectedItems = items.Where(i => i.IsSelected == 1).ToList();

            // 计算总金额和总数量
            var totalPrice = selectedItems.Sum(i => i.ProductPrice * i.Quantity);
            var totalCount = selectedItems.Sum(i => i.Quantity);

            // 获取购物车并更新统计信息
            var cart = await _cartRepository.GetCartByUserIdAsync(
                (await _cartRepository.GetCartByUserIdAsync(cartId))?.UserId ?? 0);

            if (cart != null)
            {
                cart.TotalPrice = totalPrice;
                cart.TotalCount = totalCount;
                await _cartRepository.UpdateCartAsync(cart);
            }
        }

        /// <summary>
        /// 将实体对象映射为DTO对象
        /// 用于数据传输和展示
        /// </summary>
        private ShoppingCartDto MapToDto(ShoppingCart cart)
        {
            return new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = (decimal)cart.TotalPrice,
                TotalCount = (int)cart.TotalCount,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity,
                    IsSelected = (byte)item.IsSelected
                }).ToList()
            };
        }
    }
}
