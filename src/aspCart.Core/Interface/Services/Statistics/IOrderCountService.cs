using aspCart.Core.Domain.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Statistics
{
    public interface IOrderCountService
    {
        /// <summary>
        /// Get all OrderCount
        /// </summary>
        /// <returns>OrderCount entity</returns>
        IList<OrderCount> GetAllOrderCount();

        /// <summary>
        /// Get all OrderCount
        /// </summary>
        /// <param name="take">Number of date to return</param>
        /// <returns>OrderCount entities</returns>
        IList<OrderCount> GetAllOrderCount(int take);

        /// <summary>
        /// Get OrderCount by date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>OrderCount entity</returns>
        OrderCount GetOrderCountByDate(DateTime date);

        /// <summary>
        /// Insert OrderCount
        /// </summary>
        /// <param name="orderCount">OrderCount entity</param>
        void InsertOrderCount(OrderCount orderCount);

        /// <summary>
        /// Update OrderCount
        /// </summary>
        /// <param name="orderCount">OrderCount entity</param>
        void UpdateOrderCount(OrderCount orderCount);
    }
}
