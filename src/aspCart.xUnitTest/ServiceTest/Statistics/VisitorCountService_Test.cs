using aspCart.Core.Domain.Statistics;
using aspCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace aspCart.xUnitTest.ServiceTest.Statistics
{
    public class VisitorCountService_Test
    {
        [Fact]
        public void VisitorCountService_Test_GetAllVisitorCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VisitorCountService_Test_GetAllVisitorCount")
                .Options;

            var visitorCountEntities = new List<VisitorCount>()
            {
                new VisitorCount { Date = DateTime.Now.AddDays(-1).Date, ViewCount = 100 },
                new VisitorCount { Date = DateTime.Now.Date, ViewCount = 200 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var visitorCount in visitorCountEntities)
                    context.VisitorCounts.Add(visitorCount);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(visitorCountEntities.Count, service.VisitorCountService.GetAllVisitorCount().Count);
            }
        }

        [Fact]
        public void VisitorCountService_Test_GetAllVisitorCountTake()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VisitorCountService_Test_GetAllVisitorCountTake")
                .Options;

            var visitorCountEntities = new List<VisitorCount>()
            {
                new VisitorCount { Date = DateTime.Now.AddDays(-1), ViewCount = 100 },
                new VisitorCount { Date = DateTime.Now, ViewCount = 200 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var visitorCount in visitorCountEntities)
                    context.VisitorCounts.Add(visitorCount);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(1, service.VisitorCountService.GetAllVisitorCount(1).Count);
            }
        }

        [Fact]
        public void VisitorCountService_Test_GetVisitorCountByDate()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VisitorCountService_Test_GetVisitorCountByDate")
                .Options;

            var visitorCountEntity = new VisitorCount { Date = DateTime.Now.Date, ViewCount = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                context.VisitorCounts.Add(visitorCountEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.VisitorCountService.GetVisitorCountByDate(DateTime.Now));
            }
        }

        [Fact]
        public void VisitorCountService_Test_InsertVisitorCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VisitorCountService_Test_InsertVisitorCount")
                .Options;

            var visitorCountEntity = new VisitorCount { Date = DateTime.Now, ViewCount = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.VisitorCountService.InsertVisitorCount(visitorCountEntity);

                // assert
                Assert.Equal(1, service.VisitorCountService.GetAllVisitorCount().Count);
            }
        }

        [Fact]
        public void VisitorCountService_Test_UpdateVisitorCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_UpdateProduct")
                .Options;

            var visitorCountEntity = new VisitorCount { Date = DateTime.Now, ViewCount = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                context.VisitorCounts.Add(visitorCountEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.VisitorCountService.UpdateVisitorCount(visitorCountEntity);

                // assert
                Assert.Equal(101, service.VisitorCountService.GetAllVisitorCount().Single().ViewCount);
            }
        }
    }
}
