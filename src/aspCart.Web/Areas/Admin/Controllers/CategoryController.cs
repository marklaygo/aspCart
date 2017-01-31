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
        private readonly DataHelper _dataHelper;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CategoryController(
            ICategoryService categoryService,
            ViewHelper viewHelper,
            DataHelper dataHelper,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _viewHelper = viewHelper;
            _dataHelper = dataHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Category/
        public IActionResult Index()
        {
            return RedirectToAction("List");
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

        // GET: /Category/Create
        public IActionResult Create()
        {
            var model = new CategoryCreateOrUpdateModel();
            model.ParentCategorySelectList = _viewHelper.GetParentCategorySelectList();

            return View(model);
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryCreateOrUpdateModel model, bool continueEditing)
        {
            var hasError = false;

            if(ModelState.IsValid)
            {
                // check if name exist
                if (_dataHelper.CheckForDuplicate(ServiceType.Category, DataType.Name, model.Name))
                {
                    ModelState.AddModelError(string.Empty, "Category name already exist");
                    hasError = true;
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Category, model.Name);
                }
                else
                {
                    // check if seo already exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Category, DataType.Seo, model.SeoUrl))
                    {
                        ModelState.AddModelError(string.Empty, "SEO Url already exist");
                        hasError = true;
                    }
                }

                // if everything works
                if (!hasError)
                {
                    // map model to entity
                    var categoryEntity = _mapper.Map<CategoryCreateOrUpdateModel, Category>(model);
                    categoryEntity.DateAdded = DateTime.Now;
                    categoryEntity.DateModified = DateTime.Now;

                    // save
                    _categoryService.InsertCategory(categoryEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = categoryEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }
            
            // something went wrong, redisplay form
            model.ParentCategorySelectList = _viewHelper.GetParentCategorySelectList();
            return View(model);
        }

        // GET: /Category/Edit
        public IActionResult Edit(Guid? id, string ActiveTab)
        {
            if (id == null)
                return RedirectToAction("List");

            // check category id exist
            var categoryEntity = _categoryService.GetCategoryById(id ?? Guid.Empty);
            if (categoryEntity == null)
                return RedirectToAction("List");

            // map entity to model
            var model = _mapper.Map<Category, CategoryCreateOrUpdateModel>(categoryEntity);
            model.ParentCategorySelectList = _viewHelper.GetParentCategorySelectList(model.Id);
            model.ActiveTab = ActiveTab ?? model.ActiveTab;

            return View(model);
        }

        // Post: /Category/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryCreateOrUpdateModel model, bool continueEditing)
        {
            var hasError = false;

            if(ModelState.IsValid)
            {
                // check if name exist
                if (_dataHelper.CheckForDuplicate(ServiceType.Category, DataType.Name, model.Name))
                {
                    ModelState.AddModelError(string.Empty, "Category name already exist");
                    hasError = true;
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Category, model.Name);
                }
                else
                {
                    // check if seo already exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Category, DataType.Seo, model.SeoUrl))
                    {
                        ModelState.AddModelError(string.Empty, "SEO Url already exist");
                        hasError = true;
                    }
                }

                // if everything works
                if (!hasError)
                {
                    // map model to entity
                    var categoryEntity = _mapper.Map<CategoryCreateOrUpdateModel, Category>(model);
                    categoryEntity.DateModified = DateTime.Now;

                    // save
                    _categoryService.UpdateCategory(categoryEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = categoryEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            // something went wrong, redisplay form
            model.ParentCategorySelectList = _viewHelper.GetParentCategorySelectList();
            return View(model);
        }

        // Post: /Category/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return RedirectToAction("List");

            _categoryService.DeleteCategories(ids);

            return RedirectToAction("List");
        }

        #endregion
    }
}
