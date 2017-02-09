using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Sale;
using aspCart.Core.Interface.Services.User;
using AutoMapper;
using aspCart.Web.Areas.Admin.Models.Sale;
using aspCart.Core.Domain.Sale;

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
                if (billingAddressEntity != null)
                {
                    var orderListModel = new OrderListModel
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        Name = billingAddressEntity.FirstName + " " + billingAddressEntity.LastName,
                        Email = billingAddressEntity.Email,
                        Status = order.Status.ToString(),
                        OrderPlacementDateTime = order.OrderPlacementDateTime,
                        TotalOrderPrice = order.TotalOrderPrice
                    };
                    orderList.Add(orderListModel);
                }
            }

            return View(orderList.OrderByDescending(x => x.OrderPlacementDateTime));
        }

        // GET: /Order/Manage
        public IActionResult Manage(Guid? id, string ActiveTab)
        {
            if (id == null)
                return RedirectToAction("List");

            var orderEntity = _orderService.GetOrderById(id ?? Guid.Empty);
            if (orderEntity == null)
                return RedirectToAction("List");

            var billingAddressEntity = _billingAddressService.GetBillingAddressById(orderEntity.BillingAddressId);

            var model = new OrderManageModel
            {
                Id = orderEntity.Id,
                OrderNumber = orderEntity.OrderNumber,
                UserId = orderEntity.UserId,
                BillingAddressId = billingAddressEntity.Id,
                FirstName = billingAddressEntity.FirstName,
                LastName = billingAddressEntity.LastName,
                Email = billingAddressEntity.Email,
                Address = billingAddressEntity.Address,
                City = billingAddressEntity.City,
                StateProvince = billingAddressEntity.StateProvince,
                ZipPostalCode = billingAddressEntity.ZipPostalCode,
                Country = billingAddressEntity.Country,
                Telephone = billingAddressEntity.Telephone,
                OrderStatus = orderEntity.Status,
                OrderPlacementDateTime = orderEntity.OrderPlacementDateTime,
                OrderCompletedDateTime = orderEntity.OrderCompletedDateTime,
                TotalOrderPrice = orderEntity.TotalOrderPrice,
                OrderItems = orderEntity.Items
            };
            model.ActiveTab = ActiveTab ?? model.ActiveTab;

            return View(model);
        }

        // POST: /Order/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Manage(Guid id, string ActiveTab, int OrderStatus, bool continueEditing)
        {
            var orderEntity = _orderService.GetOrderById(id);
            if (orderEntity != null)
            {
                orderEntity.Status = (OrderStatus)OrderStatus;
                _orderService.UpdateOrder(orderEntity);

                if(continueEditing)
                    return RedirectToAction("Manage", new { id = id, ActiveTab = ActiveTab });
            }

            return RedirectToAction("List");
        }

        #endregion
    }
}
