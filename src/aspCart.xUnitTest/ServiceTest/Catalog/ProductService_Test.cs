using aspCart.Core.Domain.Catalog;
using aspCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace aspCart.xUnitTest.ServiceTest.Catalog
{
    public class ProductService_Test
    {
        [Fact]
        public void ProductService_Test_GetAllProducts()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_GetAllProduct")
                .Options;

            var productEntities = new List<Product>
            {
                new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m },
                new Product() { Id = Guid.NewGuid(), Name = "Product 2", Price = 100m },
                new Product() { Id = Guid.NewGuid(), Name = "Product 3", Price = 100m }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var product in productEntities)
                    context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(productEntities.Count, service.ProductService.GetAllProducts().Count);
            }
        }

        [Fact]
        public void ProductService_Test_GetProductById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_GetProductById")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ProductService.GetProductById(productEntity.Id));
            }
        }

        [Fact]
        public void ProductService_Test_GetProductBySeo()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_GetProductBySeo")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m, SeoUrl = "Product-1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ProductService.GetProductBySeo(productEntity.SeoUrl));
            }
        }

        [Fact]
        public void ProductService_Test_InsertProduct()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_InsertProduct")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ProductService.InsertProduct(productEntity);

                // assert
                Assert.Equal(1, service.ProductService.GetAllProducts().Count);
            }
        }

        [Fact]
        public void ProductService_Test_UpdateProduct()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_UpdateProduct")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                productEntity.Name = "Product 1 Updated";
                service.ProductService.UpdateProduct(productEntity);

                // assert
                Assert.Equal("Product 1 Updated", service.ProductService.GetAllProducts().Single().Name);
            }
        }

        [Fact]
        public void ProductService_Test_DeleteProduct()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_DeleteProduct")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ProductService.DeleteProducts(new List<Guid> { productEntity.Id });

                // assert
                Assert.Equal(0, service.ProductService.GetAllProducts().Count);
            }
        }

        [Fact]
        public void ProductService_Test_Table()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_Table")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // assert
                Assert.NotNull(service.ProductService.Table());
            }
        }
    }
}
