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
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ManufacturerController(
            IProductService productService,
            IReviewService reviewService,
            IMapper mapper)
        {
            _productService = productService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public IActionResult ManufacturerInfo(string seo, string sortBy, [FromQuery] string[] category, [FromQuery] string[] price)
        {
            if(seo != null)
            {
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

                    //get product rating
                    var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                    if (reviews != null && reviews.Count > 0)
                    {
                        productModel.Rating = reviews.Sum(x => x.Rating);
                        productModel.Rating /= reviews.Count;
                        productModel.ReviewCount = reviews.Count;
                    }

                    productList.Add(productModel);
                }

                var result = productList;

                // filter the result using category parameter
                if (category.Length > 0)
                {
                    result = result.Where(x => x.Categories.Select(c => c.Name).Intersect(category).Count() > 0).ToList();
                }

                // filter the result using price parameter
                if (price.Length > 0)
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

                // sort result if the parameter is provided
                if (sortBy != null && sortBy.Length > 0)
                {
                    SortProductModel(sortBy, ref result);
                }

                // get all filters to recheck all filters in view
                var allFilters = category.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;
                ViewData["Manufacturer"] = seo;

                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helpers

        private void SortProductModel(string sortBy, ref List<ProductModel> model)
        {
            switch (sortBy)
            {
                case "LowestPrice":
                    model = model.OrderBy(x => x.Price)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "HighestPrice":
                    model = model.OrderByDescending(x => x.Price)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "BestSelling":
                    break;

                case "MostReviews":
                    model = model.OrderByDescending(x => x.ReviewCount)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "NewestToOldest":
                    model = model.OrderByDescending(x => x.DateAdded)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                default:
                    break;
            }

            ViewData["SortBy"] = sortBy;
        }

        #endregion
    }
}
