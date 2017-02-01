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
    public class ManufacturerService_Test
    {
        [Fact]
        public void ManufacturerService_Test_GetAllManufacturers()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_GetAllManufacturers")
                .Options;

            var manufacturerEntities = new List<Manufacturer>
            {
                new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1" },
                new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 2" },
                new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 3" }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var manufacturer in manufacturerEntities)
                    context.Manufacturers.Add(manufacturer);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(manufacturerEntities.Count, service.ManufacturerService.GetAllManufacturers().Count);
            }
        }

        [Fact]
        public void ManufacturerService_Test_GetManufacturerById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_GetManufacturerById")
                .Options;

            var manufacturerEntity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Manufacturers.Add(manufacturerEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ManufacturerService.GetManufacturerById(manufacturerEntity.Id));
            }
        }

        [Fact]
        public void ManufacturerService_Test_GetManufacturerBySeo()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_GetManufacturerBySeo")
                .Options;

            var manufacturerEntity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Manufacturers.Add(manufacturerEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ManufacturerService.GetManufacturerBySeo(manufacturerEntity.SeoUrl));
            }
        }

        [Fact]
        public void ManufacturerService_Test_InsertManufacturer()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_InsertManufacturer")
                .Options;

            var manufacturerEntity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ManufacturerService.InsertManufacturer(manufacturerEntity);

                // assert
                Assert.Equal(1, service.ManufacturerService.GetAllManufacturers().Count);
            }
        }

        [Fact]
        public void ManufacturerService_Test_UpdateManufacturer()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_UpdateManufacturer")
                .Options;

            var manufacturerEntity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Manufacturers.Add(manufacturerEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                manufacturerEntity.Name = "Manufacturer 1 Updated";
                service.ManufacturerService.UpdateManufacturer(manufacturerEntity);

                // assert
                Assert.Equal("Manufacturer 1 Updated", service.ManufacturerService.GetAllManufacturers().Single().Name);
            }
        }

        [Fact]
        public void ManufacturerService_Test_DeleteManufacturers()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_DeleteManufacturers")
                .Options;

            var manufacturerEntity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Manufacturers.Add(manufacturerEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ManufacturerService.DeleteManufacturers(new List<Guid>() { manufacturerEntity.Id });

                // assert
                Assert.Equal(0, service.ManufacturerService.GetAllManufacturers().Count);
            }
        }

        [Fact]
        public void ManufacturerService_Test_InsertProductManufacturerMappings()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_InsertProductManufacturerMappings")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m, SeoUrl = "Product-1" };
            var manufacturer1Entity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };
            var manufacturer2Entity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 2", SeoUrl = "Manufacturer-2" };

            var manufacturerMappings = new List<ProductManufacturerMapping>()
            {
                new ProductManufacturerMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, ManufacturerId = manufacturer1Entity.Id },
                new ProductManufacturerMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, ManufacturerId = manufacturer2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.Manufacturers.Add(manufacturer1Entity);
                context.Manufacturers.Add(manufacturer2Entity);
                foreach (var mapping in manufacturerMappings)
                    context.ProductManufacturerMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(2, service.ProductService.GetProductById(productEntity.Id).Manufacturers.Count);
            }
        }

        [Fact]
        public void ManufacturerService_Test_DeleteAllProductManufacturersMappings()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerService_Test_DeleteAllProductManufacturersMappings")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m, SeoUrl = "Product-1" };
            var manufacturer1Entity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 1", SeoUrl = "Manufacturer-1" };
            var manufacturer2Entity = new Manufacturer() { Id = Guid.NewGuid(), Name = "Manufacturer 2", SeoUrl = "Manufacturer-2" };

            var manufacturerMappings = new List<ProductManufacturerMapping>()
            {
                new ProductManufacturerMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, ManufacturerId = manufacturer1Entity.Id },
                new ProductManufacturerMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, ManufacturerId = manufacturer2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.Manufacturers.Add(manufacturer1Entity);
                context.Manufacturers.Add(manufacturer2Entity);
                foreach (var mapping in manufacturerMappings)
                    context.ProductManufacturerMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ManufacturerService.DeleteAllProductManufacturersMappings(productEntity.Id);

                // assert
                Assert.Equal(0, service.ProductService.GetProductById(productEntity.Id).Manufacturers.Count);
            }
        }
    }
}
