using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.ViewComponents
{
    [ViewComponent(Name = "Filter")]
    public class FilterViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;

        public FilterViewComponent(
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _productService = productService;
        }

        public IViewComponentResult Invoke(string name = "", string manufacturer = "")
        {
            // category filter
            var categoryFilterViewModel = new List<CategoryFilterViewModel>();

            var allCategories = _categoryService.GetAllCategoriesWithoutParent();

            foreach(var category in allCategories)
            {
                var filterModel = new CategoryFilterViewModel { Name = category.Name, Id = category.Id };
                var qty = 0;

                if(name.Length > 0)
                {
                    qty = filterModel.Quantity = _productService.GetAllProducts()
                        .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                        .Where(x => x.Categories.Any(c => c.Category.SeoUrl.ToLower() == category.SeoUrl.ToLower()))
                        .Where(x => x.Published == true)
                        .Count();
                }
                if(manufacturer.Length > 0)
                {
                    qty = filterModel.Quantity = _productService.GetAllProducts()
                        .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name.ToLower() == manufacturer.ToLower()))
                        .Where(x => x.Categories.Any(c => c.Category.SeoUrl.ToLower() == category.SeoUrl.ToLower()))
                        .Where(x => x.Published == true)
                        .Count();
                }

                if (qty > 0) { categoryFilterViewModel.Add(filterModel); }
            }


            // price filter
            var allProducts = _productService.GetAllProducts();

            if(name.Length > 0)
            {
                allProducts = allProducts
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .Where(x => x.Published == true)
                    .ToList();
            }
            if(manufacturer.Length > 0)
            {
                allProducts = allProducts
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name.ToLower() == manufacturer.ToLower()))
                    .Where(x => x.Published == true)
                    .ToList();
            }

            List<decimal> allPrices = allProducts.Select(x => x.Price).ToList();

            var range = new[] { 0, 100, 250, 500, 750, 1000, 1500, 2000, 3000, 4000, 5000, 10000 };
            var groupings = allPrices.GroupBy(price => range.FirstOrDefault(r => r >= price));


            // result
            var result = new FilterViewModel();
            result.CategoryFilterViewModel = categoryFilterViewModel;
            result.PriceGroupings = groupings.OrderBy(x => x.Key);
            result.PriceRange = range;

            if (name.Length > 0)
            {
                result.FilterType = "name";
                ViewData["name"] = name;
            }
            if (manufacturer.Length > 0) { result.FilterType = "manufacturer"; }

            return View(result);
        }
    }
}
