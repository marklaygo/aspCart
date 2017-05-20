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

        public IActionResult ManufacturerInfo(string seo, string sortBy, [FromQuery] string[] category, [FromQuery] string[] price)
        {
            if(seo != null)
            {
                ViewData["Manufacturer"] = seo;
                var productEntities = _productService.GetAllProducts()
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.SeoUrl.ToLower() == seo.ToLower()))
                    .Where(x => x.Published == true);

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

                    // get all categories
                    foreach(var c in product.Categories)
                    {
                        productModel.Categories.Add(new CategoryModel { Name = c.Category.Name, SeoUrl = c.Category.SeoUrl });
                    }

                    productList.Add(productModel);
                }

                var result = productList;

                if(category.Length > 0)
                {
                    result = result.Where(x => x.Categories.Select(c => c.Name).Intersect(category).Count() > 0).ToList();
                }

                if(price.Length > 0)
                {
                    var tmpResult = new List<ProductModel>();
                    foreach (var p in price)
                    {
                        var tmpPrice = p.Split(new char[] { '-' });
                        int minPrice = Convert.ToInt32(tmpPrice[0]);
                        int maxPrice = Convert.ToInt32(tmpPrice[1]);

                        var r = result.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();

                        if (r.Count > 0) { tmpResult.AddRange(r); }
                    }
                    result = tmpResult;
                }

                if (sortBy != null && sortBy.Length > 0)
                {
                    switch (sortBy)
                    {
                        case "LowestPrice":
                            result = result.OrderBy(x => x.Price).ToList();
                            break;

                        case "HighestPrice":
                            result = result.OrderByDescending(x => x.Price).ToList();
                            break;

                        case "BestSelling":
                            break;

                        case "MostReviews":
                            break;

                        case "NewestToOldest":
                            break;

                        default:
                            break;
                    }

                    ViewData["SortBy"] = sortBy;
                }

                var allFilters = category.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;

                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
