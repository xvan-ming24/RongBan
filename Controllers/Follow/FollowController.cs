using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.Cecil.Cil;
using Service.IService;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RongBan__V1.Controllers.Follow
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : Controller
    {
        private readonly IFollowService _iFollowService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FollowController(IFollowService iFollowService, IHttpContextAccessor httpContextAccessor)
        {
            _iFollowService = iFollowService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="request">关注请求参数</param>
        /// <returns>关注结果</returns>
        [HttpPost("follow")]
        [Authorize]
        public async Task<IActionResult> FollowUser([FromBody] long followedId)
        {
            // 从Token中获取当前登录用户ID
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = await _iFollowService.ToggleFollowAsync(currentUserId, followedId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }


        /// <summary>
        /// 获取当前登录用户关注的人的在线状态
        /// </summary>
        /// <param name="currentUserId">当前登录用户ID（实际项目中从Token中获取）</param>
        [HttpGet("loginfollowed-users-status")]
        [Authorize]
        public async Task<IActionResult> GetLoginFollowedUsersStatus()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _iFollowService.GetFollowedUsersStatusAsync(currentUserId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
        /// <summary>
        /// 获取当前登录用户粉丝列表
        /// </summary>
        /// <param name="currentUserId">当前登录用户ID（实际项目中从Token中获取）</param>
        [HttpGet("loginfans")]
        [Authorize]
        public async Task<IActionResult> GetLoginFans()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _iFollowService.GetFollowersAsync(currentUserId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
    }
}
