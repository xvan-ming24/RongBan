using Microsoft.AspNetCore.Mvc;
using Rongban.BLL.Services;
using Rongban.Models.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rongban.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 直接购买商品
        /// </summary>
        [HttpPost("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] BuyNowRequest request)
        {
            // 从身份认证中获取当前用户ID
            var userId = GetCurrentUserId();
            if (userId <= 0)
                return Unauthorized("请先登录");

            var result = await _orderService.BuyNowAsync(userId, request);
            return Ok(result);
        }

        /// <summary>
        /// 从购物车一键购买
        /// </summary>
        [HttpPost("buy-from-cart")]
        public async Task<IActionResult> BuyFromCart([FromBody] ReceiverInfo receiverInfo)
        {
            // 从身份认证中获取当前用户ID
            var userId = GetCurrentUserId();
            if (userId <= 0)
                return Unauthorized("请先登录");

            var result = await _orderService.BuyFromCartAsync(userId, receiverInfo);
            return Ok(result);
        }

        /// <summary>
        /// 获取当前登录用户ID
        /// </summary>
        private long GetCurrentUserId()
        {
            // 实际项目中从JWT令牌或其他身份认证方式获取用户ID
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                return userId;
            }
            return 0;
        }

        /// <summary>
        /// 根据订单号查询订单详情
        /// </summary>
        /// <param name="request">查询参数（订单号+用户ID）</param>
        [HttpPost("query")]
        public async Task<IActionResult> QueryOrder([FromBody] OrderQueryRequest request)
        {
            // 从身份认证中获取当前用户ID
            var userId = GetCurrentUserId();
            if (userId <= 0)
                return Unauthorized("请先登录");
            request.UserId = userId;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.GetOrderByNoAsync(request);
            if (result == null)
                return NotFound(new { Message = "订单不存在或无权访问" });

            return Ok(result);
        }

        /// <summary>
        /// 用户确认收货（标记订单为已完成）
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="userId">用户ID（从Token获取）</param>
        [HttpPost("confirm-receipt")]
        public async Task<IActionResult> ConfirmReceipt(string orderNo)
        {
            var userId = GetCurrentUserId();
            if (userId <= 0)
                return Unauthorized("请先登录");


            if (string.IsNullOrEmpty(orderNo))
                return BadRequest(new { Message = "订单编号不能为空" });

            var result = await _orderService.ConfirmReceiptAsync(orderNo, userId);
            return Ok(result);
        }
    }
}
