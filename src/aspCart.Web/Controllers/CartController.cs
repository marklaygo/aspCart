using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Core.Interface.Services.User;
using aspCart.Core.Interface.Services.Sale;
using Microsoft.AspNetCore.Authorization;
using aspCart.Core.Domain.User;
using AutoMapper;
using aspCart.Infrastructure.EFModels;
using Microsoft.AspNetCore.Identity;
using aspCart.Core.Domain.Sale;

namespace aspCart.Web.Controllers
{
    public class CartController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly IBillingAddressService _billingAddressService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private readonly string _cartItesmSessionKey = "CartItems";
        private readonly string _cartItemsCountSessionKey = "CartItemsCount";

        #endregion

        #region Constructor

        public CartController(
            UserManager<ApplicationUser> userManager,
            IProductService productService,
            IBillingAddressService billingAddressService,
            IOrderService orderService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _userManager = userManager;
            _productService = productService;
            _billingAddressService = billingAddressService;
            _orderService = orderService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        // GET: /Cart/
        public IActionResult Index()
        {
            var cartItems = new List<CartItemModel>();

            if (Session.GetString(_cartItesmSessionKey) != null)
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(Session.GetString(_cartItesmSessionKey));

            return View(cartItems);
        }

        // POST: /Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Guid id)
        {
            if(id == null)
                return RedirectToAction("Index");

            // check if product exist
            var selectedItem = _productService.GetProductById(id);
            if(selectedItem == null)
                return RedirectToAction("Index");

            // check if there are already cart instance
            var cartItems = new List<CartItemModel>();
            if (Session.GetString(_cartItesmSessionKey) != null)
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(Session.GetString(_cartItesmSessionKey));

            // check if the item are already in the cart
            // if the item is already in the cart,
            // increase the quantity by 1
            if (cartItems.Exists(x => x.Id == selectedItem.Id))
                cartItems.Find(x => x.Id == selectedItem.Id).Quantity++;
            else
            {
                var item = new CartItemModel
                {
                    Id = selectedItem.Id,
                    Name = selectedItem.Name,
                    Price = selectedItem.Price,
                    Quantity = 1,
                    MaxCartQuantity = selectedItem.MaximumCartQuantity,
                    SeoUrl = selectedItem.SeoUrl
                };

                // check for discount
                if (selectedItem.SpecialPriceEndDate != null && selectedItem.SpecialPriceEndDate >= DateTime.Now)
                {
                    item.OldPrice = selectedItem.Price;
                    item.Price = selectedItem.SpecialPrice ?? item.OldPrice;
                }

                if (selectedItem.Images.Count > 0)
                {
                    item.MainImage = selectedItem.Images
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Position)
                        .FirstOrDefault()
                        .Image
                        .FileName;
                }

                cartItems.Add(item);
            }

            // add to session
            Session.SetString(_cartItesmSessionKey, JsonConvert.SerializeObject(cartItems));
            Session.SetInt32(_cartItemsCountSessionKey, cartItems.Count());

            return RedirectToAction("Index");
        }

        // GET: /Cart/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(List<string> ids, List<string> quantity)
        {
            var cartItems = new List<CartItemModel>();

            for (int i = 0; i < ids.Count; i++)
            {
                int qty = Convert.ToInt32(Math.Floor(Convert.ToDouble(quantity[i])));
                if (qty == 0)
                    continue;

                Guid id;
                if (Guid.TryParse(ids[i], out id))
                {
                    // check if id is valid
                    var item = _productService.GetProductById(id);
                    if(item != null)
                    {
                        var newCartItem = new CartItemModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Price = item.Price,
                            Quantity = qty,
                            MaxCartQuantity = item.MaximumCartQuantity,
                            SeoUrl = item.SeoUrl
                        };

                        // check for discount
                        if (item.SpecialPriceEndDate != null && item.SpecialPriceEndDate >= DateTime.Now)
                        {
                            newCartItem.OldPrice = item.Price;
                            newCartItem.Price = item.SpecialPrice ?? newCartItem.OldPrice;
                        }

                        if (item.Images.Count > 0)
                        {
                            newCartItem.MainImage = item.Images
                                .OrderBy(x => x.SortOrder)
                                .ThenBy(x => x.Position)
                                .FirstOrDefault()
                                .Image
                                .FileName;
                        }

                        cartItems.Add(newCartItem);
                    }
                }
            }

            // add or remove sesion
            if(cartItems.Count > 0)
            {
                Session.SetString(_cartItesmSessionKey, JsonConvert.SerializeObject(cartItems));
                Session.SetInt32(_cartItemsCountSessionKey, cartItems.Count());
            }
            else
            {
                Session.Remove(_cartItesmSessionKey);
                Session.Remove(_cartItemsCountSessionKey);
            }

            return RedirectToAction("Index");
        }

        // GET: /Cart/Checkout
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            if (Session.GetString(_cartItesmSessionKey) == null)
                return View("Index");

            var user = await GetCurrentUserAsync();
            var checkoutModel = new CheckoutModel();
            var cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(Session.GetString(_cartItesmSessionKey));
            var billingAdddressEntity = _billingAddressService.GetBillingAddressById(user.BillingAddressId);
            if (billingAdddressEntity != null)
            {
                Session.SetString("BillingAddress", JsonConvert.SerializeObject(billingAdddressEntity));
                ViewData["BillingAddress"] = true;
            }

            checkoutModel.CartItemModel = cartItems;

            return View(checkoutModel);
        }

        // POST: /Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            // get current user
            var user = await GetCurrentUserAsync();
            var totalOrderPrice = 0m;

            // create order entity 
            var orderEntity = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = GenerateUniqueOrderNumber(),
                UserId = Guid.Parse(user.Id),
                Status = OrderStatus.Pending,
                OrderPlacementDateTime = DateTime.Now
            };

            var orderItemEntities = new List<OrderItem>();
            var cartItems = new List<CartItemModel>();

            // get cart session
            if (Session.GetString(_cartItesmSessionKey) != null)
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(Session.GetString(_cartItesmSessionKey));

            foreach(var item in cartItems)
            {
                var currentItem = _productService.GetProductById(item.Id);
                if(currentItem != null)
                {
                    var newOrderItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderEntity.Id,
                        ProductId = item.Id.ToString(),
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = (item.Price * item.Quantity)
                    };

                    orderItemEntities.Add(newOrderItem);
                    totalOrderPrice += newOrderItem.TotalPrice;
                }
            }

            // check if the order have item/s
            if (orderItemEntities.Count > 0)
            {
                // create billingAddress for this order
                var billingAddressEntity = _mapper.Map<CheckoutModel, BillingAddress>(model);
                _billingAddressService.InsertBillingAddress(billingAddressEntity);

                orderEntity.Items = orderItemEntities;
                orderEntity.TotalOrderPrice = totalOrderPrice;
                orderEntity.BillingAddressId = billingAddressEntity.Id;

                // save
                _orderService.InsertOrder(orderEntity);

                // clear cart session
                Session.Remove(_cartItesmSessionKey);
                Session.Remove(_cartItemsCountSessionKey);

                return RedirectToAction("OrderHistoryList", "Manage");
            }

            // something went wrong
            cartItems = new List<CartItemModel>();
            return RedirectToAction("Index", "Cart", cartItems);
        }

        #endregion

        #region Helpers

        private string GenerateUniqueOrderNumber()
        {
            var rand = new Random();
            string orderNumber = rand.Next(100, 999) + "-" + rand.Next(100, 999) + "-" + rand.Next(100000, 999999);

            var orderId = _orderService.GetOrderByOrderId(orderNumber);
            
            while (orderId != null)
            {
                orderNumber = rand.Next(100, 999) + "-" + rand.Next(100, 999) + "-" + rand.Next(100000, 999999);
                orderId = _orderService.GetOrderByOrderId(orderNumber);
            }

            return orderNumber;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
