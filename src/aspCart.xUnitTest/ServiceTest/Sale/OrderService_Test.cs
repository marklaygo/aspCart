using aspCart.Core.Domain.Sale;
using aspCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace aspCart.xUnitTest.ServiceTest.Sale
{
    public class OrderService_Test
    {
        [Fact]
        public void OrderService_Test_GetAllOrders()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_GetAllOrders")
                .Options;

            var userId = Guid.NewGuid();

            var orderEntities = new List<Order>()
            {
                new Order() { Id = Guid.NewGuid(), UserId = userId },
                new Order() { Id = Guid.NewGuid(), UserId = userId },
                new Order() { Id = Guid.NewGuid(), UserId = userId }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var order in orderEntities)
                    context.Orders.Add(order);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(orderEntities.Count, service.OrderService.GetAllOrders().Count);
            }
        }

        [Fact]
        public void OrderService_Test_GetOrderById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_GetOrderById")
                .Options;

            var userId = Guid.NewGuid();
            var orderEntity = new Order() { Id = Guid.NewGuid(), UserId = userId };

            using (var context = new ApplicationDbContext(options))
            {
                context.Orders.Add(orderEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.OrderService.GetOrderById(orderEntity.Id));
            }
        }

        [Fact]
        public void OrderService_Test_GetOrderByOrderId()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_GetOrderByOrderId")
                .Options;

            var userId = Guid.NewGuid();
            var orderEntity = new Order() { Id = Guid.NewGuid(), UserId = userId, OrderNumber = "123-123-123456" };

            using (var context = new ApplicationDbContext(options))
            {
                context.Orders.Add(orderEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.OrderService.GetOrderByOrderId(orderEntity.OrderNumber));
            }
        }

        [Fact]
        public void OrderService_Test_GetAllOrdersByUserId()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_GetAllOrdersByUserId")
                .Options;

            var userId = Guid.NewGuid();

            var orderEntities = new List<Order>()
            {
                new Order() { Id = Guid.NewGuid(), UserId = userId },
                new Order() { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }, // fake user
                new Order() { Id = Guid.NewGuid(), UserId = userId }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var order in orderEntities)
                    context.Orders.Add(order);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(2, service.OrderService.GetAllOrdersByUserId(userId).Count);
            }
        }

        [Fact]
        public void OrderService_Test_InsertOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_InsertOrder")
                .Options;

            var userId = Guid.NewGuid();
            var orderEntity = new Order() { Id = Guid.NewGuid(), UserId = userId };

            using (var context = new ApplicationDbContext(options))
            {
                context.Orders.Add(orderEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.OrderService.GetOrderById(orderEntity.Id));
            }
        }

        [Fact]
        public void OrderService_Test_UpdateOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_UpdateOrder")
                .Options;

            var userId = Guid.NewGuid();
            var newUserId = Guid.NewGuid();
            var orderEntity = new Order() { Id = Guid.NewGuid(), UserId = userId };

            using (var context = new ApplicationDbContext(options))
            {
                context.Orders.Add(orderEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                orderEntity.UserId = newUserId;
                service.OrderService.UpdateOrder(orderEntity);

                // assert
                Assert.Equal(newUserId, service.OrderService.GetAllOrders().Single().UserId);
            }
        }

        [Fact]
        public void OrderService_Test_DeleteOrders()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderService_Test_DeleteOrders")
                .Options;

            var userId = Guid.NewGuid();
            var orderEntity = new Order() { Id = Guid.NewGuid(), UserId = userId };

            using (var context = new ApplicationDbContext(options))
            {
                context.Orders.Add(orderEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                service.OrderService.DeleteOrders(new List<Guid>() { orderEntity.Id });

                // assert
                Assert.Equal(0, service.OrderService.GetAllOrders().Count);
            }
        }
    }
}
