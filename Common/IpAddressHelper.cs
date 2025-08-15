using Microsoft.AspNetCore.Http;
using System.Net;

public static class IpAddressHelper
{
    /// <summary>
    /// 可靠获取客户端IP地址（支持反向代理和负载均衡场景）
    /// </summary>
    public static string GetClientIpAddress(HttpContext context)
    {
        if (context == null)
            return string.Empty;

        // 1. 尝试从X-Forwarded-For头获取（适用于反向代理/负载均衡）
        var xForwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrEmpty(xForwardedFor))
        {
            // X-Forwarded-For可能包含多个IP，取第一个（客户端真实IP）
            return xForwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault()?.Trim() ?? string.Empty;
        }

        // 2. 尝试从X-Real-IP头获取（Nginx等代理常用）
        var xRealIp = context.Request.Headers["X-Real-IP"].ToString();
        if (!string.IsNullOrEmpty(xRealIp))
        {
            return xRealIp.Trim();
        }

        // 3. 直接从连接获取（适用于无代理的场景）
        if (context.Connection.RemoteIpAddress != null)
        {
            // 处理IPv6映射的IPv4地址（如::ffff:192.168.1.1）
            if (context.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                context.Connection.RemoteIpAddress = Dns.GetHostEntry(context.Connection.RemoteIpAddress).AddressList
                    .FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        return string.Empty;
    }
}
