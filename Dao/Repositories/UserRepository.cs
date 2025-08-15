using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Models.Dto.LoginDto;
using Rongban.Models.Entities;
using System.Net;

namespace AuthSystem.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetPlatformDbContext _dbContext;

        public UserRepository(PetPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserByIdAsync(long userId)
        {
            return await _dbContext.UserInfos.FindAsync(userId);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserInfo> CreateUserAsync(UserInfo user)
        {
            _dbContext.UserInfos.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(UserInfo user)
        {
            _dbContext.UserInfos.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserByPhoneNumberAsync(byte credentialType, string credentialValue)
        {
            UserInfo user = null;

            // 根据凭证类型查询对应的用户字段
            // 手机号
             if (credentialType == 1)
            {
                user = await _dbContext.UserInfos
                    .FirstOrDefaultAsync(u => u.Phone == credentialValue);
            }
            // 邮箱
            else if (credentialType == 2)
            {
                user = await _dbContext.UserInfos
                    .FirstOrDefaultAsync(u => u.Email == credentialValue);
            }
            return user;
        }

        /// <summary>
        /// 获取凭证
        /// </summary>
        /// <param name="credentialType"></param>
        /// <param name="credentialValue"></param>
        /// <returns></returns>
        public async Task<UserCredential> GetCredentialAsync(byte credentialType, string credentialValue)
        {


            return await _dbContext.UserCredentials
                .Include(uc => uc.User)
                .FirstOrDefaultAsync(uc => uc.CredentialType == credentialType &&
                                         uc.CredentialValue == credentialValue);
        }

        /// <summary>
        /// 创建凭证
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public async Task<UserCredential> CreateCredentialAsync(UserCredential credential)
        {
            _dbContext.UserCredentials.Add(credential);
            await _dbContext.SaveChangesAsync();
            return credential;
        }

        /// <summary>
        /// 更新凭证
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public async Task UpdateCredentialAsync(UserCredential credential)
        {
            _dbContext.UserCredentials.Update(credential);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task AddVerificationCodeAsync(VerificationCode code)
        {
            _dbContext.VerificationCodes.Add(code);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 获取有效的验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<VerificationCode> GetValidVerificationCodeAsync(string phoneNumber, string code)
        {
            return await _dbContext.VerificationCodes
                .FirstOrDefaultAsync(vc => vc.Target == phoneNumber &&
                                         vc.Code == code &&
                                         vc.IsUsed == false &&
                                         vc.ExpireTime > DateTime.Now);
        }

        /// <summary>
        /// 标记验证码已使用
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public async Task MarkVerificationCodeAsUsedAsync(long codeId)
        {
            var code = await _dbContext.VerificationCodes.FindAsync(codeId);
            if (code != null)
            {
                code.IsUsed = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public async Task DeleteVerificationCodeAsync(long codeId)
        {
            var code = await _dbContext.VerificationCodes.FindAsync(codeId);
            if (code != null)
            {
                _dbContext.VerificationCodes.Remove(code);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
