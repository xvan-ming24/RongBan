using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.LoginDto
{
    /// <summary>
    /// 登录请求模型
    /// </summary>
    public class LoginRequest
    {
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string deviceId { get; set; }
        public string deviceName { get; set; }
    }
}
