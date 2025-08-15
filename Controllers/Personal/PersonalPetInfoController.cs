using Dao.APP.Personal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dto.Personal;
using Service.IService;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RongBan.Controllers.Personal
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPetInfoController : Controller
    {
        private readonly IPersonalPetInfoService _personalPetInfoService;
        public PersonalPetInfoController(IPersonalPetInfoService personalPetInfoService)
        {
            _personalPetInfoService = personalPetInfoService;
        }
        /// <summary>
        /// 获取用户宠物信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PetInfoDto>> GetPetByUserIdAndPetId(long petId)
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _personalPetInfoService.GetPetByUserIdAndPetIdAsync(currentUserId, petId);

            if (data == null)
            {
                return NotFound("未找到该用户关联的宠物信息");
            }

            return Json(new { 
                code = data.StatusCode,
                message = data.Message,
                data = data.Data
            });
        }
        /// <summary>
        /// 根据用户ID获取名下所有宠物信息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>宠物信息列表</returns>
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<PetInfoDto>>> GetPetsByUserId()
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _personalPetInfoService.GetPetsByUserIdAsync(currentUserId);
            return Json(new
            {
                code = data.StatusCode,
                message = data.Message,
                data = data.Data
            });
        }
    }
}
