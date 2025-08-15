using Common;
using RongbanDao.APP;
using RongbanServeice;
using Service.IService;

namespace Service.Service
{
    public class UserOnlineService: IUserOnlineService
    {
        private readonly UserOnlineDao _userOnlineDao;
        public UserOnlineService(UserOnlineDao userOnlineDao)
        {
            _userOnlineDao = userOnlineDao;
        }
        // 处理客户端心跳，更新最后活跃时间
        public async Task<Response<string>> UpdateUserActivityAsync(int userId, string deviceId, string ipAddress)
        {
            LogHelper.Info<UserOnlineService>($"开始处理用户{userId}的心跳");
            try
            {
                var res = await _userOnlineDao.UpdateUserActivityAsync(userId, deviceId, ipAddress);
                if (res > 0)
                {
                    LogHelper.Warn<UserOnlineService>($"更新最后活跃时间成功");
                    return Response<string>.SuccessWithoutData("更新成功");
                }
                LogHelper.Warn<UserOnlineService>($"更新最后活跃时间失败");
                return Response<string>.FailWithoutData("更新失败！");
            }
            catch (Exception ex) 
            { 
                LogHelper.Error<UserOnlineService>("更新发生错误",ex);
                return Response<string>.FailWithoutData("更新失败！");
            }
        }
        // 判断用户是否在线
        public async Task<Response<string>> IsUserOnlineAsync(int userId)
        {

            LogHelper.Info<UserOnlineService>($"开始判断用户{userId}的状态");
            try
            {
                var res = await _userOnlineDao.IsUserOnlineAsync(userId);
                if (res == null)
                {
                    LogHelper.Warn<UserOnlineService>($"查询用户状态失败");
                    return Response<string>.FailWithoutData("查询失败！");
                }
                if (res)
                {
                    LogHelper.Info<UserOnlineService>($"查询用户状态成功");
                    return Response<string>.SuccessWithoutData("查询成功,用户在线");
                }
                else
                {
                    LogHelper.Warn<UserOnlineService>($"查询用户状态成功");
                    return Response<string>.SuccessWithoutData("查询成功,用户离线");
                }
            }catch(Exception ex)
            {
                LogHelper.Error<UserOnlineService>("查询发生错误",ex);
                return Response<string>.FailWithoutData("查询失败！");
            }

        }
        // 获取用户详细状态（在线/离线/隐身+最后活跃时间）
        public async Task<Response<string>> GetUserStatusAsync(int userId)
        {
            LogHelper.Info<UserOnlineService>($"开始获取用户{userId}的详细状态");
            try
            {
                var res = await _userOnlineDao.GetUserStatusAsync(userId);
                if (!string.IsNullOrEmpty(res))
                {
                    LogHelper.Info<UserOnlineService>($"获取用户{userId}的详细状态成功");
                    return Response<string>.Success(res, "获取成功");
                }
                LogHelper.Warn<UserOnlineService>($"获取用户{userId}的详细状态失败");
                return Response<string>.FailWithoutData("获取失败！");
            }
            catch (Exception ex) 
            {
                LogHelper.Error<UserOnlineService>("获取发生错误",ex);
                return Response<string>.FailWithoutData("获取失败！");
            }
        }
    }
}
