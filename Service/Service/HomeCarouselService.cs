using Chessie.ErrorHandling;
using Common;
using Dao.APP.Home;
using Models.Dto;
using Models.Dto.Home;
using Rongban.Models.Entities;
using Service.IService;
using System.Collections.Generic;

namespace RongbanServeice
{
    public class HomeCarouselService: IHomeCarouselService
    {
        private readonly HomePageDao _homePageDao;

        public HomeCarouselService(HomePageDao homePageDao)
        {
            _homePageDao = homePageDao;
        }

        /// <summary>
        /// 获取首页轮播图
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<HomeCarousel>>> GetHomeCarousels()
        {
            try
            {
                var result = await _homePageDao.GetHomeCarousels();
                return Response<List<HomeCarousel>>.Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<HomeCarouselService>($"查询失败",ex);
                return Response<List<HomeCarousel>>.Fail($"查询失败{ex.Message}");
            }

        }

        /// <summary>
        /// 获取首页直播
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<HomeLiveDto>>> GetHomeLives()
        {
            try 
            {
                var result = await _homePageDao.GetHomeLives();
                return Response<List<HomeLiveDto>>.Success(result, "查询成功");
            } catch (Exception ex) 
            {
                LogHelper.Error<HomeCarouselService>($"查询失败", ex);
                return Response<List<HomeLiveDto>>.Fail($"查询失败{ex.Message}");
            }
        }

        /// <summary>
        /// 获取首页精选
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<HomeSelectedDto>>> GetHomeSelectedDto()
        {
            try
            { 
                var result = await _homePageDao.GetHomeSelectedDto();
                return Response<List<HomeSelectedDto>>.Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<HomeCarouselService>($"查询失败", ex);
                return Response<List<HomeSelectedDto>>.Fail($"查询失败{ex.Message}");
            }
        }
    }
}
