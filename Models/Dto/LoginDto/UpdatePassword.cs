using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.LoginDto
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdatePassword
    {
        public long UserId { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = null!;
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = null!;
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; } = null!;
    }
}
