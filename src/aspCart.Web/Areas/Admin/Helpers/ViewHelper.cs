using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Helpers
{
    public class ViewHelper
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISpecificationService _specificationService;

        #endregion

        #region Constructor

        public ViewHelper(
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ISpecificationService specificationService)
        {
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _specificationService = specificationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get category parent mapping
        /// </summary>
        /// <param name="categoryId">Category id</param>
        /// <returns>Category parent mapping</returns>
        public string GetCategoryParentMapping(Guid categoryId)
        {
            var mapping = "";
            var haveParent = true;
            var categories = _categoryService.GetAllCategories();

            while (haveParent)
            {
                // get parent category
                var category = categories.FirstOrDefault(x => x.Id == categoryId);
                if (category != null)
                {
                    // add parent name to mapping
                    mapping = mapping.Insert(0, category.Name + " >> ");

                    // check if the parent category have parent
                    if (category.ParentCategoryId != Guid.Empty)
                        categoryId = category.ParentCategoryId;
                    else
                        haveParent = false;
                }
                else
                    haveParent = false;
            }

            return mapping;
        }

        /// <summary>
        /// Get parent category select list
        /// </summary>
        /// <returns>Select list parent category</returns>
        public SelectList GetParentCategorySelectList(Guid idToExclude = default(Guid))
        {
            var categories = _categoryService.GetAllCategories();
            var categorySelectList = new List<CategorySelectList>();
            var root = new CategorySelectList
            {
                Text = "None",
                Value = Guid.Empty.ToString()
            };
            categorySelectList.Add(root);

            foreach (var category in categories)
            {
                // skip id to exclude
                if (category.Id == idToExclude)
                    continue;

                var categoryModel = new CategorySelectList
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                };

                if (category.ParentCategoryId != Guid.Empty)
                    categoryModel.Text = GetCategoryParentMapping(category.ParentCategoryId) + category.Name;

                categorySelectList.Add(categoryModel);
            }

            var selectList = new SelectList(categorySelectList.OrderBy(x => x.Text), "Value", "Text");
            return selectList;
        }

        /// <summary>
        /// Get category select list
        /// </summary>
        /// <returns>Select list category</returns>
        public SelectList GetCategorySelectList()
        {
            var categories = _categoryService.GetAllCategories();
            var categorySelectList = new List<CategorySelectList>();

            foreach (var category in categories)
            {
                var categoryModel = new CategorySelectList
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                };

                if (category.ParentCategoryId != Guid.Empty)
                    categoryModel.Text = GetCategoryParentMapping(category.ParentCategoryId) + category.Name;

                categorySelectList.Add(categoryModel);
            }

            var selectList = new SelectList(categorySelectList.OrderBy(x => x.Text), "Value", "Text");
            return selectList;
        }

        /// <summary>
        /// Get manufacturer select list
        /// </summary>
        /// <returns>Select list manufacturer</returns>
        public SelectList GetManufacturerSelectList()
        {
            var manufacturers = _manufacturerService.GetAllManufacturers();
            var manufacturerSelectList = new List<ManufacturerSelectList>();

            foreach (var manufacturer in manufacturers)
            {
                var manufacturerModel = new ManufacturerSelectList
                {
                    Text = manufacturer.Name,
                    Value = manufacturer.Id.ToString()
                };

                manufacturerSelectList.Add(manufacturerModel);
            }

            var selectList = new SelectList(manufacturerSelectList.OrderBy(x => x.Text), "Value", "Text");
            return selectList;
        }

        /// <summary>
        /// Get specification key select list
        /// </summary>
        /// <returns>Select list specification key</returns>
        public SelectList GetSpecificationKeySelectList()
        {
            var specifications = _specificationService.GetAllSpecifications();
            var specificationList = new List<SpecificationKeySelectList>();

            foreach(var specification in specifications)
            {
                var specificationModel = new SpecificationKeySelectList
                {
                    Text = specification.Name,
                    Value = specification.Id.ToString()
                };

                specificationList.Add(specificationModel);
            }

            var selectList = new SelectList(specificationList.OrderBy(x => x.Text), "Value", "Text");
            return selectList;
        }

        #endregion
    }
}
