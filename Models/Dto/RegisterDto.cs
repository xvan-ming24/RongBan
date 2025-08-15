using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto
{
    public class RegisterDto
    {
        public string? Phone { get; set; }

        public string? PasswordHash { get; set; }
        public string? VerificationCode { get; set; }

        public DateTime? RegisterTime { get; set; } = DateTime.Now;
    }
}
