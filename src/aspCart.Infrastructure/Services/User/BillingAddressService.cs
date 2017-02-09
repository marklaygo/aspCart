using aspCart.Core.Domain.User;
using aspCart.Core.Interface.Services.User;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.User
{
    public class BillingAddressService : IBillingAddressService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<BillingAddress> _billingAddressRepository;

        #endregion

        #region Constructor

        public BillingAddressService(
            ApplicationDbContext context,
            IRepository<BillingAddress> billingAddressRepository)
        {
            _context = context;
            _billingAddressRepository = billingAddressRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get billing address by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Billing address entity</returns>
        public BillingAddress GetBillingAddressById(Guid id)
        {
            return _billingAddressRepository.FindByExpression(x => x.Id == id);
        }

        /// <summary>
        /// Insert billing address
        /// </summary>
        /// <param name="billingAddress">Billing address entity</param>
        public void InsertBillingAddress(BillingAddress billingAddress)
        {
            if (billingAddress == null)
                throw new ArgumentNullException("billingAddress");

            _billingAddressRepository.Insert(billingAddress);
            _billingAddressRepository.SaveChanges();
        }

        /// <summary>
        /// Update billing address
        /// </summary>
        /// <param name="billingAddress">Billing address entity</param>
        public void UpdateBillingAddress(BillingAddress billingAddress)
        {
            if (billingAddress == null)
                throw new ArgumentNullException("billingAddress");

            _billingAddressRepository.Update(billingAddress);
            _billingAddressRepository.SaveChanges();
        }

        #endregion
    }
}
