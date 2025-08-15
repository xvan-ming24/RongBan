using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Rongban.Models.Entities;

public partial class PetPlatformDbContext : DbContext
{
    public PetPlatformDbContext()
    {
    }

    public PetPlatformDbContext(DbContextOptions<PetPlatformDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdoptionContract> AdoptionContracts { get; set; }

    public virtual DbSet<AdoptionInfo> AdoptionInfos { get; set; }

    public virtual DbSet<AiConsultRecord> AiConsultRecords { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<FosterService> FosterServices { get; set; }

    public virtual DbSet<HomeActivity> HomeActivities { get; set; }

    public virtual DbSet<HomeCarousel> HomeCarousels { get; set; }

    public virtual DbSet<HomeCity> HomeCities { get; set; }

    public virtual DbSet<HomeLive> HomeLives { get; set; }

    public virtual DbSet<MallProduct> MallProducts { get; set; }

    public virtual DbSet<MomentComment> MomentComments { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderMain> OrderMains { get; set; }

    public virtual DbSet<OrgInfo> OrgInfos { get; set; }

    public virtual DbSet<PetBeautyService> PetBeautyServices { get; set; }

    public virtual DbSet<PetCategory> PetCategories { get; set; }

    public virtual DbSet<PetInfo> PetInfos { get; set; }

    public virtual DbSet<PetInsurance> PetInsurances { get; set; }

    public virtual DbSet<PetMedicalService> PetMedicalServices { get; set; }

    public virtual DbSet<PetMedium> PetMedia { get; set; }

    public virtual DbSet<PetMoment> PetMoments { get; set; }

    public virtual DbSet<PointsExchange> PointsExchanges { get; set; }

    public virtual DbSet<PointsProduct> PointsProducts { get; set; }

    public virtual DbSet<PointsRecord> PointsRecords { get; set; }

    public virtual DbSet<PresenceType> PresenceTypes { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductMedium> ProductMedia { get; set; }

    public virtual DbSet<ReservationDetail> ReservationDetails { get; set; }

    public virtual DbSet<ReservationMain> ReservationMains { get; set; }

    public virtual DbSet<SequenceCounter> SequenceCounters { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<SysConfig> SysConfigs { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserFollow> UserFollows { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<UserLevel> UserLevels { get; set; }

    public virtual DbSet<UserPetRelation> UserPetRelations { get; set; }

    public virtual DbSet<UserPoint> UserPoints { get; set; }

    public virtual DbSet<UserPresenceRecord> UserPresenceRecords { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    public virtual DbSet<UserTaskRecord> UserTaskRecords { get; set; }

    public virtual DbSet<VerificationCode> VerificationCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=序安;database=pet_platform_db;uid=sa;pwd=123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdoptionContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__adoption__3213E83F62377931");

            entity.ToTable("adoption_contract");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdoptionId).HasColumnName("adoption_id");
            entity.Property(e => e.ContractContent).HasColumnName("contract_content");
            entity.Property(e => e.OrgId).HasColumnName("org_id");
            entity.Property(e => e.SignTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("sign_time");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Adoption).WithMany(p => p.AdoptionContracts)
                .HasForeignKey(d => d.AdoptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_contract_adoption");

            entity.HasOne(d => d.Org).WithMany(p => p.AdoptionContracts)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_contract_org");

            entity.HasOne(d => d.User).WithMany(p => p.AdoptionContracts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_contract_user");
        });

        modelBuilder.Entity<AdoptionInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__adoption__3213E83F425CCE5F");

            entity.ToTable("adoption_info", tb => tb.HasComment("领养信息表，存储宠物领养信息（个人或机构发布）"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdoptionRequirements).HasColumnName("adoption_requirements");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsContractRequired).HasColumnName("is_contract_required");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.PublisherType).HasColumnName("publisher_type");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Pet).WithMany(p => p.AdoptionInfos)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_adoption_pet");
        });

        modelBuilder.Entity<AiConsultRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ai_consu__3213E83FDA2D543C");

            entity.ToTable("ai_consult_record");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AiDiagnosis).HasColumnName("ai_diagnosis");
            entity.Property(e => e.ConsultationTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("consultation_time");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Symptoms).HasColumnName("symptoms");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pet).WithMany(p => p.AiConsultRecords)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ai_consult_pet");

            entity.HasOne(d => d.Service).WithMany(p => p.AiConsultRecords)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ai_consult_service");

            entity.HasOne(d => d.User).WithMany(p => p.AiConsultRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ai_consult_user");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cart_ite__3213E83F8C0B7936");

            entity.ToTable("cart_item", tb => tb.HasComment("购物车商品项表，记录购物车中的具体商品及数量等信息"));

            entity.HasIndex(e => e.CartId, "IX_cart_item_cart_id");

            entity.HasIndex(e => new { e.CartId, e.ProductId }, "UK_cart_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsSelected)
                .HasDefaultValue((byte)1)
                .HasColumnName("is_selected");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(128)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_price");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_cart_item_cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cart_item_product");
        });

        modelBuilder.Entity<FosterService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__foster_s__3213E83F84F402C0");

            entity.ToTable("foster_service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicablePetType)
                .HasMaxLength(128)
                .HasColumnName("applicable_pet_type");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrgId).HasColumnName("org_id");
            entity.Property(e => e.PricePerDay)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price_per_day");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(128)
                .HasColumnName("service_name");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Org).WithMany(p => p.FosterServices)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_foster_org");
        });

        modelBuilder.Entity<HomeActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__home_act__3213E83F72C773E8");

            entity.ToTable("home_activity", tb => tb.HasComment("活动信息表，存储平台举办的各类猫咪相关活动"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(128)
                .HasColumnName("activity_name");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CoverUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("cover_url");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");

            entity.HasOne(d => d.City).WithMany(p => p.HomeActivities)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_home_activity_city");
        });

        modelBuilder.Entity<HomeCarousel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__home_car__3214EC073936EC91");

            entity.ToTable("home_carousel");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.LinkUrl).HasMaxLength(255);
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<HomeCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__home_cit__3213E83FC5682CC6");

            entity.ToTable("home_city", tb => tb.HasComment("城市信息表，存储平台支持的城市数据"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("city_name");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsDaily)
                .HasDefaultValue((byte)0)
                .HasColumnName("is_daily");
            entity.Property(e => e.IsHot)
                .HasDefaultValue((byte)0)
                .HasColumnName("is_hot");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");
        });

        modelBuilder.Entity<HomeLive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__home_liv__3213E83FB7AD469E");

            entity.ToTable("home_live", tb => tb.HasComment("直播信息表，存储猫咪相关直播内容"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.HostId).HasColumnName("host_id");
            entity.Property(e => e.HostName)
                .HasMaxLength(64)
                .HasColumnName("host_name");
            entity.Property(e => e.LiveTitle)
                .HasMaxLength(128)
                .HasColumnName("live_title");
            entity.Property(e => e.LiveUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("live_url");
            entity.Property(e => e.OnlineNum)
                .HasDefaultValue(0)
                .HasColumnName("online_num");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)0)
                .HasColumnName("status");

            entity.HasOne(d => d.Host).WithMany(p => p.HomeLives)
                .HasForeignKey(d => d.HostId)
                .HasConstraintName("FK_home_live_host");
        });

        modelBuilder.Entity<MallProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mall_pro__3213E83FF2151AC6");

            entity.ToTable("mall_product", tb => tb.HasComment("商品信息表，存储全品类宠物用品的信息"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicablePetType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("applicable_pet_type");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CoverUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("cover_url");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OriginalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("original_price");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(128)
                .HasColumnName("product_name");
            entity.Property(e => e.SalesVolume)
                .HasDefaultValue(0)
                .HasColumnName("sales_volume");
            entity.Property(e => e.SeckillEndTime).HasColumnName("seckill_end_time");
            entity.Property(e => e.SeckillLimit)
                .HasDefaultValue(1)
                .HasColumnName("seckill_limit");
            entity.Property(e => e.SeckillPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("seckill_price");
            entity.Property(e => e.SeckillStartTime).HasColumnName("seckill_start_time");
            entity.Property(e => e.SeckillStatus)
                .HasDefaultValue((byte)0)
                .HasColumnName("seckill_status");
            entity.Property(e => e.SeckillStock)
                .HasDefaultValue(0)
                .HasColumnName("seckill_stock");
            entity.Property(e => e.SeckillVersion)
                .HasDefaultValue(0)
                .HasColumnName("seckill_version");
            entity.Property(e => e.Specification)
                .HasMaxLength(255)
                .HasColumnName("specification");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Category).WithMany(p => p.MallProducts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mall_product_category");
        });

        modelBuilder.Entity<MomentComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__moment_c__3213E83F4984F3F2");

            entity.ToTable("moment_comment", tb => tb.HasComment("宠友圈评论表，记录用户对动态的评论"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(512)
                .HasColumnName("content");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.MomentId).HasColumnName("moment_id");
            entity.Property(e => e.ParentId)
                .HasDefaultValue(0L)
                .HasColumnName("parent_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Moment).WithMany(p => p.MomentComments)
                .HasForeignKey(d => d.MomentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comment_moment");

            entity.HasOne(d => d.User).WithMany(p => p.MomentComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comment_user");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_it__3213E83F1379F62C");

            entity.ToTable("order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(128)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_order_item_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_item_product");
        });

        modelBuilder.Entity<OrderMain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_ma__3213E83F1AE0D1DD");

            entity.ToTable("order_main", tb => tb.HasComment("订单主表，存储订单整体信息"));

            entity.HasIndex(e => e.OrderNo, "UQ__order_ma__465C81B87E7448E3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsSeckill)
                .HasDefaultValue(false)
                .HasColumnName("is_seckill");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("order_no");
            entity.Property(e => e.PayStatus)
                .HasDefaultValue((byte)0)
                .HasColumnName("pay_status");
            entity.Property(e => e.PayTime).HasColumnName("pay_time");
            entity.Property(e => e.ReceiverAddress)
                .HasMaxLength(512)
                .HasColumnName("receiver_address");
            entity.Property(e => e.ReceiverName)
                .HasMaxLength(64)
                .HasColumnName("receiver_name");
            entity.Property(e => e.ReceiverPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("receiver_phone");
            entity.Property(e => e.SeckillPayTimeout)
                .HasDefaultValue(10)
                .HasColumnName("seckill_pay_timeout");
            entity.Property(e => e.SeckillStockRecovered)
                .HasDefaultValue(false)
                .HasColumnName("seckill_stock_recovered");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)0)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.OrderMains)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_user");
        });

        modelBuilder.Entity<OrgInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__org_info__3213E83FC2D3027F");

            entity.ToTable("org_info", tb => tb.HasComment("机构信息表，存储宠物店、宠物医院等机构信息"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(64)
                .HasColumnName("contact_person");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("latitude");
            entity.Property(e => e.LicenseUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("license_url");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("longitude");
            entity.Property(e => e.OpeningHours)
                .HasMaxLength(128)
                .HasColumnName("opening_hours");
            entity.Property(e => e.OrgName)
                .HasMaxLength(128)
                .HasColumnName("org_name");
            entity.Property(e => e.OrgType).HasColumnName("org_type");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.City).WithMany(p => p.OrgInfos)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_org_city");
        });

        modelBuilder.Entity<PetBeautyService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_beau__3213E83FE54D956F");

            entity.ToTable("pet_beauty_service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicablePetType)
                .HasMaxLength(128)
                .HasColumnName("applicable_pet_type");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.OrgId).HasColumnName("org_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(128)
                .HasColumnName("service_name");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Org).WithMany(p => p.PetBeautyServices)
                .HasForeignKey(d => d.OrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_beauty_org");
        });

        modelBuilder.Entity<PetCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_cate__3213E83FC0DED171");

            entity.ToTable("pet_category", tb => tb.HasComment("宠物分类表，存储全品类宠物的品种分类（猫、狗、鸟等）"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(64)
                .HasColumnName("category_name");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsHot)
                .HasDefaultValue((byte)0)
                .HasColumnName("is_hot");
            entity.Property(e => e.ParentId)
                .HasDefaultValue(0L)
                .HasColumnName("parent_id");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");
        });

        modelBuilder.Entity<PetInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_info__3213E83F058D8E75");

            entity.ToTable("pet_info", tb => tb.HasComment("宠物信息表，存储全品类宠物的详细信息"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Breed)
                .HasMaxLength(64)
                .HasColumnName("breed");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Characteristic).HasColumnName("characteristic");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Gender)
                .HasDefaultValue((byte)0)
                .HasColumnName("gender");
            entity.Property(e => e.PetName)
                .HasMaxLength(64)
                .HasColumnName("pet_name");
            entity.Property(e => e.Sterilization)
                .HasDefaultValue((byte)0)
                .HasColumnName("sterilization");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.Vaccine)
                .HasMaxLength(255)
                .HasColumnName("vaccine");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Category).WithMany(p => p.PetInfos)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pet_info_category");
        });

        modelBuilder.Entity<PetInsurance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_insu__3213E83F6022185D");

            entity.ToTable("pet_insurance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coverage).HasColumnName("coverage");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.InsuranceName)
                .HasMaxLength(128)
                .HasColumnName("insurance_name");
            entity.Property(e => e.Premium)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("premium");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Service).WithMany(p => p.PetInsurances)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_insurance_service");
        });

        modelBuilder.Entity<PetMedicalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_medi__3213E83FD064C52F");

            entity.ToTable("pet_medical_service", tb => tb.HasComment("宠物医疗服务表，包含四大医疗服务板块"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrgId).HasColumnName("org_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(128)
                .HasColumnName("service_name");
            entity.Property(e => e.ServiceType).HasColumnName("service_type");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Org).WithMany(p => p.PetMedicalServices)
                .HasForeignKey(d => d.OrgId)
                .HasConstraintName("FK_medical_org");
        });

        modelBuilder.Entity<PetMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_medi__3213E83FBD36F221");

            entity.ToTable("pet_media", tb => tb.HasComment("宠物多媒体表，存储宠物的图片和视频资源"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsCover)
                .HasDefaultValue((byte)0)
                .HasColumnName("is_cover");
            entity.Property(e => e.MediaType).HasColumnName("media_type");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("media_url");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetMedia)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pet_media_pet");
        });

        modelBuilder.Entity<PetMoment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_mome__3213E83F73E5D995");

            entity.ToTable("pet_moment", tb => tb.HasComment("宠友圈动态表，存储用户发布的全品类宠物相关动态"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentCount)
                .HasDefaultValue(0L)
                .HasColumnName("comment_count");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.ImageUrls)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("image_urls");
            entity.Property(e => e.LikeCount)
                .HasDefaultValue(0L)
                .HasColumnName("like_count");
            entity.Property(e => e.ShareCount)
                .HasDefaultValue(0L)
                .HasColumnName("share_count");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PetMoments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_moment_user");
        });

        modelBuilder.Entity<PointsExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__points_e__3213E83F531BD468");

            entity.ToTable("points_exchange");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExchangeTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("exchange_time");
            entity.Property(e => e.PointsUsed).HasColumnName("points_used");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.PointsExchanges)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_exchange_product");

            entity.HasOne(d => d.User).WithMany(p => p.PointsExchanges)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_exchange_user");
        });

        modelBuilder.Entity<PointsProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__points_p__3213E83FCF8FDC49");

            entity.ToTable("points_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CoverUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("cover_url");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.PointsRequired).HasColumnName("points_required");
            entity.Property(e => e.ProductName)
                .HasMaxLength(128)
                .HasColumnName("product_name");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<PointsRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__points_r__3213E83F693EB559");

            entity.ToTable("points_record");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.SourceType).HasColumnName("source_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PointsRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_points_record_user");
        });

        modelBuilder.Entity<PresenceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__presence__3213E83FBD54F467");

            entity.ToTable("presence_type");

            entity.HasIndex(e => e.PresenceName, "UQ__presence__C33E964029620DAE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.PresenceColor)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("presence_color");
            entity.Property(e => e.PresenceName)
                .HasMaxLength(20)
                .HasColumnName("presence_name");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue((byte)0)
                .HasColumnName("sort_order");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83F5527E234");

            entity.ToTable("product_category", tb => tb.HasComment("商品分类表，存储全品类宠物用品的分类信息"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(64)
                .HasColumnName("category_name");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.ParentId)
                .HasDefaultValue(0L)
                .HasColumnName("parent_id");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<ProductMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83F4AD7A587");

            entity.ToTable("product_media", tb => tb.HasComment("商品媒体表，存储商品的图片和视频资源"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.MediaType).HasColumnName("media_type");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("media_url");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Sort)
                .HasDefaultValue(0)
                .HasColumnName("sort");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductMedia)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_product_media_product");
        });

        modelBuilder.Entity<ReservationDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reservat__3213E83F696D2804");

            entity.ToTable("reservation_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Notes)
                .HasMaxLength(512)
                .HasColumnName("notes");
            entity.Property(e => e.PaymentStatus)
                .HasDefaultValue((byte)0)
                .HasColumnName("payment_status");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.ServiceFee)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("service_fee");

            entity.HasOne(d => d.Pet).WithMany(p => p.ReservationDetails)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detail_pet");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservationDetails)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_detail_reservation");
        });

        modelBuilder.Entity<ReservationMain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reservat__3213E83F7ABFDA3D");

            entity.ToTable("reservation_main", tb => tb.HasComment("预约主表，统一管理医疗、寄养、美容等各类预约"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.RelatedServiceId).HasColumnName("related_service_id");
            entity.Property(e => e.ReservationDate).HasColumnName("reservation_date");
            entity.Property(e => e.ReservationTime).HasColumnName("reservation_time");
            entity.Property(e => e.ReservationType).HasColumnName("reservation_type");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ReservationMains)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reservation_user");
        });

        modelBuilder.Entity<SequenceCounter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sequence__3214EC07F680E341");

            entity.ToTable("SequenceCounter");

            entity.HasIndex(e => e.SequenceName, "UQ__Sequence__8CE6B41FB93CEADD").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.SequenceName).HasMaxLength(50);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shopping__3213E83FCF8FC124");

            entity.ToTable("shopping_cart", tb => tb.HasComment("购物车主表，关联用户与购物车，记录购物车整体信息"));

            entity.HasIndex(e => e.UserId, "UK_user_cart").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.TotalCount)
                .HasDefaultValue(0)
                .HasColumnName("total_count");
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart)
                .HasForeignKey<ShoppingCart>(d => d.UserId)
                .HasConstraintName("FK_shopping_cart_user");
        });

        modelBuilder.Entity<SysConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sys_conf__3213E83F8FBDD111");

            entity.ToTable("sys_config", tb => tb.HasComment("系统配置表，存储平台各类配置信息"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConfigKey)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("config_key");
            entity.Property(e => e.ConfigValue).HasColumnName("config_value");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("remark");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_cre__3213E83FADB28B63");

            entity.ToTable("user_credentials");

            entity.HasIndex(e => new { e.CredentialType, e.CredentialValue }, "UQ__user_cre__EAA512BC3CADECDD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("access_token");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.CredentialType).HasColumnName("credential_type");
            entity.Property(e => e.CredentialValue)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("credential_value");
            entity.Property(e => e.ExpiresIn).HasColumnName("expires_in");
            entity.Property(e => e.OpenId)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("open_id");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("refresh_token");
            entity.Property(e => e.UnionId)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("union_id");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserCredentials)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__user_cred__user___59063A47");
        });

        modelBuilder.Entity<UserFollow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_fol__3213E83F288F0863");

            entity.ToTable("user_follow", tb => tb.HasComment("用户关注关系表，记录用户间的关注关系"));

            entity.HasIndex(e => new { e.FollowerId, e.FollowedId }, "UK_follow_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.FollowedId).HasColumnName("followed_id");
            entity.Property(e => e.FollowerId).HasColumnName("follower_id");

            entity.HasOne(d => d.Followed).WithMany(p => p.UserFollowFolloweds)
                .HasForeignKey(d => d.FollowedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_follow_followed");

            entity.HasOne(d => d.Follower).WithMany(p => p.UserFollowFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_follow_follower");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_inf__3213E83FC8A98B56");

            entity.ToTable("user_info", tb => tb.HasComment("用户信息表，存储平台用户的基本信息，使用手机号或邮箱登录"));

            entity.HasIndex(e => e.Email, "IX_user_info_email")
                .IsUnique()
                .HasFilter("([email] IS NOT NULL)");

            entity.HasIndex(e => e.Phone, "IX_user_info_phone")
                .IsUnique()
                .HasFilter("([phone] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("avatar_url");
            entity.Property(e => e.BannedStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("banned_status");
            entity.Property(e => e.Bio)
                .HasMaxLength(255)
                .HasColumnName("bio");
            entity.Property(e => e.City)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasDefaultValue((byte)0)
                .HasColumnName("gender");
            entity.Property(e => e.LastLoginTime).HasColumnName("last_login_time");
            entity.Property(e => e.Nickname)
                .HasMaxLength(64)
                .HasColumnName("nickname");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RegisterTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("register_time");
        });

        modelBuilder.Entity<UserLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_lev__3213E83FB9C0CE5D");

            entity.ToTable("user_level");

            entity.HasIndex(e => e.UserId, "UQ_user_level").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CutenessValue).HasColumnName("cuteness_value");
            entity.Property(e => e.Level)
                .HasDefaultValue((byte)1)
                .HasColumnName("level");
            entity.Property(e => e.NextLevelRequired)
                .HasDefaultValue(100)
                .HasColumnName("next_level_required");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.UserLevel)
                .HasForeignKey<UserLevel>(d => d.UserId)
                .HasConstraintName("FK_user_level_user");
        });

        modelBuilder.Entity<UserPetRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_pet__3213E83F33A195A9");

            entity.ToTable("user_pet_relation", tb => tb.HasComment("用户与宠物的关联表，记录用户所拥有或关联的宠物"));

            entity.HasIndex(e => new { e.UserId, e.PetId }, "UK_user_pet_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.IsOwner)
                .HasDefaultValue((byte)1)
                .HasColumnName("is_owner");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.RelationType)
                .HasMaxLength(20)
                .HasDefaultValue("主人")
                .HasColumnName("relation_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pet).WithMany(p => p.UserPetRelations)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK_relation_pet");

            entity.HasOne(d => d.User).WithMany(p => p.UserPetRelations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_relation_user");
        });

        modelBuilder.Entity<UserPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_poi__3213E83F7A5A0F59");

            entity.ToTable("user_points");

            entity.HasIndex(e => e.UserId, "UQ_user_points").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContinuousCheckinDays)
                .HasDefaultValue(0)
                .HasColumnName("continuous_checkin_days");
            entity.Property(e => e.LastCheckinDate).HasColumnName("last_checkin_date");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.TotalCheckinDays)
                .HasDefaultValue(0)
                .HasColumnName("total_checkin_days");
            entity.Property(e => e.TotalPoints).HasColumnName("total_points");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.UserPoint)
                .HasForeignKey<UserPoint>(d => d.UserId)
                .HasConstraintName("FK_user_points_user");
        });

        modelBuilder.Entity<UserPresenceRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_pre__3213E83F2EE19BC9");

            entity.ToTable("user_presence_record");

            entity.HasIndex(e => e.LastActiveTime, "IX_user_presence_record_last_active");

            entity.HasIndex(e => e.UserId, "IX_user_presence_record_user_id");

            entity.HasIndex(e => new { e.UserId, e.DeviceId }, "UK_user_device").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("device_id");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(50)
                .HasColumnName("device_name");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ip_address");
            entity.Property(e => e.LastActiveTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("last_active_time");
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("login_time");
            entity.Property(e => e.PresenceId).HasColumnName("presence_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Presence).WithMany(p => p.UserPresenceRecords)
                .HasForeignKey(d => d.PresenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_presence_record_presence_type");

            entity.HasOne(d => d.User).WithMany(p => p.UserPresenceRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_user_presence_record_user_info");
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_tas__3213E83F4A0446B3");

            entity.ToTable("user_task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_time");
            entity.Property(e => e.MaxCompleteTimes)
                .HasDefaultValue(1)
                .HasColumnName("max_complete_times");
            entity.Property(e => e.PointsReward).HasColumnName("points_reward");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.TaskName)
                .HasMaxLength(128)
                .HasColumnName("task_name");
            entity.Property(e => e.TaskType).HasColumnName("task_type");
        });

        modelBuilder.Entity<UserTaskRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_tas__3213E83F682CFF1D");

            entity.ToTable("user_task_record");

            entity.HasIndex(e => new { e.UserId, e.TaskId }, "UQ_user_task").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompleteTimes)
                .HasDefaultValue(0)
                .HasColumnName("complete_times");
            entity.Property(e => e.LastCompleteTime).HasColumnName("last_complete_time");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Task).WithMany(p => p.UserTaskRecords)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_record_task");

            entity.HasOne(d => d.User).WithMany(p => p.UserTaskRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_task_record_user");
        });

        modelBuilder.Entity<VerificationCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__verifica__3213E83FB38CDD92");

            entity.ToTable("verification_codes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CodeType).HasColumnName("code_type");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.ExpireTime).HasColumnName("expire_time");
            entity.Property(e => e.IsUsed)
                .HasDefaultValue(false)
                .HasColumnName("is_used");
            entity.Property(e => e.Target)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("target");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
