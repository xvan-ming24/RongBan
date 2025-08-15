using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;
using PetApi.Services;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PetApi.Controllers.AI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultController : ControllerBase
    {
        private readonly PetPlatformDbContext _context; // 替换为你的实际DbContext
        private readonly AiService _aiService;

        // 构造函数注入
        public ConsultController(PetPlatformDbContext context, AiService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        /// <summary>
        /// AI问诊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitConsult(ConsultRequest request)
        {
            var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(currentUserId == null)
            {
                return BadRequest("用户未登录");
            }
            // 1. 验证请求数据
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. 验证外键是否存在（关键修复点）
            // 验证用户ID
            var userExists = await _context.UserInfos
                .AnyAsync(u => u.Id == currentUserId);
            if (!userExists)
            {
                return BadRequest($"用户ID {currentUserId} 不存在，请检查");
            }

            // 验证宠物ID
            var pet = await _context.PetInfos
                .FirstOrDefaultAsync(p => p.Id == request.PetId);
            if (pet == null)
            {
                return BadRequest($"宠物ID {request.PetId} 不存在，请检查");
            }

            // 验证服务ID
            var serviceExists = await _context.PetMedicalServices
                .AnyAsync(s => s.Id == 1);
            if (!serviceExists)
            {
                return BadRequest($"服务ID {1} 不存在，请检查");
            }

            try
            {
                // 3. 调用AI服务获取诊断结果
                var aiDiagnosis = await _aiService.GetDiagnosis(request.Symptoms, pet);

                // 4. 创建问诊记录实体
                var consultRecord = new AiConsultRecord
                {
                    UserId = currentUserId,
                    PetId = request.PetId,
                    Symptoms = request.Symptoms,
                    AiDiagnosis = aiDiagnosis,
                    ServiceId =1,
                    ConsultationTime = DateTime.Now // 可使用默认值，也可显式设置
                };

                // 5. 保存到数据库
                _context.AiConsultRecords.Add(consultRecord);
                await _context.SaveChangesAsync(); // 原错误发生在这一行

                return Ok(new
                {
                    code = 200,
                    Message = "问诊记录提交成功",
                    ConsultId = consultRecord.Id,
                    Diagnosis = aiDiagnosis
                });
            }
            catch (Exception ex)
            {
                // 6. 异常处理
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "提交失败",
                    Error = ex.Message
                });
            }
        }
    }

    // 咨询请求模型
    public class ConsultRequest
    {
        public long PetId { get; set; }
        public string Symptoms { get; set; }
     
    }
}
