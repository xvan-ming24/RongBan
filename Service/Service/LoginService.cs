using Chessie.ErrorHandling;
using Common;
using Dao.APP.UserLogin;
using Microsoft.AspNetCore.Http;
using Models.Dto;
using Models.Dto.LoginDto;
using Paket;
using RongbanDao;
using RongbanDao.APP;
using Service.IService;
using YourProject.Utils;

namespace RongbanServeice
{

    public class LoginService : ILoginService
    {
        private readonly LoginDao _loginDao;
        private readonly JwtUtils _jwtUtils;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionHelper _sessionHelper;
        private readonly UserOnlineDao _userOnlineDao;
        private readonly RegisterDao _registerDao;
        private readonly LogOutDao _logOutDao;
        private readonly AuthDao _authDao;


        public LoginService(LoginDao loginDao,
            JwtUtils jwtUtils,
            SessionHelper sessionService,
            UserOnlineDao userOnlineDao,
            IHttpContextAccessor httpContextAccessor,
            RegisterDao registerDao,
            LogOutDao logOutDao,
            AuthDao authDao)
        {
            _loginDao = loginDao;
            _jwtUtils = jwtUtils;
            _sessionHelper = sessionService;
            _userOnlineDao = userOnlineDao;
            _httpContextAccessor = httpContextAccessor;
            _registerDao = registerDao;
            _logOutDao = logOutDao;
            _authDao = authDao;
        }
        /// <summary>
        /// 手机密码登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<Response<LoginResponse>> PhonePwdLoginAsync(LoginRequest loginRequest) {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Phone)) { 

                    return Response<LoginResponse>.Fail("请输入手机号码");
                }
                if (string.IsNullOrEmpty(loginRequest.Password))
                {
                    return Response<LoginResponse>.Fail("请输入密码");
                }
                var res = await _loginDao.PhonePwdLoginAsync(loginRequest);
                if (res == null)
                {
                    return Response<LoginResponse>.Fail("用户不存在");
                }
                //记录登录状态
                var ipAddress = IpAddressHelper.GetClientIpAddress(_httpContextAccessor.HttpContext);
                UserOnlineDto userOnlineDto = new UserOnlineDto
                {
                    userId = res.Id,
                    Phone = loginRequest.Phone,
                    deviceId = loginRequest.deviceId,
                    deviceName = loginRequest.deviceName,
                    ipAddress = ipAddress
                };
                await _userOnlineDao.RecordLoginStatusAsync(userOnlineDto);
                //生成token
                var token = _jwtUtils.GenerateToken(
                    Account: res.Phone,
                    userId: res.Id
                );
                // 准备返回数据
                var responseData = new LoginResponse
                {
                    Token = token,
                    Expiration = DateTime.Now.AddMinutes(30 * 24 * 60),
                    Phone = res.Phone,
                    UserId = res.Id,
                    Nickname = res.Nickname 
                };
                return Response<LoginResponse>.Success(responseData, "登录成功");
            }catch (Exception ex)
            { 
                LogHelper.Error<LoginService>("登录过程中发生错误",ex);
                return Response<LoginResponse>.Fail("登录过程中发生错误，请稍后重试");
            }
        }
        /// <summary>
        /// 手机号验证码登录
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<Response<LoginResponse>> PhoneCodeLoginAsync(PhoneCodeLoginDto phoneCodeLoginDto)
        {

            try
            {
                if (string.IsNullOrEmpty(phoneCodeLoginDto.phoneNumber))
                {

                    return Response<LoginResponse>.Fail("请输入手机号码");
                }
                if (string.IsNullOrEmpty(phoneCodeLoginDto.verificationCode))
                {
                    return Response<LoginResponse>.Fail("请输入验证码");
                }
                var res = await _authDao.PhoneCodeLoginAsync(phoneCodeLoginDto);
                if (res == null)
                {
                    return Response<LoginResponse>.Fail("用户不存在");
                }
                //记录登录状态
                var ipAddress = IpAddressHelper.GetClientIpAddress(_httpContextAccessor.HttpContext);
                UserOnlineDto userOnlineDto = new UserOnlineDto
                {
                    userId = res.UserId,
                    Phone = phoneCodeLoginDto.phoneNumber,
                    deviceId = phoneCodeLoginDto.deviceId,
                    deviceName = phoneCodeLoginDto.deviceName,
                    ipAddress = ipAddress
                };
                await _userOnlineDao.RecordLoginStatusAsync(userOnlineDto);
                //生成token
                var token = _jwtUtils.GenerateToken(
                    Account: phoneCodeLoginDto.phoneNumber,
                    userId: res.UserId
                );
                // 准备返回数据
                var responseData = new LoginResponse
                {
                    Token = token,
                    Expiration = DateTime.Now.AddMinutes(30 * 24 * 60),
                    Phone = phoneCodeLoginDto.phoneNumber,
                    UserId = res.UserId,
                    Nickname = res.Nickname
                };
                return Response<LoginResponse>.Success(responseData, "登录成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<LoginService>("登录过程中发生错误", ex);
                return Response<LoginResponse>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updatePassword"></param>
        /// <returns></returns>
        public async Task<Response<UpdatePassword>> UpdatePasswordAsync(UpdatePassword updatePassword)
        {
            try
            {
                if (updatePassword.NewPassword == updatePassword.OldPassword)
                {

                    return Response<UpdatePassword>.Fail("新密码不能与旧密码相同");
                }
                if (updatePassword.NewPassword != updatePassword.ConfirmPassword)
                {
                    return Response<UpdatePassword>.Fail("新密码与确认密码不一致");
                }
                LogHelper.Info<LoginService>( $"开始修改密码");
                int result = await _loginDao.UpdatePasswordAsync(updatePassword);
                if (result > 0)
                {
                    LogHelper.Info<LoginService>($"修改成功");
                    return Response<UpdatePassword>.Success(updatePassword, "修改成功！");
                }
                else
                {
                    
                    return Response<UpdatePassword>.Fail("修改失败");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<LoginService>("登录过程中发生错误", ex);
                return Response<UpdatePassword>.Fail("修改过程中发生错误，请稍后重试");
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public async Task<Response<RegisterDto>> RegisterAsync(RegisterDto registerDto)
        {


            try
            {
                // 错误原因：如果 AuthService 是静态类，这里会报错
                // 修正：使用当前实例的类型（this.GetType()）或 typeof(AuthService)
                LogHelper.Info<LoginService>($"开始用户注册");


                int result = await _registerDao.RegisterAsync(registerDto);

                if (result > 0)
                {
                    LogHelper.Info<LoginService>($"用户注册成功");

                    return Response<RegisterDto>.Success(registerDto, "注册成功！");
                }
                else
                {
                    LogHelper.Warn<LoginService>($"用户注册失败");
                    return Response<RegisterDto>.Fail("注册失败", Response<RegisterDto>.StatusNotFound);
                }
            }
            catch (Exception ex)
            {
                // 同样使用 typeof(AuthService) 代替泛型方法
                LogHelper.Error<LoginService>($"注册过程中发生错误",ex);
                return Response<RegisterDto>.Fail("注册过程中发生错误，请稍后重试");
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<string>> LogOutAsync(long userId)
        {
            try
            {
                LogHelper.Info<LoginService>($"开始注销用户{userId}的状态");

                int res =await _logOutDao.LogOut(userId);
                if (res > 0)
                {
                    LogHelper.Info<LoginService>($"用户注销成功");
                    return Response<string>.SuccessWithoutData("注销成功");
                }
                LogHelper.Error<LoginService>($"用户注销失败");
                return Response<string>.FailWithoutData("注销失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error<LoginService>($"用户注销过程中发生错误", ex);
                return Response<string>.FailWithoutData("注销过程中发生错误，请稍后重试");
            }
        }

        /// <summary>
        /// 登录获取用户在线状态
        /// </summary>
        /// <param name="userOnlineDto"></param>
        /// <returns></returns>
        public async Task<Response<string>> RecordLoginStatusAsync(UserOnlineDto userOnlineDto)
        {
            LogHelper.Info<LoginService>($"开始记录用户{userOnlineDto.userId}的状态");
            try 
            {
                var res = await _userOnlineDao.RecordLoginStatusAsync(userOnlineDto);
                if (res > 0)
                {
                    return Response<string>.SuccessWithoutData("用户在线！");
                }
                LogHelper.Warn<LoginService>($"用户状态记录失败");
                return Response<string>.FailWithoutData("状态记录失败！");
            } catch (Exception ex)
            {
                LogHelper.Error<LoginService>($"用户状态记录过程中发生错误", ex);
                return Response<string>.FailWithoutData("状态记录过程中发生错误，请稍后重试");
            }

        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<Response<string>> SendVerificationCodeAsync(string phoneNumber)
        {
            try
            {
                LogHelper.Info<LoginService>($"开始发送验证码");
                var res = await _authDao.SendVerificationCodeAsync(phoneNumber);
                if (res == "2")
                {
                    LogHelper.Info<LoginService>($"短信发送成功");
                    return Response<string>.SuccessWithoutData("发送成功！");
                }
                LogHelper.Warn<LoginService>($"短信发送失败");
                return Response<string>.SuccessWithoutData("发送失败！");
            }
            catch (Exception ex)
            {
                LogHelper.Error<LoginService>($"发送验证码过程中发生错误", ex);
                return Response<string>.FailWithoutData("发送过程中发生错误，请稍后重试");
            }

        }

    }
}
