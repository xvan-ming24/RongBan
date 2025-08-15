using System;
using System.Collections.Generic;

namespace Rongban.Models.ViewModels
{
    /// <summary>
    /// 订单创建结果
    /// </summary>
    public class OrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string PayUrl { get; set; }
    }

    /// <summary>
    /// 收货信息
    /// </summary>
    public class ReceiverInfo
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    /// <summary>
    /// 直接购买请求参数
    /// </summary>
    public class BuyNowRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public ReceiverInfo Receiver { get; set; }
    }
}
