
using Common;

using Microsoft.EntityFrameworkCore;
using Models.Dto.LoginDto;
using Rongban.Models.Entities;
using System.Security.Claims;


namespace RongbanDao.APP
{
    public class LoginDao
    {
        private readonly PetPlatformDbContext _dbContext;
        private readonly SessionHelper _sessionHelper;

        public LoginDao(PetPlatformDbContext dbContext, SessionHelper sessionService)
        {
            _dbContext = dbContext;
            _sessionHelper = sessionService;
        }
        /// <summary>
        /// 手机号密码登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserInfo> PhonePwdLoginAsync(LoginRequest loginRequest)
        {
            try {
                await Task.Delay(100);

                var user = await _dbContext.UserInfos.FirstOrDefaultAsync(x => x.Phone == loginRequest.Phone);
                if (user == null)
                {

                    return null;
                }

                string storedHashWithSalt = user.PasswordHash;
                bool isPasswordCorrect = PasswordHasher.VerifyPassword(loginRequest.Password, storedHashWithSalt);
                Console.WriteLine(isPasswordCorrect ? "密码正确" : "密码错误");
                return isPasswordCorrect ? user : null;
            }
            catch (Exception ex)
            {
                // 记录日志或进行其他错误处理
                throw new Exception($"登录失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updatePassword"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UpdatePasswordAsync(UpdatePassword updatePassword)
        {
            try
            {
                await Task.Delay(100); // 模拟异步操作
                //查询用户
                var user = await _dbContext.UserInfos.FirstOrDefaultAsync(x => x.Id == updatePassword.UserId);

                string storedHashWithSalt = user.PasswordHash;
                bool isPasswordCorrect = PasswordHasher.VerifyPassword(updatePassword.OldPassword, storedHashWithSalt);
                if (isPasswordCorrect) {
                    // 密码哈希
                    byte[] salt = PasswordHasher.GenerateSalt();
                    // 密码加密
                    user.PasswordHash = PasswordHasher.HashPassword(updatePassword.NewPassword, salt);
                    //user.PasswordHash = updatePassword.NewPassword;

                    _dbContext.UserInfos.Update(user);
                   return await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"修改失败: {ex.Message}", ex);
            }
            throw new NotImplementedException();
        }
    }
}
