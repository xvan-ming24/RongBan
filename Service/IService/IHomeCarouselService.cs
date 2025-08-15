using Models.Dto.Home;
using Rongban.Models.Entities;

namespace Service.IService
{
    public interface IHomeCarouselService
    {
        Task<Response<List<HomeCarousel>>> GetHomeCarousels();
        Task<Response<List<HomeLiveDto>>> GetHomeLives();
        Task<Response<List<HomeSelectedDto>>> GetHomeSelectedDto();
    }
}
