using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Catalog;
using AutoMapper;
using aspCart.Web.Areas.Admin.Models.Catalog;
using aspCart.Core.Domain.Catalog;
using aspCart.Web.Areas.Admin.Helpers;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly ViewHelper _viewHelper; 
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CategoryController(
            ICategoryService categoryService,
            ViewHelper _viewHelper,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Category/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Category/List
        public IActionResult List()
        {
            var categoryEntities = _categoryService.GetAllCategories();
            var categoryList = new List<CategoryListModel>();

            foreach(var category in categoryEntities)
            {
                var categoryModel = _mapper.Map<Category, CategoryListModel>(category);

                // check if category have parent category
                if (category.ParentCategoryId != Guid.Empty)
                    categoryModel.NameWithParent = _viewHelper.GetCategoryParentMapping(category.ParentCategoryId);

                categoryModel.NameWithParent += category.Name;
                categoryList.Add(categoryModel);
            }

            return View(categoryList);
        }

        #endregion
    }
}
