using Models.Dto;
using Models.Dto.LoginDto;
using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ILoginService
    {
        Task<Response<UpdatePassword>> UpdatePasswordAsync(UpdatePassword updatePassword);
        Task<Response<RegisterDto>> RegisterAsync(RegisterDto registerDto);
        Task<Response<string>> LogOutAsync(long userId);
        Task<Response<string>> RecordLoginStatusAsync(UserOnlineDto userOnlineDto);
        Task<Response<string>> SendVerificationCodeAsync(string phoneNumber);
        /// <summary>
        /// 手机验证码登录
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task<Response<LoginResponse>> PhoneCodeLoginAsync(PhoneCodeLoginDto phoneCodeLoginDto);
        /// <summary>
        /// 手机密码登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<Response<LoginResponse>> PhonePwdLoginAsync(LoginRequest loginRequest);
    }
}
