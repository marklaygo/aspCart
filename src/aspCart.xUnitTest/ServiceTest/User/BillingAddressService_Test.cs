using aspCart.Core.Domain.User;
using aspCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace aspCart.xUnitTest.ServiceTest.User
{
    public class BillingAddressService_Test
    {
        [Fact]
        public void BillingAddressService_Test_GetBillingAddressById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BillingAddressService_Test_GetBillingAddressById")
                .Options;

            var billingAddressEntity = new BillingAddress() { Id = Guid.NewGuid(), Address = "local", Telephone = "0123456789" };

            using (var context = new ApplicationDbContext(options))
            {
                context.BillingAddresses.Add(billingAddressEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.BillingAddressService.GetBillingAddressById(billingAddressEntity.Id));
            }
        }

        [Fact]
        public void BillingAddressService_Test_InsertBillingAddress()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BillingAddressService_Test_InsertBillingAddress")
                .Options;

            var billingAddressEntity = new BillingAddress() { Id = Guid.NewGuid(), Address = "local", Telephone = "0123456789" };

            using (var context = new ApplicationDbContext(options))
            {
                context.BillingAddresses.Add(billingAddressEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.BillingAddressService.GetBillingAddressById(billingAddressEntity.Id));
            }
        }

        [Fact]
        public void BillingAddressService_Test_UpdateBillingAddress()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BillingAddressService_Test_UpdateBillingAddress")
                .Options;

            var billingAddressEntity = new BillingAddress() { Id = Guid.NewGuid(), Address = "local", Telephone = "0123456789" };

            using (var context = new ApplicationDbContext(options))
            {
                context.BillingAddresses.Add(billingAddressEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);

                // act
                billingAddressEntity.Address = "local updated";
                service.BillingAddressService.UpdateBillingAddress(billingAddressEntity);

                // assert
                Assert.Equal("local updated", service.BillingAddressService.GetBillingAddressById(billingAddressEntity.Id).Address);
            }
        }
    }
}
