using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Catalog;
using Microsoft.AspNetCore.Http;
using aspCart.Web.Areas.Admin.Helpers;
using AutoMapper;
using aspCart.Web.Areas.Admin.Models.Catalog;
using aspCart.Core.Domain.Catalog;
using Newtonsoft.Json;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IImageManagerService _imageManagerService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly ISpecificationService _specificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ViewHelper _viewHelper;
        private readonly DataHelper _dataHelper;
        private readonly IMapper _mapper;

        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private string _sessionKey = "ProductModel";

        #endregion

        #region Constructor

        public ProductController(
            ICategoryService categoryService,
            IImageManagerService imageManagerService,
            IManufacturerService manufacturerService,
            IProductService productService,
            ISpecificationService specificationService,
            IHttpContextAccessor httpContextAccessor,
            ViewHelper viewHelper,
            DataHelper dataHelper,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _imageManagerService = imageManagerService;
            _manufacturerService = manufacturerService;
            _productService = productService;
            _specificationService = specificationService;
            _httpContextAccessor = httpContextAccessor;
            _viewHelper = viewHelper;
            _dataHelper = dataHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Product/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /Product/List
        public IActionResult List()
        {
            var productEntities = _productService.GetAllProducts();
            var productList = new List<ProductListModel>();

            foreach(var product in productEntities)
            {
                var productModel = _mapper.Map<Product, ProductListModel>(product);

                if (product.Images.Count > 0)
                {
                    // get first image
                    productModel.MainImage = product.Images
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Position)
                        .FirstOrDefault()
                        .Image
                        .FileName;
                }

                productList.Add(productModel);
            }

            return View(productList);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            var model = new ProductCreateOrUpdateModel();
            model.CategorySelectList = _viewHelper.GetCategorySelectList();
            model.ManufacturerSelectList = _viewHelper.GetManufacturerSelectList();
            model.SpecificationKeySelectList = _viewHelper.GetSpecificationKeySelectList();
            return View(model);
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateOrUpdateModel model, bool continueEditing)
        {
            var hasError = false;

            if(ModelState.IsValid)
            {
                // check if name exist
                if (_dataHelper.CheckForDuplicate(ServiceType.Product, DataType.Name, model.Name))
                {
                    ModelState.AddModelError(string.Empty, "Product name already exist");
                    hasError = true;
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Product, model.Name);
                }
                else
                {
                    // check if seo already exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Product, DataType.Seo, model.SeoUrl))
                    {
                        ModelState.AddModelError(string.Empty, "SEO Url already exist");
                        hasError = true;
                    }
                }

                // if everything is valid
                if(!hasError)
                {
                    // generate new id for model
                    model.Id = Guid.NewGuid();

                    // map view model to entity
                    var productEntity = _mapper.Map<ProductCreateOrUpdateModel, Product>(model);
                    productEntity.DateAdded = DateTime.Now;
                    productEntity.DateModified = DateTime.Now;

                    // save to database
                    _productService.InsertProduct(productEntity);
                    SaveCategoryMappings(model);
                    SaveManufacturerMappings(model);
                    SaveImageMappings(model);
                    SaveSpecificationMappings(model);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = productEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            // something went wrong, redisplay form
            model.CategorySelectList = _viewHelper.GetCategorySelectList();
            model.ManufacturerSelectList = _viewHelper.GetManufacturerSelectList();
            model.SpecificationKeySelectList = _viewHelper.GetSpecificationKeySelectList();
            return View(model);
        }

        // GET: /Product/Edit
        public IActionResult Edit(Guid? id, string ActiveTab)
        {
            if (id == null)
                return RedirectToAction("List");

            // check product id exist
            var productEntity = _productService.GetProductById(id ?? Guid.Empty);
            if (productEntity == null)
                return RedirectToAction("List");

            // get all categories
            var categoryIds = new List<string>();
            foreach (var category in productEntity.Categories)
                categoryIds.Add(category.Category.Id.ToString());

            // get all manufacturers
            var manufacturerIds = new List<string>();
            foreach (var manufacturer in productEntity.Manufacturers)
                manufacturerIds.Add(manufacturer.Manufacturer.Id.ToString());

            // get all images
            var images = new List<ImageModel>();
            foreach (var image in productEntity.Images.OrderBy(x => x.Position))
            {
                var img = new ImageModel
                {
                    Id = image.Image.Id,
                    FileName = image.Image.FileName,
                    SortOrder = image.SortOrder
                };
                images.Add(img);
            }

            // get all specifications
            var specifications = new List<ProductSpecificationModel>();
            foreach(var specification in productEntity.Specifications.OrderBy(x => x.Position))
            {
                var spec = new ProductSpecificationModel
                {
                    Key = specification.SpecificationId.ToString(),
                    Value = System.Net.WebUtility.HtmlDecode(specification.Value),
                    SortOrder = specification.SortOrder
                };
                specifications.Add(spec);
            }

            // map entity to view model
            var model = _mapper.Map<Product, ProductCreateOrUpdateModel>(productEntity);
            model.Description = System.Net.WebUtility.HtmlDecode(model.Description);
            model.CategoryIds = categoryIds;
            model.ManufacturerIds = manufacturerIds;
            model.Images = images;
            model.Specifications = specifications;

            // view helper
            model.ActiveTab = ActiveTab ?? model.ActiveTab;
            model.CategorySelectList = _viewHelper.GetParentCategorySelectList();
            model.ManufacturerSelectList = _viewHelper.GetManufacturerSelectList();
            model.SpecificationKeySelectList = _viewHelper.GetSpecificationKeySelectList();

            // add model to session
            Session.SetString(_sessionKey, JsonConvert.SerializeObject(model));

            return View(model);
        }

        // POST: /Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductCreateOrUpdateModel model, bool continueEditing)
        {
            var hasError = false;

            if(ModelState.IsValid)
            {
                // get model from session
                var sessionModel = JsonConvert.DeserializeObject<ProductCreateOrUpdateModel>(Session.GetString(_sessionKey));
                model.Id = sessionModel.Id;
                model.DateAdded = sessionModel.DateAdded;

                // check if user edit the name
                if (model.Name.ToLower() != sessionModel.Name.ToLower())
                {
                    // check if name exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Product, DataType.Name, model.Name))
                    {
                        ModelState.AddModelError(string.Empty, "Product name already exist");
                        hasError = true;
                    }
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Product, model.Name);
                }
                else
                {
                    // check if user change seo url
                    if (model.SeoUrl.ToLower() != sessionModel.SeoUrl.ToLower())
                    {
                        // check if seo already exist
                        if (_dataHelper.CheckForDuplicate(ServiceType.Product, DataType.Seo, model.SeoUrl))
                        {
                            ModelState.AddModelError(string.Empty, "SEO Url already exist");
                            hasError = true;
                        }
                    }
                }

                // if everything is valid
                if (!hasError)
                {
                    // map view model to entity
                    var productEntity = _mapper.Map<ProductCreateOrUpdateModel, Product>(model);
                    productEntity.DateModified = DateTime.Now;

                    // save to database
                    _productService.UpdateProduct(productEntity);
                    SaveCategoryMappings(model);
                    SaveManufacturerMappings(model);
                    SaveImageMappings(model);
                    SaveSpecificationMappings(model);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = productEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            // something went wrong, redisplay form
            model.CategorySelectList = _viewHelper.GetCategorySelectList();
            model.ManufacturerSelectList = _viewHelper.GetManufacturerSelectList();
            model.SpecificationKeySelectList = _viewHelper.GetSpecificationKeySelectList();
            return View(model);
        }

        // POST: /Product/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return RedirectToAction("List");

            _productService.DeleteProducts(ids);

            return RedirectToAction("List");
        }

        #endregion

        #region Helpers

        [NonAction]
        private void SaveCategoryMappings(ProductCreateOrUpdateModel model)
        {
            var categoryMappings = new List<ProductCategoryMapping>();
            if(model.CategoryIds != null)
            {
                foreach(var id in model.CategoryIds)
                {
                    // check if category exist
                    Guid categoryId;
                    if(Guid.TryParse(id, out categoryId))
                    {
                        if(_categoryService.GetCategoryById(categoryId) != null)
                        {
                            // create mapping entity
                            var categoryMapping = new ProductCategoryMapping
                            {
                                Id = Guid.NewGuid(),
                                ProductId = model.Id,
                                CategoryId = Guid.Parse(id)
                            };

                            categoryMappings.Add(categoryMapping);
                        }
                    }
                }
            }

            // save to database
            _categoryService.DeleteAllProductCategoryMappingsByProductId(model.Id);
            _categoryService.InsertProductCategoryMappings(categoryMappings);
        }

        [NonAction]
        private void SaveImageMappings(ProductCreateOrUpdateModel model)
        {
            var imageMappings = new List<ProductImageMapping>();
            if (model.ImageIds != null)
            {
                for (int i = 0; i < model.ImageIds.Count; i++)
                {
                    // convert sort order to int
                    int sortOrder = Convert.ToInt32(Math.Floor(Convert.ToDouble(model.ImageSortOrder[i])));

                    // check if image exist
                    Guid imageId;
                    if(Guid.TryParse(model.ImageIds[i], out imageId))
                    {
                        // create mapping entity
                        var imageMapping = new ProductImageMapping
                        {
                            Id = Guid.NewGuid(),
                            ProductId = model.Id,
                            ImageId = Guid.Parse(model.ImageIds[i]),
                            SortOrder = sortOrder,
                            Position = i
                        };

                        imageMappings.Add(imageMapping);
                    } 
                }
            }

            // save to database
            _imageManagerService.DeleteAllProductImageMappings(model.Id);
            _imageManagerService.InsertProductImageMappings(imageMappings);
        }

        [NonAction]
        private void SaveManufacturerMappings(ProductCreateOrUpdateModel model)
        {
            var manufacturerMappings = new List<ProductManufacturerMapping>();
            if (model.ManufacturerIds != null)
            {
                foreach (var id in model.ManufacturerIds)
                {
                    // check if manufacture exist
                    Guid manufactureId;
                    if (Guid.TryParse(id, out manufactureId))
                    {
                        if (_manufacturerService.GetManufacturerById(manufactureId) != null)
                        {
                            // create mapping entity
                            var manufacturerMapping = new ProductManufacturerMapping
                            {
                                Id = Guid.NewGuid(),
                                ProductId = model.Id,
                                ManufacturerId = Guid.Parse(id)
                            };

                            manufacturerMappings.Add(manufacturerMapping);
                        }
                    }
                }
            }

            // save to database
            _manufacturerService.DeleteAllProductManufacturersMappings(model.Id);
            _manufacturerService.InsertProductManufacturerMappings(manufacturerMappings);
        }

        [NonAction]
        private void SaveSpecificationMappings(ProductCreateOrUpdateModel model)
        {
            var specificationMappings = new List<ProductSpecificationMapping>();
            if(model.Specifications != null)
            {
                int i = 0;
                foreach(var spec in model.Specifications)
                {
                    // check if specification exist
                    Guid specificationId;
                    if(Guid.TryParse(spec.Key, out specificationId))
                    {
                        if (specificationId == Guid.Empty)
                            continue;

                        // create mapping entity
                        var specificationMapping = new ProductSpecificationMapping
                        {
                            Id = Guid.NewGuid(),
                            ProductId = model.Id,
                            SpecificationId = specificationId,
                            Value = spec.Value,
                            SortOrder = spec.SortOrder,
                            Position = i
                        };
                        i++;

                        specificationMappings.Add(specificationMapping);
                    }
                }
            }

            // save to database
            _specificationService.DeleteAllProductSpecificationMappings(model.Id);
            _specificationService.InsertProductSpecificationMappings(specificationMappings);
        }

        #endregion
    }
}
