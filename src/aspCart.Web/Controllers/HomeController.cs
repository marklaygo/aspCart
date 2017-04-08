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

        // GET: /Home/ProductInfo ?? /Product/{seo}
        public IActionResult ProductInfo(string seo)
        {
            if(seo != null)
            {
                var productEntity = _productService.GetProductBySeo(seo);
                if(productEntity != null)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(productEntity);
                    productModel.Description = System.Net.WebUtility.HtmlDecode(productModel.Description);

                    // get image
                    if (productEntity.Images.Count > 0)
                    {
                        productModel.MainImage = productEntity.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image.FileName;
                    }

                    var manufacturers = productEntity.Manufacturers;
                    foreach(var manufacturer in manufacturers)
                    {
                        productModel.Manufacturers.Add(new ManufacturerModel
                        {
                            Name = manufacturer.Manufacturer.Name,
                            SeoUrl = manufacturer.Manufacturer.SeoUrl
                        });
                    }

                    // get all specifications
                    var specifications = productEntity.Specifications.OrderBy(x => x.SortOrder).ThenBy(x => x.Position);
                    foreach(var specification in specifications)
                    {
                        productModel.Specifications.Add(new SpecificationModel
                        {
                            Key = specification.Specification.Name,
                            Value = System.Net.WebUtility.HtmlDecode(specification.Value),
                            SortOrder = specification.SortOrder
                        });
                    }

                    ViewData["ProductId"] = productModel.Id;
                    return View(productModel);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductCategory
        public IActionResult ProductCategory([FromQuery] string[] manufacturer, [FromQuery] string[] price, string category)
        {
            if (category != null)
            {
                ViewData["Category"] = category;

                var productEntities = _productService.GetAllProducts()
                    .Where(x => x.Categories.Any(c => c.Category.Name.ToLower() == category.ToLower()))
                    .ToList();

                var productList = new List<ProductModel>();

                foreach(var product in productEntities)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(product);

                    // get image
                    if (product.Images.Count > 0)
                    {
                        productModel.MainImage = product.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image.FileName;
                    }

                    // get manufacturer
                    if(product.Manufacturers.Count > 0)
                    {
                        foreach(var m in product.Manufacturers)
                        {
                            productModel.Manufacturers.Add(new ManufacturerModel { Name = m.Manufacturer.Name, SeoUrl = m.Manufacturer.SeoUrl });
                        }
                    }

                    productList.Add(productModel);
                }

                var result = productList;

                if (manufacturer.Length > 0)
                {
                    result = result.Where(x => x.Manufacturers.Select(c => c.Name).Intersect(manufacturer).Count() > 0).ToList();
                }

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

                var allFilters = manufacturer.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;

                return View(result);
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductSearch
        public IActionResult ProductSearch([FromQuery] string[] category, [FromQuery] string[] price, string name)
        {
            if(name != string.Empty && name != null)
            {
                var searchResult = _productService.GetAllProducts()
                .Where(x =>
                    x.Name.ToLower().Contains(name.ToLower()) &&
                    x.Published == true);
                var productList = new List<ProductModel>();

                foreach (var product in searchResult)
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
                    foreach (var c in product.Categories)
                    {
                        productModel.Categories.Add(new CategoryModel { Name = c.Category.Name, SeoUrl = c.Category.SeoUrl });
                    }

                    productList.Add(productModel);
                }

                var result = productList;

                if (category.Length > 0)
                {
                    result = result.Where(x => x.Categories.Select(c => c.Name).Intersect(category).Count() > 0).ToList();
                }

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

                var allFilters = category.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;
                ViewData["ProductSearchName"] = name;

                return View(result);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion
    }
}
