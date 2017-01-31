using aspCart.Core.Interface.Services.Catalog;
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

        #endregion
    }
}
