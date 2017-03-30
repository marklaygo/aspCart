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

        public IViewComponentResult Invoke(bool isManufacturerViewFilter, string manufacturer)
        {
            var model = new List<CategoryFilterViewModel>();

            if(isManufacturerViewFilter)
            {
                var allCategories = _categoryService.GetAllCategoriesWithoutParent().Where(x => x.Published);

                foreach(var category in allCategories)
                {
                    var filterModel = new CategoryFilterViewModel { Name = category.Name, Id = category.Id };
                    var qty = filterModel.Quantity = _productService.GetAllProducts()
                        .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name == manufacturer))
                        .Where(x => x.Categories.Any(c => c.Category.SeoUrl == category.SeoUrl))
                        .Count();

                    if (qty > 0) { model.Add(filterModel); }
                }
            }

            return View(model);
        }
    }
}
