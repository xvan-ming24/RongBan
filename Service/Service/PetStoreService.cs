using Common;
using Dao.APP.PetStore;
using Models.Dto.PetStore;
using Rongban.Models.Entities;
using Service.IService;

namespace RongbanServeice
{
    public class PetStoreService:IPetStoreService
    {
        private readonly PetStoreDao _petStoreDao;

        public PetStoreService(PetStoreDao petStoreDao)
        {
            _petStoreDao = petStoreDao;
        }

        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<ProductCategoryDto>>> GetProduct()
        {
            try
            {
                var data = await _petStoreDao.GetProduct();
                var res = data.Select(r => new ProductCategoryDto
                {
                    Id = r.Id,
                    CategoryName = r.CategoryName,
                    ParentId = r.ParentId,
                    Sort = r.Sort
                }).ToList();


                return Response<List<ProductCategoryDto>>.Success(res, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<ProductCategoryDto>("查询发生错误", ex);
                return Response<List<ProductCategoryDto>>.Fail($"查询失败{ex}");
            }
        }

        /// <summary>
        /// 获取商品媒体资源（图片视频等）
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<ProductMedium>>> GetProductMedia()
        {
            try 
            {
                var res = await _petStoreDao.GetProductMedia();
                return Response<List<ProductMedium>>.Success(res, "查询成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error<PetStoreService>("查询发生错误", ex);
                return Response<List<ProductMedium>>.Fail($"查询失败{ex}");
            }
        }
        /// <summary>
        /// 获取商城商品
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<MallProductDto>>> GetMallProducts()
        {
            var data = await _petStoreDao.GetMallProducts();
            var res = data.Select(x => new MallProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                ApplicablePetType = x.ApplicablePetType,
                Price = x.Price,
                OriginalPrice = x.OriginalPrice,
                Stock = x.Stock,
                SalesVolume = x.SalesVolume,
                Specification = x.Specification,
                Description = x.Description,
                CoverUrl = x.CoverUrl,
                Status = x.Status,
            }).ToList();
            return Response<List<MallProductDto>>.Success(res, "查询成功");

        }

        /// <summary>
        /// 根据分类ID获取商城商品
        /// </summary>
        /// <param name="categoryId">要获取的分类的id</param>
        /// <returns></returns>
        public async Task<Response<List<MallProductDto>>> GetMallProductsByCategoryId(long categoryId)
        {
            var data = await _petStoreDao.GetMallProductsByCategoryId(categoryId);
            var res = data.Select(x => new MallProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                ApplicablePetType = x.ApplicablePetType,
                Price = x.Price,
                OriginalPrice = x.OriginalPrice,
                Stock = x.Stock,
                SalesVolume = x.SalesVolume,
                Specification = x.Specification,
                Description = x.Description,
                CoverUrl = x.CoverUrl,
                Status = x.Status,
            }).ToList();
            return Response<List<MallProductDto>>.Success(res, "查询成功");
        }

        /// <summary>
        /// 根据商品ID获取单个商品（遵循相同格式）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>包装后的Response对象</returns>
        public async Task<Response<MallProductDto>> GetMallProductById(long productId)
        {
            // 业务校验（与分类查询保持一致的校验风格）
            if (productId <= 0)
            {
                throw new ArgumentException("商品ID必须大于0", nameof(productId));
            }

            var data = await _petStoreDao.GetMallProductById(productId);
            var res = new MallProductDto
            {
                Id = data.Id,
                ProductName = data.ProductName,
                CategoryId = data.CategoryId,
                ApplicablePetType = data.ApplicablePetType,
                Price = data.Price,
                OriginalPrice = data.OriginalPrice,
                Stock = data.Stock,
                SalesVolume = data.SalesVolume,
                Specification = data.Specification,
                Description = data.Description,
                CoverUrl = data.CoverUrl,
                Status = data.Status,
            };
            return Response<MallProductDto>.Success(res, "查询成功");
        }

        #region 秒杀功能相关新增方法

        /// <summary>
        /// 执行商品秒杀（核心逻辑：校验秒杀条件 -> 扣减库存 -> 创建秒杀订单）
        /// </summary>
        /// <param name="userId">秒杀用户ID</param>
        /// <param name="productId">秒杀商品ID</param>
        /// <returns>秒杀结果响应（包含订单号或错误信息）</returns>
        public async Task<Response<string>> ExecuteSeckill(long userId, long productId)
        {
            // 1. 参数校验（与现有风格一致）
            if (userId <= 0)
            {
                throw new ArgumentException("用户ID必须大于0", nameof(userId));
            }
            if (productId <= 0)
            {
                throw new ArgumentException("商品ID必须大于0", nameof(productId));
            }

            try
            {
                // 2. 校验商品是否可参与秒杀（核心错误点：原方法调用的`GetSeckillProductAsync`不存在，需用现有方法替代）
                var mallProduct = await _petStoreDao.GetMallProductById(productId); // 使用现有DAL方法获取商品
                if (mallProduct == null)
                {
                    return new Response<string>(1, "秒杀商品不存在", null);
                }

                // 校验秒杀状态（假设实体已添加`SeckillStatus`等字段）
                if (mallProduct.SeckillStatus != 1) // 1-秒杀中
                {
                    return new Response<string>(1, "该商品未参与秒杀活动", null);
                }
                if (DateTime.Now < mallProduct.SeckillStartTime || DateTime.Now > mallProduct.SeckillEndTime)
                {
                    return new Response<string>(1, "秒杀活动未开始或已结束", null);
                }
                if (mallProduct.SeckillStock <= 0)
                {
                    return new Response<string>(1, "秒杀库存不足", null);
                }

                // 3. 校验用户限购（需新增方法：查询用户已秒杀数量）
                var userSeckillCount = await CheckUserSeckillLimit(userId, productId); // 下方实现该方法
                if (userSeckillCount >= mallProduct.SeckillLimit)
                {
                    return new Response<string>(1, $"超过限购数量（每人限{mallProduct.SeckillLimit}件）", null);
                }

                // 4. 乐观锁扣减秒杀库存（核心错误点：原`ReduceSeckillStockAsync`方法不存在，需补充DAL方法）
                var stockReduced = await _petStoreDao.ReduceSeckillStockAsync(productId, (int)mallProduct.SeckillVersion);
                if (!stockReduced)
                {
                    return new Response<string>(1, "秒杀失败，请重试", null);
                }

                // 5. 创建秒杀订单（假设存在订单Service方法）
                var orderNo = await CreateSeckillOrder(userId, mallProduct);
                if (string.IsNullOrEmpty(orderNo))
                {
                    // 订单创建失败，回滚库存（需DAL新增`RestoreSeckillStockAsync`方法）
                    await RestoreSeckillStockAsync(productId, (int)mallProduct.SeckillVersion);
                    return new Response<string>(1, "订单创建失败", null);
                }

                // 6. 返回成功结果
                return new Response<string>(200, "秒杀成功", orderNo);
            }
            catch (Exception ex)
            {
                // 保持原有异常处理风格
                return new Response<string>(-1, $"秒杀失败: {ex.Message}", null);
            }
        }

        // 新增：校验用户已秒杀数量（需订单DAL支持）
        public async Task<int> CheckUserSeckillLimit(long userId, long productId)
        {
            // 实际应查询订单表：用户在该商品的有效秒杀订单数  
            // 示例：return await _orderDao.CountUserSeckillOrdersAsync(userId, productId);
            return 0; // 临时默认值，需根据订单DAL实现
        }

        /// <summary>
        /// 恢复秒杀库存（用于订单取消等场景）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="currentVersion">当前秒杀版本号（用于乐观锁校验）</param>
        /// <returns>是否恢复成功</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Response<bool>> RestoreSeckillStockAsync(long productId, int seckillVersion)
        {
            try
            {
                // 调用DAL层方法（需传入商品ID和版本号）
                var success = await _petStoreDao.RestoreSeckillStockAsync(productId, seckillVersion);
                return new Response<bool>(200, "成功", success);
            }
            catch (Exception ex)
            {
                return new Response<bool>(-1, $"恢复秒杀库存失败: {ex.Message}", false);
            }
        }

        /// <summary>
        /// 创建秒杀订单（需结合订单Dao实现）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="product">秒杀商品信息</param>
        /// <returns>订单号</returns>
        public async Task<string> CreateSeckillOrder(long userId, MallProduct product)
        {
            // 1. 生成订单号（示例格式：SECKILL_时间戳_随机数）
            var orderNo = $"SECKILL_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 6)}";

            // 2. 调用订单Dao创建订单（假设存在OrderDao）
            // 示例：var orderId = await _orderDao.CreateSeckillOrderAsync(userId, product, orderNo);
            // if (orderId <= 0) return null;

            return orderNo; // 返回订单号
        }


        /// <summary>
        /// 获取所有秒杀中且库存充足的商品列表
        /// </summary>
        /// <returns>秒杀商品列表响应</returns>
        public async Task<Response<List<MallProduct>>> GetSeckillProducts()
        {
            try
            {
                // 调用DAL层方法查询符合条件的秒杀商品
                var seckillProducts = await _petStoreDao.GetSeckillProductsAsync();

                return new Response<List<MallProduct>>(200, "成功", seckillProducts);
            }
            catch (Exception ex)
            {
                return new Response<List<MallProduct>>(-1, $"获取秒杀商品列表失败: {ex.Message}", null);
            }
        }


        #endregion
    }
}
