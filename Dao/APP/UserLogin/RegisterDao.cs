using AuthSystem.DAL.Repositories;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.Win32;
using Models.Dto;
using Rongban.Models.Entities;
using YourProject.Utilities;


namespace RongbanDao.APP
{
    public class RegisterDao
    {
        private readonly PetPlatformDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly SequenceGeneratorHeplper _sequenceGenerator;

        // 凭证类型常量
        private const byte WeChatCredentialType = 1;
        private const byte QQCredentialType = 2;
        private const byte PhoneCredentialType = 1;

        public RegisterDao(PetPlatformDbContext dbContext, IUserRepository userRepository, SequenceGeneratorHeplper sequenceGenerator)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _sequenceGenerator = sequenceGenerator;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                await Task.Delay(100); // 模拟异步操作
                // 密码哈希
                byte[] salt = PasswordHasher.GenerateSalt();
                // 密码加密
                registerDto.PasswordHash = PasswordHasher.HashPassword(registerDto.PasswordHash, salt);


                // 创建用户
                var user = new UserInfo
                {
                    PasswordHash = registerDto.PasswordHash,
                    Phone = registerDto.Phone,
                    RegisterTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };
               
                _dbContext.UserInfos.Add(user);
                // 创建登录凭证
                var res =  await _dbContext.SaveChangesAsync();
                var credential = new UserCredential
                {
                    UserId = user.Id,
                    CredentialType = PhoneCredentialType,
                    CredentialValue = registerDto.Phone,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                };
                await _userRepository.CreateCredentialAsync(credential);
                return res;
            }
            catch (Exception ex)
            {
                // 记录日志或进行其他错误处理
                throw new Exception($"注册失败: {ex.Message}", ex);
            }
        }
    }
}
