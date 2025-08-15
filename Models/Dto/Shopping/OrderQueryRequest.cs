/// <summary>
/// 订单查询请求参数
/// </summary>
public class OrderQueryRequest
{
    public string OrderNo { get; set; } // 订单编号（必填）
    public long UserId { get; set; } // 用户ID（用于权限校验，避免越权查询）
}