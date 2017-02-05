using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Models;
using AutoMapper;
using aspCart.Core.Domain.Catalog;

namespace aspCart.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public HomeController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Home/
        public IActionResult Index()
        {
            var productEntities = _productService.GetAllProducts()
                .Where(x => x.Published == true);
            var productList = new List<ProductModel>();

            foreach(var product in productEntities)
            {
                var productModel = _mapper.Map<Product, ProductModel>(product);

                // get main image
                if(product.Images.Count > 0)
                {
                    productModel.MainImage = product.Images
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Position)
                        .FirstOrDefault()
                        .Image
                        .FileName;
                }

                productList.Add(productModel);
            }

            return View(productList);
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion
    }
}
