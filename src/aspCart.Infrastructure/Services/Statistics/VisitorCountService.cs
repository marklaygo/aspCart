using aspCart.Core.Domain.Statistics;
using aspCart.Core.Interface.Services.Statistics;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Statistics
{
    public class VisitorCountService : IVisitorCountService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<VisitorCount> _visitorCountRepository;

        #endregion

        #region Constructor

        public VisitorCountService(
            ApplicationDbContext context,
            IRepository<VisitorCount> visitorCountRepository)
        {
            _context = context;
            _visitorCountRepository = visitorCountRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all VisitorCount
        /// </summary>
        /// <returns>VisitorCount entity</returns>
        public IList<VisitorCount> GetAllVisitorCount()
        {
            return _visitorCountRepository.GetAll().ToList();
        }

        /// <summary>
        /// Get all VisitorCount
        /// </summary>
        /// <param name="take">Number of date to return</param>
        /// <returns>VisitorCount entity</returns>
        public IList<VisitorCount> GetAllVisitorCount(int take)
        {
            return _visitorCountRepository.GetAll().Take(take).ToList(); ;
        }

        /// <summary>
        /// Get VisitorCount by date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>VisitorCount entity</returns>
        public VisitorCount GetVisitorCountByDate(DateTime date)
        {
            //return _visitorCountRepository.FindByExpression(x => x.Date == date.Date);
            return _context.VisitorCounts.SingleOrDefault(x => x.Date == date.Date);
        }

        /// <summary>
        /// Insert VisitorCount entity
        /// </summary>
        /// <param name="visitorCount">VisitorCount entity</param>
        public void InsertVisitorCount(VisitorCount visitorCount)
        {
            if(visitorCount == null)
                throw new ArgumentNullException("visitorCount");

            visitorCount.Date = visitorCount.Date.Date;

            _visitorCountRepository.Insert(visitorCount);
            _visitorCountRepository.SaveChanges();
        }

        /// <summary>
        /// Update VisitorCount entity 
        /// </summary>
        /// <param name="visitorCount">VisitorCount entity</param>
        public void UpdateVisitorCount(VisitorCount visitorCount)
        {
            if (visitorCount == null)
                throw new ArgumentNullException("visitorCount");

            visitorCount.Date = visitorCount.Date.Date;
            visitorCount.ViewCount++;

            _visitorCountRepository.Update(visitorCount);
            _visitorCountRepository.SaveChanges();
        }

        #endregion
    }
}
