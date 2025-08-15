using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common
{
    // 自定义服务：封装Session操作
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        // 构造函数注入IHttpContextAccessor
        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // 保存用户信息到Session
        public void SaveUserInfo(UserInfoSession user)
        {
            // 复杂对象需要序列化（这里用System.Text.Json）
            string userJson = JsonSerializer.Serialize(user);
            _session.SetString("CurrentUser", userJson);
        }

        // 从Session获取用户信息
        public UserInfoSession GetUserInfo()
        {
            string userJson = _session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(userJson))
                return null;

            // 反序列化为对象
            return JsonSerializer.Deserialize<UserInfoSession>(userJson);
        }

        // 清除Session中的用户信息
        public void ClearUserInfo()
        {
            _session.Remove("CurrentUser");
        }
    }
    public class UserInfoSession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
