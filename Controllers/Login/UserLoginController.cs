
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Models.Dto.LoginDto;
using Rongban.Models.Entities;
using RongbanServeice;
using Service.IService;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Rongban.Controllers.Login
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]

    public class UserLoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly SessionHelper _sessionHelper;


        public UserLoginController(LoginService loginService, SessionHelper sessionHelper)
        {
            _loginService = loginService;
            _sessionHelper = sessionHelper;
        }
        /// <summary>
        /// 手机密码登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PhonePwdLoginAsync(LoginRequest loginRequest)
        { 
            var response = await _loginService.PhonePwdLoginAsync(loginRequest);
            return Json(new
            {
                code = response.StatusCode,
                Message = response.Message,
                Data = response.Data
            });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updatePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePassword updatePassword) {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            updatePassword.UserId = userId;
            var response = await _loginService.UpdatePasswordAsync(updatePassword);
            return Json(new
            {
                code = response.StatusCode,
                Message = response.Message
            });
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _loginService.RegisterAsync(registerDto);

            return Json(new
            {
                code = response.StatusCode,
                Message = response.Message,
                // Data = response.Data
            });
        }
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        [HttpPost("phone/send-code")]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendCodeRequest request)
        {
            try
            {
                await _loginService.SendVerificationCodeAsync(request.PhoneNumber);
                return Json(new
                {
                    code =200,
                    Message = "验证码已发送",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// 手机号验证码登录
        /// </summary>
        [HttpPost("phone/login")]
        public async Task<IActionResult> PhoneCodeLogin(PhoneCodeLoginDto phoneCodeLoginDto)
        {
            try
            {
                var data = await _loginService.PhoneCodeLoginAsync(phoneCodeLoginDto);
                return Json(new
                {
                    code = data.StatusCode,
                    Message = data.Message,
                    Data = data.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult LogOut()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = _loginService.LogOutAsync(userId).Result;

            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,

            });

        }
    }

    public class SendCodeRequest
    {
        public string PhoneNumber { get; set; }
    }
}
