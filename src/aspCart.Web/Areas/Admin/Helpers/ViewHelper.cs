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

        #endregion


        #region Constructor

        public ViewHelper(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
                if (category.Id != idToExclude)
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

        #endregion
    }
}
