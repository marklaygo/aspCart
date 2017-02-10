using aspCart.Core.Interface.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Helpers
{
    public enum ServiceType
    {
        Category,
        Manufacturer,
        Product,
        Specification
    }

    public enum DataType
    {
        Name,
        Seo
    }

    public class DataHelper
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly ISpecificationService _specificationService;

        #endregion

        #region Constructor

        public DataHelper(
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService,
            ISpecificationService specificationService)
        {
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _productService = productService;
            _specificationService = specificationService;
        }

        #endregion

        #region Methods

        public string GenerateSeoFriendlyUrl(ServiceType serviceType, string name, int counter = 0)
        {
            var entities = new List<string>();
            var seoFriendlyUrl = (counter == 0) ? name : name + " " + counter;

            seoFriendlyUrl = Regex.Replace(seoFriendlyUrl, @"[^a-zA-z0-9\s]", "");
            seoFriendlyUrl = Regex.Replace(seoFriendlyUrl, @"\s", "-");

            // get entities
            if (serviceType == ServiceType.Category)
            {
                entities = _categoryService.GetAllCategories()
                    .Select(x => x.SeoUrl.ToLower())
                    .ToList();
            }
            else if (serviceType == ServiceType.Manufacturer)
            {
                entities = _manufacturerService.GetAllManufacturers()
                    .Select(x => x.SeoUrl.ToLower())
                    .ToList();
            }
            else if(serviceType == ServiceType.Product)
            {
                entities = _productService.GetAllProducts()
                    .Select(x => x.SeoUrl.ToLower())
                    .ToList();
            }

            // check if seo already exist
            if (entities.Contains(seoFriendlyUrl.ToLower()))
            {
                if (counter == 0)
                    seoFriendlyUrl = GenerateSeoFriendlyUrl(serviceType, name, 2); // 2 will be concatenated at the name
                else
                    seoFriendlyUrl = GenerateSeoFriendlyUrl(serviceType, name, counter + 1);
            }


            return seoFriendlyUrl;
        }

        public bool CheckForDuplicate(ServiceType serviceType, DataType dataType, string value)
        {
            if (value == "")
                return true;

            var entities = new List<string>();

            if (dataType == DataType.Name)
            {
                if (serviceType == ServiceType.Category)
                {
                    entities = _categoryService.GetAllCategories()
                        .Select(x => x.Name.ToLower())
                        .ToList();
                }
                else if (serviceType == ServiceType.Manufacturer)
                {
                    entities = _manufacturerService.GetAllManufacturers()
                        .Select(x => x.Name.ToLower())
                        .ToList();
                }
                else if (serviceType == ServiceType.Product)
                {
                    entities = _productService.GetAllProducts()
                        .Select(x => x.Name.ToLower())
                        .ToList();
                }
                else if(serviceType == ServiceType.Specification)
                {
                    entities = _specificationService.GetAllSpecifications()
                        .Select(x => x.Name.ToLower())
                        .ToList();
                }
            }
            else if (dataType == DataType.Seo)
            {
                if (serviceType == ServiceType.Category)
                {
                    entities = _categoryService.GetAllCategories()
                        .Select(x => x.SeoUrl.ToLower())
                        .ToList();
                }
                else if (serviceType == ServiceType.Manufacturer)
                {
                    entities = _manufacturerService.GetAllManufacturers()
                        .Select(x => x.SeoUrl.ToLower())
                        .ToList();
                }
                else if (serviceType == ServiceType.Product)
                {
                    entities = _productService.GetAllProducts()
                        .Select(x => x.SeoUrl.ToLower())
                        .ToList();
                }
            }

            // check for duplicate
            if (!entities.Contains(value.ToLower()))
                return false;

            // value exist
            return true;
        }

        #endregion
    }
}
