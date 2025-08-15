namespace ShoppingCartApi.Models.Dtos
{
    /// <summary>
    /// 购物车项数据传输对象
    /// 用于API接口与客户端之间的数据交互
    /// </summary>
    public class CartItemDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 是否选中：1-选中，0-未选中
        /// </summary>
        public byte IsSelected { get; set; } = 1; // 默认选中
    }
}
