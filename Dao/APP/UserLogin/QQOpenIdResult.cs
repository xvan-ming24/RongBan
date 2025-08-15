using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.UserLogin
{
    // QQ API返回结果模型
    public class QQOpenIdResult
    {
        public string ClientId { get; set; }
        public string OpenId { get; set; }
    }
}
