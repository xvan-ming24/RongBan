using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Security.Claims;


namespace RongBan__V1.Controllers.UserOnline
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class UserOnlineController : Controller
    {
        private readonly IUserOnlineService _iUserOnlineService;
        public UserOnlineController(IUserOnlineService iUserOnlineService)
        {
            _iUserOnlineService = iUserOnlineService;
        }
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserActivityAsync( string deviceId, string ipAddress)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _iUserOnlineService.UpdateUserActivityAsync(currentUserId, deviceId, ipAddress);
            return Ok(new
            {
                code = data.StatusCode,
                message = data.Message
            });
        }
        /// <summary>
        /// 查询用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckOnlineStatus()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _iUserOnlineService.IsUserOnlineAsync(currentUserId);

            return Ok(new
            {
                code = data.StatusCode,
                message = data.Message,
                data = data.Data
            });
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDetailedStatus()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _iUserOnlineService.GetUserStatusAsync(currentUserId);
            return Ok(new
            {
                code = data.StatusCode,
                message = data.Message,
                data = data.Data
            });
        }
    }
}
