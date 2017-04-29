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
    public class CategoryService_Test
    {
        [Fact]
        public void CategoryService_Test_GetAllCategories()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_GetAllCategories")
                .Options;

            var categoryEntities = new List<Category>()
            {
                new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty, SeoUrl = "Category-1" },
                new Category() { Id = Guid.NewGuid(), Name = "Category 2", ParentCategoryId = Guid.NewGuid(), SeoUrl = "Category-2" },
                new Category() { Id = Guid.NewGuid(), Name = "Category 3", ParentCategoryId = Guid.Empty, SeoUrl = "Category-3" }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var category in categoryEntities)
                    context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(categoryEntities.Count, service.CategoryService.GetAllCategories().Count);
            }
        }

        [Fact]
        public void CategoryService_Test_GetAllCategoryWithoutParent()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_GetAllCategoryWithoutParent")
                .Options;

            var categoryEntities = new List<Category>()
            {
                new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.NewGuid(), SeoUrl = "Category-1" },
                new Category() { Id = Guid.NewGuid(), Name = "Category 2", ParentCategoryId = Guid.Empty, SeoUrl = "Category-2" },
                new Category() { Id = Guid.NewGuid(), Name = "Category 3", ParentCategoryId = Guid.Empty, SeoUrl = "Category-3" }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var category in categoryEntities)
                    context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(2, service.CategoryService.GetAllCategoriesWithoutParent().Count);
            }
        }

        [Fact]
        public void CategoryService_Test_GetCategoryById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_GetCategoryById")
                .Options;

            var categoryEntity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty };

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(categoryEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.CategoryService.GetCategoryById(categoryEntity.Id));
            }
        }

        [Fact]
        public void CategoryService_Test_GetCategoryBySeo()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_GetCategoryBySeo")
                .Options;

            var categoryEntity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty, SeoUrl = "Category-1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(categoryEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.CategoryService.GetCategoryBySeo(categoryEntity.SeoUrl));
            }
        }

        [Fact]
        public void CategoryService_Test_InsertCategory()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_InsertCategory")
                .Options;

            var categoryEntity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.CategoryService.InsertCategory(categoryEntity);

                // assert
                Assert.Equal(1, service.CategoryService.GetAllCategories().Count);
            }
        }

        [Fact]
        public void CategoryService_Test_UpdateCategory()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_UpdateCategory")
                .Options;

            var categoryEntity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty };

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(categoryEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                categoryEntity.Name = "Category 1 Updated";
                service.CategoryService.UpdateCategory(categoryEntity);

                // assert
                Assert.Equal("Category 1 Updated", service.CategoryService.GetAllCategories().Single().Name);
            }
        }

        [Fact]
        public void CategoryService_Test_DeleteCategory()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_DeleteCategory")
                .Options;

            var categoryEntity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty };

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(categoryEntity);
                context.SaveChanges();
            }


            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.CategoryService.DeleteCategories(new List<Guid> { categoryEntity.Id });

                // assert
                Assert.Equal(0, service.CategoryService.GetAllCategories().Count);
            }
        }

        [Fact]
        public void CategoryService_Test_InsertProductCategoryMappings()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_InsertProductCategoryMappings")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };
            var category1Entity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty, SeoUrl = "Category-1" };
            var category2Entity = new Category() { Id = Guid.NewGuid(), Name = "Category 2", ParentCategoryId = Guid.NewGuid(), SeoUrl = "Category-2" };

            var categoryMappings = new List<ProductCategoryMapping>()
            {
                new ProductCategoryMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, CategoryId = category1Entity.Id },
                new ProductCategoryMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, CategoryId = category2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.Categories.Add(category1Entity);
                context.Categories.Add(category2Entity);
                foreach (var mapping in categoryMappings)
                    context.ProductCategoryMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(2, service.ProductService.GetProductById(productEntity.Id).Categories.Count);
            }
        }

        [Fact]
        public void CategoryService_Test_DeleteAllProductCategoryMappingsByProductId()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryService_Test_DeleteAllProductCategoryMappingsByProductId")
                .Options;

            var productEntity = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100m };
            var category1Entity = new Category() { Id = Guid.NewGuid(), Name = "Category 1", ParentCategoryId = Guid.Empty, SeoUrl = "Category-1" };
            var category2Entity = new Category() { Id = Guid.NewGuid(), Name = "Category 2", ParentCategoryId = Guid.NewGuid(), SeoUrl = "Category-2" };

            var categoryMappings = new List<ProductCategoryMapping>()
            {
                new ProductCategoryMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, CategoryId = category1Entity.Id },
                new ProductCategoryMapping() { Id = Guid.NewGuid(), ProductId = productEntity.Id, CategoryId = category2Entity.Id }
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.Categories.Add(category1Entity);
                context.Categories.Add(category2Entity);
                foreach (var mapping in categoryMappings)
                    context.ProductCategoryMappings.Add(mapping);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.CategoryService.DeleteAllProductCategoryMappingsByProductId(productEntity.Id);

                // assert
                Assert.Equal(0, service.ProductService.GetProductById(productEntity.Id).Categories.Count);
            }
        }
    }
}
