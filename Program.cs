using AuthSystem.DAL.Repositories;

using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rongban.Models.Entities;
using RongbanDao.APP;
using RongbanDao;
using RongbanServeice;
using Service.IService;
using Service.Service;
using System.Text;
using YourProject.Utilities;
using YourProject.Utils;
using System.Reflection;
using Common;
using Dao.APP.UserLogin;
using Dao.APP.Home;
using Dao.APP.PetStore;
using Dao.APP.PetCircel;
using ShoppingCartApi.BLL;
using Rongban.BLL.Services;
using Dao.APP.Personal;
using Dao.APP.PetCircle;
using PetApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 配置静态文件访问
builder.Services.AddStaticFiles();

// 添加HttpClient
builder.Services.AddHttpClient();

// 添加控制器服务
builder.Services.AddControllers();

// 添加缓存服务
builder.Services.AddDistributedMemoryCache();

// 注册HttpContext访问器
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Common.SessionHelper>();
//builder.Services.AddScoped<Service.LoginService>();


#region IOC 依赖注入容器
//注入仓储
/*默认有的部分*/

builder.Services.AddScoped<PetPlatformDbContext>();
builder.Services.AddScoped<JwtUtils>();
builder.Services.AddScoped<PetInfo>();
builder.Services.AddScoped<UserInfo>();
builder.Services.AddScoped<PetCategory>();
builder.Services.AddScoped<PetMedium>();
builder.Services.AddScoped<PetMoment>();
builder.Services.AddScoped<HomeActivity>();
builder.Services.AddScoped<HomeCity>();
builder.Services.AddScoped<MallProduct>();
builder.Services.AddScoped<MomentComment>();
builder.Services.AddScoped<ProductCategory>();
builder.Services.AddScoped<ProductMedium>();
builder.Services.AddScoped<UserFollow>();
builder.Services.AddScoped<MallProduct>();
builder.Services.AddScoped<HomeLive>();



/*自行添加*/
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<LoginDao>();
builder.Services.AddScoped<RegisterDao>();
builder.Services.AddScoped<UserOnlineService>();
builder.Services.AddScoped<IUserOnlineService, UserOnlineService>();
builder.Services.AddScoped<LogOutDao>();
builder.Services.AddScoped<AuthDao>();
builder.Services.AddScoped<UserOnlineDao>();
builder.Services.AddScoped<FollowDao>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<HomePageDao>();
builder.Services.AddScoped<HomeCarouselService>();
builder.Services.AddScoped<IHomeCarouselService, HomeCarouselService>();
builder.Services.AddScoped<PetStoreDao>();
builder.Services.AddScoped<PetStoreService>();
builder.Services.AddScoped<IPetStoreService, PetStoreService>();
builder.Services.AddScoped<SequenceGeneratorHeplper>();
builder.Services.AddScoped<PetCircleDao>();
builder.Services.AddScoped<PetCircleService>();
builder.Services.AddScoped<IPetCircleService, PetCircleService>();
builder.Services.AddScoped<CartItemRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ShoppingCartRepository>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<PersonalInfoDao>();
builder.Services.AddScoped<PersonalInfoService>();
builder.Services.AddScoped<IPersonalInfoService,PersonalInfoService>();
builder.Services.AddScoped<PersonalPetInfoDao>();
builder.Services.AddScoped<PersonalPetInfoService>();
builder.Services.AddScoped<IPersonalPetInfoService, PersonalPetInfoService>();
builder.Services.AddScoped<BatchUploadImages>();
builder.Services.AddScoped<BatchUploadImagesService>();
builder.Services.AddScoped<AdoptionPetDao>();
builder.Services.AddScoped<HomeVajraService>();
builder.Services.AddScoped<IHomeVajraService,HomeVajraService>();
builder.Services.AddScoped<FosterOrgDao>();
builder.Services.AddScoped<UserPointsDao>();
builder.Services.AddScoped<AiService>();



#endregion

#region Session服务
// 添加 Session 服务
builder.Services.AddSession(options =>
{
    // 配置 Session 选项
    options.Cookie.HttpOnly = true;       // 防止客户端脚本访问 Cookie
    options.Cookie.IsEssential = true;    // 标记为必要 Cookie（符合 GDPR）
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 会话超时时间
});
#endregion

#region log4net 日志配置
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
#endregion

#region 系统缓存服务开启
builder.Services.AddMemoryCache();
#endregion

#region  添加 Swagger 服务
builder.Services.AddSwaggerGen(options =>
{
    // 配置 Swagger 文档信息
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "RongbanAPI",
        Description = "茸伴系统api文档",

    });

    // 如果有 XML 注释文件，可以配置让 Swagger 显示注释
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // 配置 Swagger 支持 JWT 认证
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "请输入令牌，格式为：Bearer {令牌}",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

#endregion

#region JWT认证配置
// 添加控制器服务
builder.Services.AddControllers();

// 配置 JWT 认证服务
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // 验证发行人
        ValidateIssuer = true,
        // 验证受众
        ValidateAudience = true,
        // 验证过期时间
        ValidateLifetime = true,
        // 验证签名
        ValidateIssuerSigningKey = true,
        // 发行人（应从配置文件读取）
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // 受众（应从配置文件读取）
        ValidAudience = builder.Configuration["Jwt:Audience"],
        // 签名密钥（应从配置文件读取，生产环境中使用强密钥）
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


#endregion 

//添加 CORS 策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
// 配置 Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RongbanAPI V1");
    // 可选：设置 Swagger UI 为应用的默认页面
    //options.RoutePrefix = string.Empty;
});


// 配置静态文件访问
app.UseStaticFiles(); // 默认访问wwwroot文件夹

// 配置图片存储文件夹作为静态文件目录
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images" // 访问路径
});

// 启用 Session 中间件
app.UseSession();
//配置 CORS 中间件
app.UseCors("AllowAll");
app.UseHttpsRedirection();

// 启用认证中间件
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
