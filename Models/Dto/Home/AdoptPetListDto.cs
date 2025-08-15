using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home
{
    public class AdoptPetListDto
    {
        // 宠物信息
      //  public string PetImage { get; set; }
        public string? PetNickname { get; set; }
        public int? PetAge { get; set; }
        public byte? PetGender { get; set; }
        public string PetCharacteristics { get; set; }
        public byte? IsSterilized { get; set; }
        public string VaccineStatus { get; set; }

        public string PetPhotoUrl { get; set; }

        // 领养信息
        public string AdoptionRequirements { get; set; }
        public long AdoptionId { get; set; }
    }
}
