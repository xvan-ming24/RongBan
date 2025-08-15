
namespace Service.IService
{
    public interface IUserOnlineService
    {
        Task<Response<string>> UpdateUserActivityAsync(int userId, string deviceId, string ipAddress);
        Task<Response<string>> IsUserOnlineAsync(int userId);
        Task<Response<string>> GetUserStatusAsync(int userId);
    }
}
