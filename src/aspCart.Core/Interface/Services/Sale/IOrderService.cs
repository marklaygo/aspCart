using aspCart.Core.Domain.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Sale
{
    public interface IOrderService
    {
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of order entities</returns>
        IList<Order> GetAllOrders();

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Order entity</returns>
        Order GetOrderById(Guid id);

        /// <summary>
        /// Get order by order id
        /// </summary>
        /// <param name="id">Order number id</param>
        /// <returns>Order entity</returns>
        Order GetOrderByOrderId(string id);

        /// <summary>
        /// Get all orders by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of order entities</returns>
        IList<Order> GetAllOrdersByUserId(Guid userId);

        /// <summary>
        /// Insert order
        /// </summary>
        /// <param name="order">Order entity</param>
        void InsertOrder(Order order);

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="order">Order entity</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Delete all orders
        /// </summary>
        /// <param name="ids">List of order ids</param>
        void DeleteOrders(IList<Guid> ids);
    }
}
