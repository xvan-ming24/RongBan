using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto
{
    public class UserOnlineDto
    {
        public long userId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string deviceId { get; set; }
        public string deviceName { get; set; }
        public string ipAddress { get; set; }
        
    }
}
