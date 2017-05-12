using aspCart.Core.Domain.Messages;
using aspCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace aspCart.xUnitTest.ServiceTest.Messages
{
    public class ContactUsService_Test
    {
        [Fact]
        public void ContactUsService_Test_GetAllMessages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_GetAllMessages")
                .Options;

            var contactUsMessageEntities = new List<ContactUsMessage>()
            {
                new ContactUsMessage { Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now },
                new ContactUsMessage { Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var message in contactUsMessageEntities)
                    context.ContactUsMessage.Add(message);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(contactUsMessageEntities.Count, service.ContactUsService.GetAllMessages().Count);
            }
        }

        [Fact]
        public void ContactUsService_Test_GetMessageById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_GetMessageById")
                .Options;

            var testId = Guid.NewGuid();
            var contactUsMessage = new ContactUsMessage { Id = testId, Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now };

            using (var context = new ApplicationDbContext(options))
            {
                context.ContactUsMessage.Add(contactUsMessage);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ContactUsService.GetMessageById(testId));
            }
        }

        [Fact]
        public void ContactUsService_Test_InsertMessage()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_InsertMessage")
                .Options;

            var testId = Guid.NewGuid();
            var contactUsMessage = new ContactUsMessage { Id = testId, Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // act
                service.ContactUsService.InsertMessage(contactUsMessage);

                // assert
                Assert.NotNull(service.ContactUsService.GetMessageById(testId));
            }
        }

        [Fact]
        public void ContactUsService_Test_UpdateMessage()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_UpdateMessage")
                .Options;

            var testId = Guid.NewGuid();
            var contactUsMessage = new ContactUsMessage { Id = testId, Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now };

            using (var context = new ApplicationDbContext(options))
            {
                context.ContactUsMessage.Add(contactUsMessage);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // act
                contactUsMessage.Email = "new@email.com";
                service.ContactUsService.UpdateMessage(contactUsMessage);

                // assert
                Assert.Equal("new@email.com", service.ContactUsService.GetMessageById(testId).Email);
            }
        }

        [Fact]
        public void ContactUsService_Test_DeleteMessages()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_DeleteMessages")
                .Options;

            var testId = Guid.NewGuid();
            var contactUsMessageEntities = new List<ContactUsMessage>()
            {
                new ContactUsMessage { Id = testId, Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now },
                new ContactUsMessage { Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var message in contactUsMessageEntities)
                    context.ContactUsMessage.Add(message);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // act
                service.ContactUsService.DeleteMessages(new List<Guid> { testId });

                // assert
                Assert.Equal(1, service.ContactUsService.GetAllMessages().Count);
            }
        }

        [Fact]
        public void ContactUsService_Test_MarkAsRead()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactUsService_Test_MarkAsRead")
                .Options;

            var testId = Guid.NewGuid();
            var contactUsMessage = new ContactUsMessage { Id = testId, Email = "email@email.com", Title = "Title", Message = "Message", Read = false, SendDate = DateTime.Now };

            using (var context = new ApplicationDbContext(options))
            {
                context.ContactUsMessage.Add(contactUsMessage);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // act
                service.ContactUsService.MarkAsRead(testId);

                // assert
                Assert.True(service.ContactUsService.GetMessageById(testId).Read);
            }
        }
    }
}
