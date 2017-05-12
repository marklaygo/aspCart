using aspCart.Core.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Messages
{
    public interface IContactUsService
    {
        /// <summary>
        /// Get all ContactUsMessage
        /// </summary>
        /// <returns>List of ContactUsMessage entities</returns>
        IList<ContactUsMessage> GetAllMessages();

        /// <summary>
        /// Get ContactUsMessage using id
        /// </summary>
        /// <param name="id">ContactUsMessage id</param>
        /// <returns>ContactUsMessage entity</returns>
        ContactUsMessage GetMessageById(Guid id);

        /// <summary>
        /// Insert ContactUsMessage
        /// </summary>
        /// <param name="message">ContactUsMessage entity</param>
        void InsertMessage(ContactUsMessage message);

        /// <summary>
        /// Update ContactUsMessage
        /// </summary>
        /// <param name="message">ContactUsMessage entity</param>
        void UpdateMessage(ContactUsMessage message);

        /// <summary>
        /// Delete ContactUsMessage
        /// </summary>
        /// <param name="ids">List of ContactUsMessage ids</param>
        void DeleteMessages(IList<Guid> ids);

        /// <summary>
        /// Mark the ContactUsMessage as read
        /// </summary>
        /// <param name="id">ContactUsMessage id</param>
        void MarkAsRead(Guid id);
    }
}
