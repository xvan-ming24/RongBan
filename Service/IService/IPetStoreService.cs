using Models.Dto.PetStore;
using Rongban.Models.Entities;

namespace Service.IService
{
    public interface IPetStoreService
    {
        Task<Response<List<ProductCategoryDto>>> GetProduct();
        Task<Response<List<ProductMedium>>> GetProductMedia();
        Task<Response<List<MallProductDto>>> GetMallProducts();
        Task<Response<List<MallProductDto>>> GetMallProductsByCategoryId(long categoryId);
        Task<Response<MallProductDto>> GetMallProductById(long productId);
        Task<Response<string>> ExecuteSeckill(long userId, long productId);
        Task<int> CheckUserSeckillLimit(long userId, long productId);
        Task<Response<bool>> RestoreSeckillStockAsync(long productId, int seckillVersion);
        Task<string> CreateSeckillOrder(long userId, MallProduct product);
        Task<Response<List<MallProduct>>> GetSeckillProducts();
    }
}
