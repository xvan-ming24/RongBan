using Models.Dto.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IPersonalInfoService
    {
        Task<Response<UserProfileDto>> GetUserProfile(long userId);
    }
}
