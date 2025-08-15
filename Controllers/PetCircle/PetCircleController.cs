using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dto.PetCircle;
using Service.IService;
using static Models.Dto.PetCircle.PublishUpdatesDto;
using System.Security.Claims;


namespace RongBan__V1.Controllers.PetCircle
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class PetCircleController : Controller
    {
        private readonly IPetCircleService _iPetCircleService;

        public PetCircleController(IPetCircleService iPetCircleService)
        {
            _iPetCircleService = iPetCircleService;
        }
        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="pageIndex">页码，默认1</param>
        /// <param name="pageSize">每页数量，默认20</param>
        /// <returns>动态列表</returns>
        [HttpGet]
        public async Task<ActionResult<List<MomentListDto>>> GetMomentList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20)
        {
            var data = await _iPetCircleService.GetMomentListAsync(pageIndex, pageSize);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
        /// <summary>
        /// 获取动态详情
        /// </summary>
        /// <param name="id">动态ID</param>
        /// <returns>动态详情</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MomentDetailDto>> GetMomentDetail(long id)
        {
            var data = await _iPetCircleService.GetMomentDetailAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
        /// <summary>
        /// 发布新动态
        /// </summary>
        /// <param name="dto">动态内容数据</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize] // 需要身份验证，确保用户已登录
        public async Task<IActionResult> CreateMoment(CreatePetMomentDto dto)
        {
            try
            {
                // 1. 模型验证（由[ApiController]特性自动完成）
                // 2. 从身份令牌中获取当前登录用户ID
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                {
                    return BadRequest("无法获取用户信息，请重新登录");
                }

                // 3. 调用服务层创建动态
                var data = await _iPetCircleService.CreateMomentAsync(userId, dto);

                return Json(new
                {
                    code = data.StatusCode,
                    Message = data.Message,
                    Data = data.Data
                });
               // return CreatedAtAction(nameof(GetMomentById), new { id = result.Data.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                // 处理用户不存在的情况
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                // 处理参数错误的情况
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // 处理其他未预料的错误
                return StatusCode(StatusCodes.Status500InternalServerError, $"发布动态失败: {ex.Message}");
            }
        }
    }
}
