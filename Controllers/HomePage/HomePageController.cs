using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RongbanServeice;
using Service.IService;


namespace Rongban.Controllers.HomePage
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class HomePageController : Controller
    {
        private readonly IHomeCarouselService _iHomeCarouselService;

        public HomePageController(HomeCarouselService iHomeCarouselService)
        {
            _iHomeCarouselService = iHomeCarouselService;
        }
        /// <summary>,
        /// 首页轮播图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHomeCarousels()
        {
            var data = await _iHomeCarouselService.GetHomeCarousels();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }

        /// <summary>
        /// 直播排行榜 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHomeLives()
        {
            var data = await _iHomeCarouselService.GetHomeLives();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }

        /// <summary>
        /// 获取首页精选
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHomeSelectedDto()
        {
            var data = await _iHomeCarouselService.GetHomeSelectedDto();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
    }
}
