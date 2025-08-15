using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    /// <summary>
    /// 静态文件扩展方法
    /// </summary>
    public static class StaticFilesExtensions
    {
        /// <summary>
        /// 配置静态文件服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStaticFiles(this IServiceCollection services)
        {

            return services;
        }
    }
}
