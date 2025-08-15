/// <summary>
/// 订单商品项视图模型
/// </summary>
public class OrderItemViewModel
{
    public string ProductName { get; set; } // 商品名称
    public decimal ProductPrice { get; set; } // 购买时单价
    public int Quantity { get; set; } // 数量
    public decimal TotalPrice { get; set; } // 小计金额
    public string CoverUrl { get; set; } // 商品封面图（可选，提升前端展示体验）
}