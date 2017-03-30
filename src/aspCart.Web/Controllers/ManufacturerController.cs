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
    public class ManufacturerController : Controller
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ManufacturerController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public IActionResult ManufacturerInfo([FromQuery] string[] category, string seo)
        {
            if(seo != null)
            {
                ViewData["Manufacturer"] = seo;
                var productEntities = _productService.GetAllProducts()
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.SeoUrl == seo));
                var productList = new List<ProductModel>();

                foreach(var product in productEntities)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(product);

                    // get main image
                    if (product.Images.Count > 0)
                    {
                        productModel.MainImage = product.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image
                            .FileName;
                    }

                    // filter result by category
                    if(category.Length > 0)
                    {
                        foreach (var c in category)
                        {
                            if (product.Categories.Any(x => x.Category.Name == c))
                            {
                                productList.Add(productModel);
                                break;
                            }
                        }
                    }
                    else
                    {
                        productList.Add(productModel);
                    }
                }

                return View(productList);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
