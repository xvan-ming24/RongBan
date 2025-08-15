using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Models.Dto.Home;
using Models.Dto.Home.SignIn;
using Service.IService;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RongBan.Controllers.HomePage
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class HomeVajraController : Controller
    {
        private readonly IHomeVajraService _homeVajraService;
        public HomeVajraController(IHomeVajraService homeVajraService)
        {
            _homeVajraService = homeVajraService;
        }
        /// <summary>
        /// 获取领养信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<AdoptPetListDto>>> GetAdoptionList()
        {
            var data = await _homeVajraService.GetAdoptionListAsync();
            return Json(new { 
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }

        /// <summary>
        /// 根据ID获取领养信息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptPetDetailDto>> GetAdoptionDetail(long id)
        {
            var data = await _homeVajraService.GetAdoptionDetailAsync(id);

            if (data == null)
            {
                return NotFound();  // 如果没找到，返回404 Not Found
            }

            return Json(new { 
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            }); 
        }
        /// <summary>
        /// 获取可提供寄养服务的门店列表
        /// </summary>
        /// <returns>门店列表</returns>
        [HttpGet("orgs")]
        public async Task<IActionResult> GetFosterOrgs()
        {
            var data = await _homeVajraService.GetFosterOrgListAsync();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CheckinResultDto>> Checkin()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(currentUserId == null)
            {
                return Json(new
                {
                    code = "404",
                    Message = "用户未登录"
                });
            }
            var data = await _homeVajraService.CheckinAsync(currentUserId);
            return Json(new
            { 
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }
        /// <summary>
        /// 获取今日任务
        /// </summary>
        /// <returns></returns>
        [HttpGet("today")]
        [Authorize]
        public async Task<ActionResult<List<UserTaskDto>>> GetTodayTasks()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (currentUserId == null)
            {
                return Json(new
                {
                    code = "404",
                    Message = "用户未登录"
                });
            }
            var data = await _homeVajraService.GetTodayTasksAsync(currentUserId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost("complete/{taskId}")]
        [Authorize]
        public async Task<ActionResult<TaskCompleteResultDto>> CompleteTask(long taskId)
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (currentUserId == null)
            {
                return Json(new
                {
                    code = "404",
                    Message = "用户未登录"
                });
            }
            var data = await _homeVajraService.CompleteTaskAsync(currentUserId, taskId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }
    }
}
