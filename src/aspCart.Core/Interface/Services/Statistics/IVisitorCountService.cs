using aspCart.Core.Domain.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Statistics
{
    public interface IVisitorCountService
    {
        /// <summary>
        /// Get all VisitorCount
        /// </summary>
        /// <returns>VisitorCount entity</returns>
        IList<VisitorCount> GetAllVisitorCount();

        /// <summary>
        /// Get all VisitorCount
        /// </summary>
        /// <param name="take">Number of date to return</param>
        /// <returns>VisitorCount entity</returns>
        IList<VisitorCount> GetAllVisitorCount(int take);

        /// <summary>
        /// Get VisitorCount by date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>VisitorCount entity</returns>
        VisitorCount GetVisitorCountByDate(DateTime date);

        /// <summary>
        /// Insert VisitorCount entity
        /// </summary>
        /// <param name="visitorCount">VisitorCount entity</param>
        void InsertVisitorCount(VisitorCount visitorCount);

        /// <summary>
        /// Update VisitorCount entity 
        /// </summary>
        /// <param name="visitorCount">VisitorCount entity</param>
        void UpdateVisitorCount(VisitorCount visitorCount);
    }
}
