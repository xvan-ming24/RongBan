using Dao.APP.Personal;
using Models.Dto.Personal;
using Rongban.Models.Entities;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class PersonalPetInfoService:IPersonalPetInfoService
    {
        private readonly PersonalPetInfoDao _personalPetInfoDao;
        public PersonalPetInfoService(PersonalPetInfoDao personalPetInfoDao)
        {
            _personalPetInfoDao = personalPetInfoDao;
        }
        public async Task<Response<PetInfoDto>> GetPetByUserIdAndPetIdAsync(long userId, long petId)
        {
            // 可以在这里添加业务逻辑验证，如权限检查等
            if (userId <= 0 || petId <= 0)
            {
                return null;
            }

            var pet = await _personalPetInfoDao.GetPetByUserIdAndPetIdAsync(userId, petId);

            var res = new PetInfoDto
            {
                Id = pet.Id,
                PetName = pet.PetName,
                Breed = pet.Breed,
                CategoryName = pet.Category?.CategoryName,
                Gender = GetGenderText(pet.Gender),
                Age = pet.Age,
                Weight = pet.Weight,
                Characteristic = pet.Characteristic,
                Sterilization = pet.Sterilization == 1 ? "是" : "否",
                Vaccine = pet.Vaccine,
                CreateTime = pet.CreateTime
                // 如需增减字段，直接在这里修改，无需改动数据访问层
            };
           return Response<PetInfoDto>.Success(res,"查询成功");
        }

        public async Task<Response<List<PetInfoDto>>> GetPetsByUserIdAsync(long userId)
        {
            // 可以在这里添加业务逻辑验证，如权限检查等
            if (userId <= 0)
            {
               return Response<List<PetInfoDto>>.Fail("请输入正确的用户id");
            }

            var data = await _personalPetInfoDao.GetPetsByUserIdAsync(userId);

            var res = data.Select(pet => new PetInfoDto
            {
                Id = pet.Id,
                PetName = pet.PetName,
                Breed = pet.Breed,
                CategoryName = pet.Category.CategoryName,
                Gender = GetGenderText(pet.Gender),
                Age = pet.Age,
                Weight = pet.Weight,
                Characteristic = pet.Characteristic,
                Sterilization = pet.Sterilization == 1 ? "是" : "否",
                Vaccine = pet.Vaccine,
                CreateTime = pet.CreateTime

            }).ToList();
            return Response<List<PetInfoDto>>.Success(res, "查询成功");
        }
        private static string GetGenderText(byte? gender)
        {
            return gender switch
            {
                1 => "公",
                2 => "母",
                _ => "未知"
            };
        }
    }
}
