using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home
{
    public class AdoptPetDetailDto
    {
        // 宠物信息
        public string PetNickname { get; set; }
        public int? PetAge { get; set; }
        public byte? PetGender { get; set; }
        public string PetCharacteristics { get; set; }
        public byte? IsSterilized { get; set; }
        public string VaccineStatus { get; set; }

        public List<string> PetPhotoUrls { get; set; } = new List<string>();

        // 领养信息
        public long Id { get; set; }
        public long PetId { get; set; }
        public byte PublisherType { get; set; }
        public long PublisherId { get; set; }
        public string AdoptionRequirements { get; set; }
        public bool IsContractRequired { get; set; }
        public byte? Status { get; set; }
    }
}
