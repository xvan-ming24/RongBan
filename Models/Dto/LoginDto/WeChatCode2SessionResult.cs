using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.LoginDto
{
    // 微信API返回结果模型
    public class WeChatCode2SessionResult
    {
        public string OpenId { get; set; }
        public string SessionKey { get; set; }
        public string UnionId { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }
}
