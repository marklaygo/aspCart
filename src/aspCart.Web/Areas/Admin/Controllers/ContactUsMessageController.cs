using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Messages;
using AutoMapper;
using aspCart.Core.Domain.Messages;
using aspCart.Web.Areas.Admin.Models.Support;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class ContactUsMessageController : AdminController
    {
        #region Fields

        private readonly IContactUsService _contactUsService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ContactUsMessageController(
            IContactUsService contactUsService,
            IMapper mapper)
        {
            _contactUsService = contactUsService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var entities = _contactUsService.GetAllMessages();
            var messages = new List<ContactUsMessageModel>();

            foreach (var entity in entities)
            {
                messages.Add(_mapper.Map<ContactUsMessage, ContactUsMessageModel>(entity));
            }
            
            return View(messages);
        }

        public IActionResult Message(Guid id)
        {
            if(id != null)
            {
                var messageEntity = _contactUsService.GetMessageById(id);
                if(messageEntity != null)
                {
                    var model = _mapper.Map<ContactUsMessage, ContactUsMessageModel>(messageEntity);

                    if (model.Read == false)
                        _contactUsService.MarkAsRead(id);

                    return View(model);
                }
            }
            
            return RedirectToAction("List");
        }

        // Post: /ContactUsMessage/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return RedirectToAction("List");

            _contactUsService.DeleteMessages(ids);

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(string To, string Subject, string Reply)
        {
            var from = new EmailAddress("no-reply@aspcart.com", "aspcart no-reply");
            var to = new EmailAddress(To);
            var subject = "Re: " + Subject;

            SendMail(from, to, subject, Reply, Reply).Wait();
            return RedirectToAction("List");
        }

        // basic email sender
        static async Task SendMail(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };

            msg.AddTo(to);
            var response = await client.SendEmailAsync(msg);
        }

        #endregion
    }
}
