using EcommerceWebApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<UserShoppingCart> UserShoppingCarts { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<TempStock> TempStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region 商品資料
            builder.Entity<Product>()
                .HasMany(x => x.ProductDetails)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .Property(x => x.OriginalPrice)
                .HasColumnType("decimal(18,5)");

            builder.Entity<ProductDetail>()
                .Property(x => x.OriginalPrice)
                .HasColumnType("decimal(18,5)");
            #endregion

            #region 購物車資料
            builder.Entity<UserShoppingCart>()
                .HasKey(k => new { k.AppUserId, k.ProductDetailId });

            builder.Entity<UserShoppingCart>()
                .HasOne(x => x.AppUser)
                .WithMany(x => x.UserShoppingCarts)
                .HasForeignKey(f => f.AppUserId);

            builder.Entity<UserShoppingCart>()
                .HasOne(x => x.ProductDetail)
                .WithMany(x => x.UserShoppingCarts)
                .HasForeignKey(f => f.ProductDetailId);
            #endregion

            #region 商品庫存
            builder.Entity<ProductDetail>()
                .HasOne(x => x.ProductStock)
                .WithOne(x => x.ProductDetail)
                .HasForeignKey<ProductStock>(f => f.ProductDetailId)
                .IsRequired();
            #endregion

            #region 訂單主表
            builder.Entity<AppUser>()
                .HasMany(x => x.OrderMasters)
                .WithOne(x => x.AppUser)
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderMaster>()
                .HasMany(x => x.OrderDetails)
                .WithOne(x => x.OrderMaster)
                .HasForeignKey(k => k.OrderMasterId);

            builder.Entity<OrderMaster>()
                .Property(x => x.OrderTotal)
                .HasColumnType("decimal(18,5)");

            builder.Entity<OrderMaster>()
                .Property(x => x.Discount)
                .HasColumnType("decimal(18,5)");

            builder.Entity<OrderMaster>()
                .Property(x => x.SubTotal)
                .HasColumnType("decimal(18,5)");
            #endregion
            #region 訂單明細
            builder.Entity<Product>()
                .HasMany(x => x.OrderDetails)
                .WithOne(x => x.Product);

            builder.Entity<OrderDetail>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,5)");
            #endregion
        }
    }
}
