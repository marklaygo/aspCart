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
    public class SpecificationService_Test
    {
        [Fact]
        public void SpecificationService_Test_GetAllSpecifications()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_GetAllSpecifications")
                .Options;

            var specificationEntities = new List<Specification>()
            {
                new Specification(){ Id = Guid.NewGuid(), Name = "Specification 1" },
                new Specification(){ Id = Guid.NewGuid(), Name = "Specification 2" },
                new Specification(){ Id = Guid.NewGuid(), Name = "Specification 3" }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var specification in specificationEntities)
                    context.Specifications.Add(specification);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(specificationEntities.Count, service.SpecificationService.GetAllSpecifications().Count);
            }
        }

        [Fact]
        public void SpecificationService_Test_GetSpecificationById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_GetSpecificationById")
                .Options;

            var specificationEntity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Specifications.Add(specificationEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.SpecificationService.GetSpecificationById(specificationEntity.Id));
            }
        }

        [Fact]
        public void SpecificationService_Test_InsertSpecification()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_InsertSpecification")
                .Options;

            var specificationEntity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.SpecificationService.InsertSpecification(specificationEntity);

                // assert
                Assert.Equal(1, service.SpecificationService.GetAllSpecifications().Count);
            }
        }

        [Fact]
        public void SpecificationService_Test_UpdateSpecification()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_UpdateSpecification")
                .Options;

            var specificationEntity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Specifications.Add(specificationEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                specificationEntity.Name = "Specification 1 Updated";
                service.SpecificationService.UpdateSpecification(specificationEntity);

                // assert
                Assert.Equal("Specification 1 Updated", service.SpecificationService.GetAllSpecifications().Single().Name);
            }
        }

        [Fact]
        public void SpecificationService_Test_DeleteSpecifications()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_DeleteSpecifications")
                .Options;

            var specificationEntity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Specifications.Add(specificationEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.SpecificationService.DeleteSpecifications(new List<Guid>() { specificationEntity.Id });

                // assert
                Assert.Equal(0, service.SpecificationService.GetAllSpecifications().Count);
            }
        }

        [Fact]
        public void SpecificationService_Test_InsertProductSpecificationMappings()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_InsertProductSpecificationMappings")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m, SeoUrl = "Product-1" };
            var specification1Entity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };
            var specification2Entity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 2" };

            var specificationMappings = new List<ProductSpecificationMapping>()
            {
                new ProductSpecificationMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, SpecificationId = specification1Entity.Id },
                new ProductSpecificationMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, SpecificationId = specification2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                context.Products.Add(productEntity);
                context.Specifications.Add(specification1Entity);
                context.Specifications.Add(specification2Entity);
                foreach (var mapping in specificationMappings)
                    context.ProductSpecificationMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(2, service.ProductService.GetProductById(productEntity.Id).Specifications.Count);
            }
        }

        [Fact]
        public void SpecificationService_Test_DeleteAllProductSpecificationMappings()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SpecificationService_Test_DeleteAllProductSpecificationMappings")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m, SeoUrl = "Product-1" };
            var specification1Entity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 1" };
            var specification2Entity = new Specification() { Id = Guid.NewGuid(), Name = "Specification 2" };

            var specificationMappings = new List<ProductSpecificationMapping>()
            {
                new ProductSpecificationMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, SpecificationId = specification1Entity.Id },
                new ProductSpecificationMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, SpecificationId = specification2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                context.Products.Add(productEntity);
                context.Specifications.Add(specification1Entity);
                context.Specifications.Add(specification2Entity);
                foreach (var mapping in specificationMappings)
                    context.ProductSpecificationMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.SpecificationService.DeleteAllProductSpecificationMappings(productEntity.Id);

                // assert
                Assert.Equal(0, service.ProductService.GetProductById(productEntity.Id).Specifications.Count);
            }
        }
    }
}
