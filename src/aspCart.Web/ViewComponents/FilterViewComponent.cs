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

        public IViewComponentResult Invoke(string manufacturer)
        {
            // category filter
            var categoryFilterViewModel = new List<CategoryFilterViewModel>();

            var allCategories = _categoryService.GetAllCategoriesWithoutParent().Where(x => x.Published);

            foreach(var category in allCategories)
            {
                var filterModel = new CategoryFilterViewModel { Name = category.Name, Id = category.Id };
                var qty = filterModel.Quantity = _productService.GetAllProducts()
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name == manufacturer))
                    .Where(x => x.Categories.Any(c => c.Category.SeoUrl == category.SeoUrl))
                    .Count();

                if (qty > 0) { categoryFilterViewModel.Add(filterModel); }
            }


            // price filter
            var allProducts = _productService.GetAllProducts()
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name == manufacturer));

            List<decimal> allPrices = allProducts.Select(x => x.Price).ToList();

            var range = new[] { 0, 100, 250, 500, 750, 1000, 1500, 2000, 3000, 4000, 5000, 10000 };
            var groupings = allPrices.GroupBy(price => range.FirstOrDefault(r => r >= price));


            // result
            var result = new FilterViewModel();
            result.CategoryFilterViewModel = categoryFilterViewModel;
            result.PriceGroupings = groupings.OrderBy(x => x.Key);
            result.PriceRange = range;

            return View(result);
        }
    }
}
