using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Sale;
using aspCart.Core.Interface.User;
using AutoMapper;
using aspCart.Web.Areas.Admin.Models.Sale;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class OrderController : AdminController
    {
        #region Fields

        private readonly IBillingAddressService _billingAddressService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public OrderController(
            IBillingAddressService billingAddressService,
            IOrderService orderService,
            IMapper mapper)
        {
            _billingAddressService = billingAddressService;
            _orderService = orderService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Order/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /Order/List
        public IActionResult List()
        {
            var orderEntities = _orderService.GetAllOrders();
            var orderList = new List<OrderListModel>();

            foreach (var order in orderEntities)
            {
                // get billing address
                var billingAddressEntity = _billingAddressService.GetBillingAddressById(order.BillingAddressId);

                var orderListModel = new OrderListModel
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    Name = billingAddressEntity.FirstName + " " + billingAddressEntity.LastName,
                    Email = billingAddressEntity.Email,
                    Status = order.Status.ToString(),
                    TotalOrderPrice = order.TotalOrderPrice
                };
                orderList.Add(orderListModel);
            }

            return View(orderList);
        }

        #endregion
    }
}