using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.LoginDto
{
    /// <summary>
    /// 登录响应模型
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// JWT令牌字符串
        /// 用于客户端后续请求的身份验证，格式为Bearer Token
        /// 例如：eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// </summary>
        public required string Token { get; set; }
        /// <summary>
        /// 令牌过期时间
        /// 客户端可根据此时间提前刷新令牌，避免请求失败
        /// 通常为UTC时间或服务器本地时间
        /// </summary>
        public DateTime Expiration { get; set; }
        /// <summary>
        /// 用户名/账号
        /// 标识当前令牌对应的用户身份，用于前端显示或业务判断
        /// </summary>
        public  string Phone { get; set; }
        public string Email { get; set; }
        public required long UserId { get; set; }
        public  string Nickname { get; set; }
        public  string AvatarUrl { get; set; }
    }
}
