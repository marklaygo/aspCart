using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using aspCart.Core.Interface.Services.Catalog;

namespace aspCart.Web.Controllers
{
    public class CartController : Controller
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private readonly string _cartItesmSessionKey = "CartItems";
        private readonly string _cartItemsCountSessionKey = "CartItemsCount";

        #endregion

        #region Constructor

        public CartController(
            IProductService productService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
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

                if(selectedItem.Images.Count > 0)
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

        #endregion
    }
}
