using aspCart.Core.Domain.Catalog;
using aspCart.Infrastructure.EFModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategoryMapping> ProductCategoryMappings { get; set; }
        public DbSet<ProductImageMapping> ProductImageMappings { get; set; }
        public DbSet<ProductManufacturerMapping> ProductManufacturerMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Image>().ToTable("Image");
            builder.Entity<Manufacturer>().ToTable("Manufacturer");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<ProductCategoryMapping>().ToTable("ProductCategoryMapping");
            builder.Entity<ProductImageMapping>().ToTable("ProductImageMapping");
            builder.Entity<ProductManufacturerMapping>().ToTable("ProductManufacturerMapping");
        }
    }
}
