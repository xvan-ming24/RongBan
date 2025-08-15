
using ShoppingCartApi.Models.Dtos;
using System.Threading.Tasks;

namespace ShoppingCartApi.BLL
{
    /// <summary>
    /// 购物车业务逻辑接口
    /// 定义购物车相关的业务操作方法
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// 获取用户购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>购物车DTO对象 </returns>
        Task<ShoppingCartDto> GetUserShoppingCartAsync(long userId);

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="itemDto">购物车项DTO</param>
        /// <returns>更新后的购物车DTO</returns>
        Task<ShoppingCartDto> AddItemToCartAsync(long userId, CartItemDto itemDto);

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">新数量</param>
        /// <returns>更新后的购物车DTO</returns>
        Task<ShoppingCartDto> UpdateCartItemQuantityAsync(long userId, long productId, int quantity);

        /// <summary>
        /// 更新购物车商品选中状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="isSelected">选中状态：1-选中，0-未选中</param>
        /// <returns>更新后的购物车DTO</returns>
        Task<ShoppingCartDto> UpdateCartItemSelectionAsync(long userId, long productId, byte isSelected);

        /// <summary>
        /// 从购物车移除商品
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>更新后的购物车DTO</returns>
        Task<ShoppingCartDto> RemoveItemFromCartAsync(long userId, long productId);

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>任务对象</returns>
        Task ClearShoppingCartAsync(long userId);
    }
}
