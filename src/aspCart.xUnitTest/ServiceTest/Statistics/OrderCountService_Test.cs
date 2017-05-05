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
    public class OrderCountService_Test
    {
        [Fact]
        public void OrderCountService_Test_GetAllOrderCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderCountService_Test_GetAllOrderCount")
                .Options;

            var orderCountEntities = new List<OrderCount>()
            {
                new OrderCount { Date = DateTime.Now.AddDays(-1).Date, Count = 100 },
                new OrderCount { Date = DateTime.Now.Date, Count = 200 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var orderCount in orderCountEntities)
                    context.OrderCounts.Add(orderCount);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(orderCountEntities.Count, service.OrderCountService.GetAllOrderCount().Count);
            }
        }

        [Fact]
        public void OrderCountService_Test_GetAllOrderCountTake()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderCountService_Test_GetAllOrderCountTake")
                .Options;

            var orderCountEntities = new List<OrderCount>()
            {
                new OrderCount { Date = DateTime.Now.AddDays(-1).Date, Count = 100 },
                new OrderCount { Date = DateTime.Now.Date, Count = 200 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var orderCount in orderCountEntities)
                    context.OrderCounts.Add(orderCount);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(1, service.OrderCountService.GetAllOrderCount(1).Count);
            }
        }

        [Fact]
        public void OrderCountService_Test_GetOrderCountByDate()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderCountService_Test_GetOrderCountByDate")
                .Options;

            var orderCountEntity = new OrderCount { Date = DateTime.Now.Date, Count = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                context.OrderCounts.Add(orderCountEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.OrderCountService.GetOrderCountByDate(DateTime.Now));
            }
        }

        [Fact]
        public void OrderCountService_Test_InsertOrderCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderCountService_Test_InsertOrderCount")
                .Options;

            var orderCountEntity = new OrderCount { Date = DateTime.Now.Date, Count = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.OrderCountService.InsertOrderCount(orderCountEntity);

                // assert
                Assert.NotNull(service.OrderCountService.GetOrderCountByDate(DateTime.Now));
            }
        }

        [Fact]
        public void VisitorCountService_Test_UpdateOrderCount()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VisitorCountService_Test_UpdateOrderCount")
                .Options;

            var orderCountEntity = new OrderCount { Date = DateTime.Now, Count = 100 };

            using (var context = new ApplicationDbContext(options))
            {
                context.OrderCounts.Add(orderCountEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.OrderCountService.UpdateOrderCount(orderCountEntity);

                // assert
                Assert.Equal(101, service.OrderCountService.GetAllOrderCount().Single().Count);
            }
        }
    }
}
