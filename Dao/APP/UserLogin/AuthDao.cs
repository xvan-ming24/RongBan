

using AuthSystem.DAL.Repositories;
using Common;
using Microsoft.Extensions.Configuration;
using Models.Dto.LoginDto;
using Rongban.Models.Entities;
using System.Xml.Linq;
using YourProject.Utilities;
using YourProject.Utils;

namespace Dao.APP.UserLogin
{
    public  class AuthDao
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtUtils _jwtUtils;
        private readonly SequenceGeneratorHeplper _sequenceGenerator;

        // 凭证类型常量
        private const byte WeChatCredentialType = 1;
        private const byte QQCredentialType = 2;
        private const byte PhoneCredentialType = 1;

        public AuthDao(IUserRepository userRepository, IConfiguration configuration,
                  IHttpClientFactory httpClientFactory, JwtUtils jwtUtils, SequenceGeneratorHeplper sequenceGenerator)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _jwtUtils = jwtUtils;
            _sequenceGenerator = sequenceGenerator;
        }

        /// <summary>
        /// 手机号验证码登录
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LoginResponse> PhoneCodeLoginAsync(PhoneCodeLoginDto phoneCodeLoginDto)
        {
            // 1. 验证验证码
            var code = await _userRepository.GetValidVerificationCodeAsync(phoneCodeLoginDto.phoneNumber, phoneCodeLoginDto.verificationCode);
            if (code == null)
            {
                throw new Exception("验证码无效或已过期");
            }
            var user = await _userRepository.GetUserByPhoneNumberAsync(PhoneCredentialType, phoneCodeLoginDto.phoneNumber);

            // 2. 查找或创建用户
            var credential = await _userRepository.GetCredentialAsync(PhoneCredentialType, phoneCodeLoginDto.phoneNumber);
            if (user == null) {
                user = new UserInfo
                {
                    Phone = phoneCodeLoginDto.phoneNumber,
                    RegisterTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };
                await _userRepository.CreateUserAsync(user);

            }
            if (credential == null)
            {

                credential = new UserCredential
                {
                    UserId = user.Id,
                    CredentialType = PhoneCredentialType,
                    CredentialValue = phoneCodeLoginDto.phoneNumber,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                };
                await _userRepository.CreateCredentialAsync(credential);
            }
            else
            {
                user = await _userRepository.GetUserByIdAsync(credential.UserId);
                user.LastLoginTime = DateTime.Now;
                await _userRepository.UpdateUserAsync(user);
            }
            // 3. 标记验证码为已使用
            await _userRepository.MarkVerificationCodeAsUsedAsync(code.Id);
            // 4. 生成JWT令牌
            //var token = GenerateJwtToken(user);

            var token = _jwtUtils.GenerateToken(
                Account: user.Phone,
                userId: user.Id
            );

            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Nickname = user.Nickname,
                AvatarUrl = user.AvatarUrl
            };
        }


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<string> SendVerificationCodeAsync(string phoneNumber)
        {
            using (var client = new HttpClient())
            {
                var PhopConfig = _configuration.GetSection("SmsProvider");
                var APIURL = PhopConfig["ApiUrl"];
                var APPID = PhopConfig["AppId"];
                var APIKEY = PhopConfig["ApiKey"];
                // 1. 生成6位验证码
                var random = new Random();
                var mobileCode = random.Next(100000, 999999).ToString();

                string content = $"您的验证码是：{mobileCode}。请不要把验证码泄露给其他人。";

                // 2. 保存验证码（有效期5分钟）
                var verificationCode = new VerificationCode
                {
                    Target = phoneNumber,
                    Code = mobileCode,
                    ExpireTime = DateTime.Now.AddMinutes(5)
                };
                await _userRepository.AddVerificationCodeAsync(verificationCode);
                var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("account", APPID),
                new KeyValuePair<string, string>("password", APIKEY),
                new KeyValuePair<string, string>("mobile", phoneNumber),
                new KeyValuePair<string, string>("content", content)
            };

                var contentToSend = new FormUrlEncodedContent(parameters);

                try
                {
                    var response = await client.PostAsync(APIURL, contentToSend);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    // 解析 XML 响应
                    // 解析 XML
                    XDocument xmlDoc = XDocument.Parse(responseBody);

                    // 从 XML 中获取信息
                    var code = xmlDoc.Root.Element(XName.Get("code", "http://106.ihuyi.com/"))?.Value;
                    var msg = xmlDoc.Root.Element(XName.Get("msg", "http://106.ihuyi.com/"))?.Value;
                    var smsid = xmlDoc.Root.Element(XName.Get("smsid", "http://106.ihuyi.com/"))?.Value;



                    Console.WriteLine($"code: {code}");
                    Console.WriteLine($"msg: {msg}");
                    Console.WriteLine($"smsid: {smsid}");
                    Console.WriteLine($"mo: {mobileCode}");

                    return code;
                }
                catch (Exception ex)
                {
                   return  ex.Message;
                }

                //await _userRepository.AddVerificationCodeAsync(verificationCode);

            }
        }
    }
}
