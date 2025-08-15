using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.LoginDto
{
    public class PhoneCodeLoginDto
    {
        public string phoneNumber { get; set; }
        public string verificationCode { get; set; }
        public string deviceId { get; set; }
        public string deviceName { get; set; }
    }
}
