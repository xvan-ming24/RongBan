

using Microsoft.AspNetCore.Mvc;
using RongbanServeice;
using Service.IService;


namespace Rongban.Controllers.Mall
{
    [Route("api/[Controller]/[Action]")]
    //[ApiController]
    public class MallController : Controller
    {
        private readonly IPetStoreService _iPetStoreService;

        public MallController(PetStoreService iPetStoreService)
        {
            _iPetStoreService = iPetStoreService;
        }

        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var data = await _iPetStoreService.GetProduct();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }

        /// <summary>
        /// 获取商品媒体资源（图片视频等）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProductMedia()
        {
            var data = await _iPetStoreService.GetProductMedia();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
        /// <summary>
        /// 获取商城商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMallProducts()
        {
            var data = await _iPetStoreService.GetMallProducts();
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }

        /// <summary>
        /// 根据分类id获取商品信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMallProductsByCategoryId(long categoryId)
        {
            var data = await _iPetStoreService.GetMallProductsByCategoryId(categoryId);
            return Json(new
            {
                code = data.StatusCode,
                Message = data.Message,
                Data = data.Data
            });
        }
        /// <summary>
        /// 根据商品ID获取单个商品信息
        /// </summary>
        /// <param name="productId">商品ID（必填）</param>
        /// <returns>包含状态码、消息和商品详情的JSON响应</returns>
        [HttpGet] // 路由：api/product/by-id?productId=xxx
        public async Task<IActionResult> GetMallProductById(long productId)
        {
            // 调用服务层方法获取数据
            var result = await _iPetStoreService.GetMallProductById(productId);

            // 按指定格式返回JSON
            return Json(new
            {
                code = result.StatusCode,
                Message = result.Message,
                Data = result.Data
            });
        }
        /// <summary>
        /// 执行商品秒杀
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">秒杀商品ID</param>
        /// <returns>秒杀结果（含订单号或错误信息）</returns>
        [HttpPost]
        public async Task<IActionResult> ExecuteSeckill(long userId, long productId)
        {
            // 调用Service层执行秒杀逻辑
            var response = await _iPetStoreService.ExecuteSeckill(userId, productId);
            return Json(new
            {
                code = response.StatusCode,
                message = response.Message,
                data = response.Data
            });

        }

        /// <summary>
        /// 秒杀订单取消后恢复库存（管理员或定时任务调用）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="seckillVersion">当前秒杀版本号</param>
        /// <returns>恢复结果</returns>
        [HttpPost]
        public async Task<IActionResult> RestoreSeckillStock(long productId, int seckillVersion)
        {
            if (productId <= 0 || seckillVersion < 0)
            {
                return Json(new { code = 1, message = "参数错误（商品ID或版本号无效）" });
            }

            var response = await _iPetStoreService.RestoreSeckillStockAsync(productId, seckillVersion);
            return Json(new
            {
                code = response.StatusCode,
                message = response.Message,
                data = response.Data
            });
        }
        /// <summary>
        /// 获取所有正在进行的秒杀商品列表
        /// </summary>
        /// <returns>秒杀商品列表（含秒杀价格、库存、时间等信息）</returns>
        [HttpGet]
        public async Task<IActionResult> GetSeckillProducts()
        {
            var response = await _iPetStoreService.GetSeckillProducts();
            return Json(new
            {
                code = response.StatusCode,
                message = response.Message,
                data = response.Data
            });
        }
    }
}
