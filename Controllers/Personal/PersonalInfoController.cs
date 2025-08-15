using Dao.APP.Personal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Service.IService;
using System.Security.Claims;

namespace RongBan.Controllers.Personal
{
  
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class PersonalInfoController : Controller
    {
        private readonly IPersonalInfoService _personalInfoService;

        public PersonalInfoController(IPersonalInfoService personalInfoService)
        {
            _personalInfoService = personalInfoService;
        }
        /// <summary>
        /// 根据Id查询用户信息、获赞、关注、粉丝数、互关
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (currentUserId == null)
                return Unauthorized("请登录");
            var data = await _personalInfoService.GetUserProfile(currentUserId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                data = data.Data
            });
        }
    }
}
