using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Personal
{
    /// <summary>
    /// 宠物信息数据传输对象
    /// </summary>
    public class PetInfoDto
    {
        public long Id { get; set; }
        public string PetName { get; set; }
        public string Breed { get; set; }
        public string CategoryName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
        public string Characteristic { get; set; }
        public string Sterilization { get; set; }
        public string Vaccine { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
