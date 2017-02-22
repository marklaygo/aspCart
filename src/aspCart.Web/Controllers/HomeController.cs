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

                    // get image
                    if(productEntity.Images.Count > 0)
                    {
                        productModel.MainImage = productEntity.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image.FileName;
                    }

                    // get all specifications
                    var specifications = productEntity.Specifications.OrderBy(x => x.SortOrder).ThenBy(x => x.Position);
                    foreach(var specification in specifications)
                    {
                        productModel.Specifications.Add(new SpecificationModel
                        {
                            Key = specification.Specification.Name,
                            Value = specification.Value,
                            SortOrder = specification.SortOrder
                        });
                    }

                    return View(productModel);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductSearch
        public IActionResult ProductSearch(string name)
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

                    productList.Add(productModel);
                }

                return View(productList);
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
