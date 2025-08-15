using System.Collections.Generic;

namespace ShoppingCartApi.Models.Dtos
{
    /// <summary>
    /// 购物车数据传输对象
    /// 用于API接口返回完整的购物车信息
    /// </summary>
    public class ShoppingCartDto
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 购物车商品总金额（仅包含选中的商品）
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 购物车商品总数量（仅包含选中的商品）
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 购物车中的商品项列表
        /// </summary>
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
}
