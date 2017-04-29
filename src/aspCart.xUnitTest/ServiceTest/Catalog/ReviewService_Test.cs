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
    public class ReviewService_Test
    {
        [Fact]
        public void ReviewService_Test_GetReviewsByProductId()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReviewService_Test_GetReviewsByProductId")
                .Options;

            Guid testProductId = Guid.NewGuid();
            var reviewEntities = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), ProductId = testProductId },
                new Review { Id = Guid.NewGuid(), ProductId = Guid.NewGuid() }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach(var review in reviewEntities)
                {
                    context.Reviews.Add(review);
                }
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(1, service.ReviewService.GetReviewsByProductId(testProductId).Count);
            }
        }

        [Fact]
        public void ReviewService_Test_InsertReview()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReviewService_Test_InsertReview")
                .Options;

            Guid testProductId = Guid.NewGuid();
            var reviewEntity = new Review { Id = Guid.NewGuid(), ProductId = testProductId };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.ReviewService.InsertReview(reviewEntity);

                // assert
                Assert.Equal(1, service.ReviewService.GetReviewsByProductId(testProductId).Count);
            }
        }

        [Fact]
        public void ReviewService_Test_EditReview()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReviewService_Test_EditReview")
                .Options;

            Guid testProductId = Guid.NewGuid();
            Guid testUserId = Guid.NewGuid();
            var reviewEntity = new Review { Id = Guid.NewGuid(), ProductId = testProductId, UserId = testUserId,  Rating = 5 };

            using (var context = new ApplicationDbContext(options))
            {
                context.Reviews.Add(reviewEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                var updatedEntity = reviewEntity;
                reviewEntity.Rating = 1;
                service.ReviewService.UpdateReview(updatedEntity);

                // assert
                Assert.Equal(1, service.ReviewService.GetReviewByProductIdUserId(testProductId, testUserId).Rating);
            }
        }
    }
}
