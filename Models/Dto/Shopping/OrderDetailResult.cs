/// <summary>
/// 订单详情响应模型（包含商品列表）
/// </summary>
public class OrderDetailResult
{
    public string OrderNo { get; set; } // 订单编号
    public decimal TotalAmount { get; set; } // 总金额
    public int PayStatus { get; set; } // 支付状态（0-未支付，1-已支付，2-退款）
    public int Status { get; set; } // 订单状态（0-待支付，1-已发货，2-已完成，3-已取消）
    public string StatusText { get; set; } // 状态文字描述（如“已完成”）
    public string ReceiverName { get; set; } // 收货人
    public string ReceiverPhone { get; set; } // 收货电话
    public string ReceiverAddress { get; set; } // 收货地址
    public DateTime CreateTime { get; set; } // 下单时间
    public DateTime? PayTime { get; set; } // 支付时间
    public List<OrderItemViewModel> Items { get; set; } // 订单商品列表
}