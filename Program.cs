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


// ���þ�̬�ļ�����
builder.Services.AddStaticFiles();

// ���HttpClient
builder.Services.AddHttpClient();

// ��ӿ���������
builder.Services.AddControllers();

// ��ӻ������
builder.Services.AddDistributedMemoryCache();

// ע��HttpContext������
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Common.SessionHelper>();
//builder.Services.AddScoped<Service.LoginService>();


#region IOC ����ע������
//ע��ִ�
/*Ĭ���еĲ���*/

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



/*�������*/
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

#region Session����
// ��� Session ����
builder.Services.AddSession(options =>
{
    // ���� Session ѡ��
    options.Cookie.HttpOnly = true;       // ��ֹ�ͻ��˽ű����� Cookie
    options.Cookie.IsEssential = true;    // ���Ϊ��Ҫ Cookie������ GDPR��
    options.IdleTimeout = TimeSpan.FromMinutes(30); // �Ự��ʱʱ��
});
#endregion

#region log4net ��־����
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
#endregion

#region ϵͳ���������
builder.Services.AddMemoryCache();
#endregion

#region  ��� Swagger ����
builder.Services.AddSwaggerGen(options =>
{
    // ���� Swagger �ĵ���Ϣ
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "RongbanAPI",
        Description = "�װ�ϵͳapi�ĵ�",

    });

    // ����� XML ע���ļ������������� Swagger ��ʾע��
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // ���� Swagger ֧�� JWT ��֤
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "���������ƣ���ʽΪ��Bearer {����}",
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

#region JWT��֤����
// ��ӿ���������
builder.Services.AddControllers();

// ���� JWT ��֤����
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ��֤������
        ValidateIssuer = true,
        // ��֤����
        ValidateAudience = true,
        // ��֤����ʱ��
        ValidateLifetime = true,
        // ��֤ǩ��
        ValidateIssuerSigningKey = true,
        // �����ˣ�Ӧ�������ļ���ȡ��
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // ���ڣ�Ӧ�������ļ���ȡ��
        ValidAudience = builder.Configuration["Jwt:Audience"],
        // ǩ����Կ��Ӧ�������ļ���ȡ������������ʹ��ǿ��Կ��
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


#endregion 

//��� CORS ����
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
// ���� Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RongbanAPI V1");
    // ��ѡ������ Swagger UI ΪӦ�õ�Ĭ��ҳ��
    //options.RoutePrefix = string.Empty;
});


// ���þ�̬�ļ�����
app.UseStaticFiles(); // Ĭ�Ϸ���wwwroot�ļ���

// ����ͼƬ�洢�ļ�����Ϊ��̬�ļ�Ŀ¼
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images" // ����·��
});

// ���� Session �м��
app.UseSession();
//���� CORS �м��
app.UseCors("AllowAll");
app.UseHttpsRedirection();

// ������֤�м��
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
