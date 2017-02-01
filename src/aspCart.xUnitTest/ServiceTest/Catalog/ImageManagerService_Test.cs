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
    public class ImageManagerService_Test
    {
        [Fact]
        public void ImageManagerService_Test_GetAllImages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ImageManagerService_Test_GetAllImages")
                .Options;

            var imageEntities = new List<Image>()
            {
                new Image { Id = Guid.NewGuid(), FileName = "Image 1" },
                new Image { Id = Guid.NewGuid(), FileName = "Image 2" },
                new Image { Id = Guid.NewGuid(), FileName = "Image 3" }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var image in imageEntities)
                    context.Images.Add(image);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(imageEntities.Count, service.ImageManagerService.GetAllImages().Count);
            }
        }

        [Fact]
        public void ImageManagerService_Test_GetImageById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ImageManagerService_Test_GetImageById")
                .Options;

            var imageEntity = new Image { Id = Guid.NewGuid(), FileName = "Image 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Images.Add(imageEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ImageManagerService.GetImageById(imageEntity.Id));
            }
        }

        [Fact]
        public void ImageManagerService_Test_SearchImages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ImageManagerService_Test_SearchImages")
                .Options;

            var imageEntity = new Image { Id = Guid.NewGuid(), FileName = "Image 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Images.Add(imageEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ImageManagerService.SearchImages("Image"));
            }
        }

        [Fact]
        public void ImageManagerService_Test_InsertImages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ImageManagerService_Test_InsertImage")
                .Options;

            var imageEntities = new List<Image> { new Image { Id = Guid.NewGuid(), FileName = "Image 1" } };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ImageManagerService.InsertImages(imageEntities);

                // assert
                Assert.Equal(1, service.ImageManagerService.GetAllImages().Count);
            }
        }

        [Fact]
        public void ImageManagerService_Test_DeleteImages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ImageManagerService_Test_DeleteImages")
                .Options;

            var imageEntity = new Image { Id = Guid.NewGuid(), FileName = "Image 1" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Images.Add(imageEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ImageManagerService.DeleteImages(new List<Guid> { imageEntity.Id });

                // assert
                Assert.Equal(0, service.ImageManagerService.GetAllImages().Count);
            }
        }
    }
}
