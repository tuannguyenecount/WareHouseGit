namespace Warehouse.Data.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using global::Warehouse.Entities;

    public partial class WarehouseContext : DbContext
    {
        public WarehouseContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<historyBankCharging> historyBankChargings { get; set; }
        public virtual DbSet<InfoShop> InfoShops { get; set; }
        public virtual DbSet<MailBox> MailBoxes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Property_Product> Property_Product { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ImagesProduct> ImagesProducts { get; set; }
        public virtual DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }
        public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual DbSet<ArticleTranslation> ArticleTranslations { get; set; }
        public virtual DbSet<BlogTranslation> BlogTranslations { get; set; }
        public virtual DbSet<SlideTranslation> SlideTranslations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Category>()
                .Property(e => e.Alias_SEO)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Category1)
                .WithOptional(e => e.Category2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Wards)
                .WithRequired(e => e.District)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<historyBankCharging>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<InfoShop>()
                .Property(e => e.Logo)
                .IsUnicode(false);

            modelBuilder.Entity<InfoShop>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<InfoShop>()
                .Property(e => e.Zalo)
                .IsUnicode(false);

            modelBuilder.Entity<InfoShop>()
                .Property(e => e.GoogleAnalytics)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalMoney)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductImage)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductAlias)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Money)
                .HasPrecision(19, 4);


            modelBuilder.Entity<Product>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Alias_SEO)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Property_Product)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Property>()
                .HasMany(e => e.Property_Product)
                .WithRequired(e => e.Property)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Province>()
                .HasMany(e => e.Districts)
                .WithRequired(e => e.Province)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Subscriber>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTranslation>()
                .Property(e => e.Alias_SEO)
                .IsUnicode(false);
        }
    }
}
