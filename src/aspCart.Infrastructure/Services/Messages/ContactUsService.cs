using aspCart.Core.Domain.Messages;
using aspCart.Core.Interface.Services.Messages;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Messages
{
    public class ContactUsService : IContactUsService
    {
        #region Fields

        private readonly IRepository<ContactUsMessage> _contactUsRepository;

        #endregion

        #region Constructor

        public ContactUsService(IRepository<ContactUsMessage> contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all ContactUsMessage
        /// </summary>
        /// <returns>List of ContactUsMessage entities</returns>
        public IList<ContactUsMessage> GetAllMessages()
        {
            var entities = _contactUsRepository.GetAll()
                .OrderByDescending(x => x.SendDate)
                .ToList();

            return entities;
        }

        /// <summary>
        /// Get ContactUsMessage using id
        /// </summary>
        /// <param name="id">ContactUsMessage id</param>
        /// <returns>ContactUsMessage entity</returns>
        public ContactUsMessage GetMessageById(Guid id)
        {
            return _contactUsRepository.FindByExpression(x => x.Id == id);
        }

        /// <summary>
        /// Insert ContactUsMessage
        /// </summary>
        /// <param name="message">ContactUsMessage entity</param>
        public void InsertMessage(ContactUsMessage message)
        {
            if (message == null)
                throw new ArgumentException("message");

            _contactUsRepository.Insert(message);
            _contactUsRepository.SaveChanges();
        }

        /// <summary>
        /// Update ContactUsMessage
        /// </summary>
        /// <param name="message">ContactUsMessage entity</param>
        public void UpdateMessage(ContactUsMessage message)
        {
            if (message == null)
                throw new ArgumentException("message");

            _contactUsRepository.Update(message);
            _contactUsRepository.SaveChanges();
        }

        /// <summary>
        /// Delete ContactUsMessage
        /// </summary>
        /// <param name="ids">List of ContactUsMessage ids</param>
        public void DeleteMessages(IList<Guid> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            foreach (var id in ids)
                _contactUsRepository.Delete(GetMessageById(id));

            _contactUsRepository.SaveChanges();
        }

        /// <summary>
        /// Mark the ContactUsMessage as read
        /// </summary>
        /// <param name="id">ContactUsMessage id</param>
        public void MarkAsRead(Guid id)
        {
            if (id == null || id == Guid.Empty)
                throw new ArgumentNullException("id");

            var message = GetMessageById(id);
            message.Read = true;

            _contactUsRepository.Update(message);
            _contactUsRepository.SaveChanges();
        }

        #endregion
    }
}
