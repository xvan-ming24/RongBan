

using Rongban.Models.ViewModels;

namespace Service.IService
{
    public interface IOrderService
    {
        /// <summary>
        /// 直接购买商品（单品购买）
        /// 用于用户在商品详情页点击"立即购买"时创建订单
        /// </summary>
        /// <param name="userId">用户ID，标识当前购买的用户</param>
        /// <param name="request">购买请求参数，包含商品ID、购买数量及收货信息</param>
        /// <returns>订单处理结果，包含是否成功、订单编号、支付链接等信息</returns>
        /// <remarks>
        /// 业务流程：
        /// 1. 验证商品状态及库存
        /// 2. 创建订单主记录及订单商品记录
        /// 3. 扣减对应商品库存
        /// 4. 生成支付链接
        /// 所有操作在同一事务中执行，确保数据一致性
        /// </remarks>
        Task<OrderResult> BuyNowAsync(long userId, BuyNowRequest request);

        /// <summary>
        /// 从购物车一键购买（多商品批量购买）
        /// 用于用户在购物车页面选择多个商品后一次性购买
        /// </summary>
        /// <param name="userId">用户ID，标识当前购买的用户</param>
        /// <param name="receiverInfo">收货信息，包含收货人姓名、电话、地址</param>
        /// <returns>订单处理结果，包含是否成功、订单编号、支付链接等信息</returns>
        /// <remarks>
        /// 业务流程：
        /// 1. 查询用户选中的购物车商品
        /// 2. 批量验证商品状态及库存
        /// 3. 创建订单主记录及批量订单商品记录
        /// 4. 批量扣减商品库存
        /// 5. 删除购物车中已购买的商品项
        /// 6. 生成支付链接
        /// 所有操作在同一事务中执行，确保数据一致性
        /// </remarks>
        Task<OrderResult> BuyFromCartAsync(long userId, ReceiverInfo receiverInfo);

        /// <summary>
        /// 生成唯一订单编号
        /// 用于标识每一笔订单，避免重复
        /// </summary>
        /// <param name="userId">用户ID，用于增强订单编号的唯一性</param>
        /// <returns>格式化的订单编号字符串</returns>
        /// <remarks>
        /// 订单编号生成规则（可根据业务调整）：
        /// 前缀（ORDER）+ 时间戳（yyyyMMddHHmmss）+ 用户ID后4位 + 随机数（4位）
        /// 示例：ORDER_20250806153022_1234_5678
        /// </remarks>
        string GenerateOrderNo(long userId);

        /// <summary>
        /// 生成支付链接
        /// 用于引导用户跳转至支付页面完成付款
        /// </summary>
        /// <param name="orderNo">订单编号，关联需要支付的订单</param>
        /// <param name="amount">支付金额，单位：元</param>
        /// <returns>支付页面的URL地址</returns>
        /// <remarks>
        /// 实际项目中需对接第三方支付网关（如微信支付、支付宝）：
        /// 1. 调用支付网关的统一下单接口
        /// 2. 将返回的支付参数拼接为支付链接或二维码
        /// 3. 支付完成后需处理支付回调，更新订单状态
        /// </remarks>
        string GeneratePaymentUrl(string orderNo, decimal amount);
        /// <summary>
        /// 根据订单号查询订单详情（包含商品列表）
        /// 支持用户查询自己的订单，包含订单状态、收货信息、商品明细等完整数据
        /// </summary>
        /// <param name="request">查询请求参数，包含订单编号和用户ID</param>
        /// <returns>订单详情结果模型（OrderDetailResult），包含订单基本信息和商品列表；若订单不存在或无权限，返回null</returns>
        /// <remarks>
        /// 业务逻辑：
        /// 1. 验证请求参数（订单编号不能为空）。
        /// 2. 调用数据访问层查询订单主信息和关联的商品项，同时通过用户ID进行权限校验（确保用户只能查询自己的订单）。
        /// 3. 若订单不存在或不属于当前用户，返回null。
        /// 4. 若订单存在，将数据库实体转换为视图模型，补充状态文字描述（如将status=2转换为“已完成”）。
        /// 5. 返回组装后的订单详情（包含商品列表）。
        /// 
        /// 权限控制：
        /// - 必须验证订单的userId与请求中的userId一致，防止越权查询他人订单。
        /// </remarks>
        Task<OrderDetailResult> GetOrderByNoAsync(OrderQueryRequest request);

        /// <summary>
        /// 用户确认收货，将订单状态更新为“已完成”
        /// 用于用户收到商品后，手动确认收货，触发订单状态流转的最后一步
        /// </summary>
        /// <param name="orderNo">订单编号，标识需要确认收货的订单</param>
        /// <param name="userId">用户ID，用于验证订单归属权（确保是订单的创建者操作）</param>
        /// <returns>订单操作结果（OrderResult），包含操作是否成功及提示信息</returns>
        /// <remarks>
        /// 业务逻辑：
        /// 1. 验证参数（订单编号不能为空）。
        /// 2. 调用数据访问层查询订单，验证订单是否存在且属于当前用户（userId匹配）。
        /// 3. 检查订单当前状态：只有“已发货”（status=1）的订单可执行确认收货操作，其他状态（如待支付、已取消）不允许操作。
        /// 4. 若验证通过，将订单状态更新为“已完成”（status=2），并更新订单最后修改时间。
        /// 5. 返回操作结果（成功/失败及原因）。
        /// 
        /// 状态流转约束：
        /// - 仅允许从“已发货”（status=1）流转至“已完成”（status=2），避免状态乱序（如未发货订单直接标记为完成）。
        /// - 操作成功后，订单状态不可逆转（已完成状态无法再修改）。
        /// </remarks>
        Task<OrderResult> ConfirmReceiptAsync(string orderNo, long userId);
    }
}
