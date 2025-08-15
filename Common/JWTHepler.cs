using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YourProject.Utils;

/// <summary>
/// JWT工具类，负责令牌的生成与验证
/// </summary>
public class JwtUtils
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expireMinutes;

    /// <summary>
    /// 初始化JWT工具类
    /// </summary>
    /// <param name="configuration">配置项，需包含Jwt节点</param>
    public JwtUtils(IConfiguration configuration)
    {
        // 从配置文件读取JWT参数
        _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key未配置");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer未配置");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience未配置");
        _expireMinutes = int.TryParse(configuration["Jwt:ExpireMinutes"], out int minutes)
            ? minutes
            : 30*24*60; // 默认30分钟过期
    }

    /// <summary>
    /// 生成JWT令牌
    /// </summary>
    /// <param name="Account">账号</param>
    /// <param name="userId">用户ID</param>
    /// <param name="roles">用户角色列表</param>
    /// <returns>生成的JWT令牌字符串</returns>
    public string GenerateToken(string Account, long userId, params string[] roles)
    {
        // 创建声明集合
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Account),
            new Claim(ClaimTypes.HomePhone, Account),
            new Claim(ClaimTypes.Email, Account),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // 唯一标识
        };

        // 添加角色声明
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        var allClaims = claims.Concat(roleClaims);

        // 创建签名密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 生成令牌
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: allClaims,
            expires: DateTime.Now.AddMinutes(_expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenQqWxToken(long userId, params string[] roles)
    {
        // 创建声明集合
        var claims = new[]
        {
            
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // 唯一标识
        };

        // 添加角色声明
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        var allClaims = claims.Concat(roleClaims);

        // 创建签名密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 生成令牌
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: allClaims,
            expires: DateTime.Now.AddMinutes(_expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 验证JWT令牌并返回声明信息
    /// </summary>
    /// <param name="token">JWT令牌字符串</param>
    /// <returns>验证成功返回声明集合，失败返回null</returns>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            // 验证令牌
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // 禁用默认的300秒时钟偏差
            }, out _);

            return principal;
        }
        catch
        {
            // 令牌无效（过期、篡改等）
            return null;
        }
    }

    /// <summary>
    /// 从令牌中获取用户名
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>用户名或null</returns>
    public string? GetUsernameFromToken(string token)
    {
        var principal = ValidateToken(token);
        return principal?.FindFirst(ClaimTypes.Name)?.Value;
    }

    /// <summary>
    /// 从令牌中获取用户ID
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>用户ID或0</returns>
    public long GetUserIdFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (long.TryParse(principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long userId))
        {
            return userId;
        }
        return 0;
    }

    /// <summary>
    /// 检查令牌是否包含指定角色
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <param name="role">角色名称</param>
    /// <returns>是否包含该角色</returns>
    public bool HasRole(string token, string role)
    {
        var principal = ValidateToken(token);
        return principal?.IsInRole(role) ?? false;
    }
}
