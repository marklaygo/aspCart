using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.ViewComponents
{
    [ViewComponent(Name = "Filter")]
    public class FilterViewComponent : ViewComponent
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public FilterViewComponent(
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _productService = productService;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string nameFilter = "", string categoryFilter = "", string manufacturerFilter = "" )
        {
            //
            // category filter
            //

            var categoryFilterViewModel = new List<CategoryFilterViewModel>();

            var allCategories = _categoryService.GetAllCategoriesWithoutParent();

            foreach(var category in allCategories)
            {
                var filterModel = new CategoryFilterViewModel { Name = category.Name, Id = category.Id };
                var qty = 0;

                if(nameFilter.Length > 0)
                {
                    qty = filterModel.Quantity = _productService.Table()
                        .Include(x => x.Categories).ThenInclude(x => x.Category)
                        .Where(x => x.Name.ToLower().Contains(nameFilter.ToLower()))
                        .Where(x => x.Categories.Any(c => c.Category.SeoUrl.ToLower() == category.SeoUrl.ToLower()))
                        .Where(x => x.Published == true)
                        .AsNoTracking()
                        .Count();
                }
                if(manufacturerFilter.Length > 0)
                {
                    qty = filterModel.Quantity = _productService.Table()
                        .Include(x => x.Categories).ThenInclude(x => x.Category)
                        .Include(x => x.Manufacturers).ThenInclude(x => x.Manufacturer)
                        .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name.ToLower() == manufacturerFilter.ToLower()))
                        .Where(x => x.Categories.Any(c => c.Category.SeoUrl.ToLower() == category.SeoUrl.ToLower()))
                        .Where(x => x.Published == true)
                        .AsNoTracking()
                        .Count();
                }

                if (qty > 0) { categoryFilterViewModel.Add(filterModel); }
            }

            //
            // manufacturer filter
            //

            var manufacturerFilterViewModel = new List<ManufacturerFilterViewModel>();

            var allManufacturer = _manufacturerService.GetAllManufacturers();

            foreach(var manufacturer in allManufacturer)
            {
                var filterModel = new ManufacturerFilterViewModel { Name = manufacturer.Name, Id = manufacturer.Id };
                var qty = 0;

                qty = filterModel.Quantity = _productService.Table()
                    .Include(x => x.Categories).ThenInclude(x => x.Category)
                    .Include(x => x.Manufacturers).ThenInclude(x => x.Manufacturer)
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name.ToLower() == manufacturer.Name.ToLower()))
                    .Where(x => x.Categories.Any(c => c.Category.Name.ToLower() == categoryFilter.ToLower()))
                    .Where(x => x.Published == true)
                    .AsNoTracking()
                    .Count();

                if (qty > 0) { manufacturerFilterViewModel.Add(filterModel); }
            }

            //
            // price filter
            //

            var allProducts = _productService.Table()
                .Include(x => x.Categories).ThenInclude(x => x.Category)
                .Include(x => x.Manufacturers).ThenInclude(x => x.Manufacturer)
                .Where( x=> x.Published  == true)
                .AsNoTracking()
                .ToList();

            bool showPrice = false;

            if(nameFilter.Length > 0)
            {
                allProducts = allProducts
                    .Where(x => x.Name.ToLower().Contains(nameFilter.ToLower()))
                    .ToList();

                showPrice = true;
            }
            if(manufacturerFilter.Length > 0)
            {
                allProducts = allProducts
                    .Where(x => x.Manufacturers.Any(m => m.Manufacturer.Name.ToLower() == manufacturerFilter.ToLower()))
                    .ToList();

                showPrice = true;
            }
            if (categoryFilter.Length > 0)
            {
                allProducts = allProducts
                    .Where(x => x.Categories.Any(m => m.Category.Name.ToLower() == categoryFilter.ToLower()))
                    .ToList();

                showPrice = true;
            }

            List<int> range = new List<int>();
            List<IGrouping<int, decimal>> groupings = new List<IGrouping<int, decimal>>();
            List<decimal> allPrices = allProducts.Select(x => x.Price).ToList();

            if (showPrice)
            {
                range = new List<int> { 0, 100, 250, 500, 750, 1000, 1500, 2000, 3000, 4000, 5000, 10000 };
                groupings = allPrices.GroupBy(price => range.FirstOrDefault(r => r >= price)).ToList();
            }
           
            //
            // result
            //

            var result = new FilterViewModel();
            result.CategoryFilterViewModel = categoryFilterViewModel;
            result.ManufacturerFilterViewModel = manufacturerFilterViewModel;
            result.PriceGroupings = groupings.OrderBy(x => x.Key);
            result.PriceRange = range;

            if (nameFilter.Length > 0)
            {
                result.FilterType = "name";
                ViewData["name"] = nameFilter;
            }

            if (manufacturerFilter.Length > 0) { result.FilterType = "manufacturer"; }

            if (categoryFilter.Length > 0) { result.FilterType = "category"; }

            return View(result);
        }

        #endregion
    }
}
