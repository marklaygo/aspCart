using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Models;
using AutoMapper;
using aspCart.Core.Domain.Catalog;
using aspCart.Infrastructure.EFModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using aspCart.Core.Interface.Services.Sale;

namespace aspCart.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService,
            IProductService productService,
            IReviewService reviewService,
            IMapper mapper)
        {
            _userManager = userManager;
            _orderService = orderService;
            _productService = productService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Home/
        public IActionResult Index()
        {
            var productEntities = _productService.GetAllProducts()
                .Where(x => x.Published == true);
            var productList = new List<ProductModel>();

            foreach(var product in productEntities)
            {
                var productModel = _mapper.Map<Product, ProductModel>(product);

                // get main image
                if(product.Images.Count > 0)
                {
                    productModel.MainImage = product.Images
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Position)
                        .FirstOrDefault()
                        .Image
                        .FileName;
                }

                // check for discount
                if(product.SpecialPriceEndDate != null && product.SpecialPriceEndDate >= DateTime.Now)
                {
                    productModel.OldPrice = product.Price;
                    productModel.Price = product.SpecialPrice ?? productModel.OldPrice;
                }

                //get product rating
                var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                if (reviews != null && reviews.Count > 0)
                {
                    productModel.Rating = reviews.Sum(x => x.Rating);
                    productModel.Rating /= reviews.Count;
                }

                productList.Add(productModel);
            }

            return View(productList);
        }

        // GET: /Home/ProductInfo ?? /Product/{seo}
        public IActionResult ProductInfo(string seo)
        {
            if(seo != null)
            {
                var productEntity = _productService.GetProductBySeo(seo);
                if(productEntity != null)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(productEntity);
                    productModel.Description = System.Net.WebUtility.HtmlDecode(productModel.Description);

                    // check for discount
                    if (productEntity.SpecialPriceEndDate != null && productEntity.SpecialPriceEndDate >= DateTime.Now)
                    {
                        productModel.OldPrice = productEntity.Price;
                        productModel.Price = productEntity.SpecialPrice ?? productModel.OldPrice;
                    }

                    // get all images
                    if (productEntity.Images.Count > 0)
                    {
                        productModel.MainImage = productEntity.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image.FileName;

                        productModel.ProductImages = productEntity.Images.Select(x => x.Image.FileName).ToList();
                    }

                    // get all manufacturers
                    var manufacturers = productEntity.Manufacturers;
                    foreach(var manufacturer in manufacturers)
                    {
                        productModel.Manufacturers.Add(new ManufacturerModel
                        {
                            Name = manufacturer.Manufacturer.Name,
                            SeoUrl = manufacturer.Manufacturer.SeoUrl
                        });
                    }

                    // get all specifications
                    var specifications = productEntity.Specifications.OrderBy(x => x.SortOrder).ThenBy(x => x.Position);
                    foreach(var specification in specifications)
                    {
                        productModel.Specifications.Add(new SpecificationModel
                        {
                            Key = specification.Specification.Name,
                            Value = System.Net.WebUtility.HtmlDecode(specification.Value),
                            SortOrder = specification.SortOrder
                        });
                    }

                    //get product rating
                    var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                    if (reviews != null && reviews.Count > 0)
                    {
                        productModel.Rating = reviews.Sum(x => x.Rating);
                        productModel.Rating /= reviews.Count;
                        productModel.ReviewCount = reviews.Count;
                    }

                    ViewData["ProductId"] = productModel.Id;
                    if(User.Identity.IsAuthenticated)
                        ViewData["ProductReviewer"] = _reviewService.GetReviewByProductIdUserId(productModel.Id, GetCurrentUserId()) != null ? true : false;

                    return View(productModel);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductCategory
        public IActionResult ProductCategory(string category, string sortBy, [FromQuery] string[] manufacturer, [FromQuery] string[] price)
        {
            if (category != null)
            {
                var productEntities = _productService.SearchProduct(
                    categoryFilter: new string[] { category },
                    manufacturerFilter: manufacturer,
                    priceFilter: price);

                var productList = new List<ProductModel>();

                foreach(var product in productEntities)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(product);

                    // get image
                    if (product.Images.Count > 0)
                    {
                        productModel.MainImage = product.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image.FileName;
                    }

                    // get manufacturer
                    if(product.Manufacturers.Count > 0)
                    {
                        foreach(var m in product.Manufacturers)
                        {
                            productModel.Manufacturers.Add(new ManufacturerModel { Name = m.Manufacturer.Name, SeoUrl = m.Manufacturer.SeoUrl });
                        }
                    }

                    //get product rating
                    var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                    if (reviews != null && reviews.Count > 0)
                    {
                        productModel.Rating = reviews.Sum(x => x.Rating);
                        productModel.Rating /= reviews.Count;
                        productModel.ReviewCount = reviews.Count;
                    }

                    productList.Add(productModel);
                }

                // sort result if the parameter is provided
                if (sortBy != null && sortBy.Length > 0)
                {
                    SortProductModel(sortBy, ref productList);
                }

                // get all filters to recheck all filters in view
                var allFilters = manufacturer.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;
                ViewData["Category"] = category;

                return View(productList);
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductManufacturer
        public IActionResult ProductManufacturer(string manufacturer, string sortBy, [FromQuery] string[] category, [FromQuery] string[] price)
        {
            if (manufacturer != null)
            {
                var productEntities = _productService.SearchProduct(
                    categoryFilter: category,
                    manufacturerFilter: new string[] { manufacturer },
                    priceFilter: price);

                var productList = new List<ProductModel>();

                foreach (var product in productEntities)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(product);

                    // get main image
                    if (product.Images.Count > 0)
                    {
                        productModel.MainImage = product.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image
                            .FileName;
                    }

                    // get all categories
                    foreach (var c in product.Categories)
                    {
                        productModel.Categories.Add(new CategoryModel { Name = c.Category.Name, SeoUrl = c.Category.SeoUrl });
                    }

                    //get product rating
                    var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                    if (reviews != null && reviews.Count > 0)
                    {
                        productModel.Rating = reviews.Sum(x => x.Rating);
                        productModel.Rating /= reviews.Count;
                        productModel.ReviewCount = reviews.Count;
                    }

                    productList.Add(productModel);
                }

                var result = productList;

                // filter the result using category parameter
                if (category.Length > 0)
                {
                    result = result.Where(x => x.Categories.Select(c => c.Name).Intersect(category).Count() > 0).ToList();
                }

                // filter the result using price parameter
                if (price.Length > 0)
                {
                    var tmpResult = new List<ProductModel>();
                    foreach (var p in price)
                    {
                        var tmpPrice = p.Split(new char[] { '-' });
                        int minPrice = Convert.ToInt32(tmpPrice[0]);
                        int maxPrice = Convert.ToInt32(tmpPrice[1]);

                        var r = result.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();

                        if (r.Count > 0) { tmpResult.AddRange(r); }
                    }
                    result = tmpResult;
                }

                // sort result if the parameter is provided
                if (sortBy != null && sortBy.Length > 0)
                {
                    SortProductModel(sortBy, ref result);
                }

                // get all filters to recheck all filters in view
                var allFilters = category.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;
                ViewData["Manufacturer"] = manufacturer;

                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Home/ProductSearch
        public IActionResult ProductSearch(string name, string sortBy, [FromQuery] string[] category, [FromQuery] string[] price)
        {
            if(name != string.Empty && name != null)
            {
                var searchResult = _productService.SearchProduct(
                    nameFilter: name,
                    categoryFilter: category,
                    priceFilter: price);

                var productList = new List<ProductModel>();

                foreach (var product in searchResult)
                {
                    var productModel = _mapper.Map<Product, ProductModel>(product);

                    // get main image
                    if (product.Images.Count > 0)
                    {
                        productModel.MainImage = product.Images
                            .OrderBy(x => x.SortOrder)
                            .ThenBy(x => x.Position)
                            .FirstOrDefault()
                            .Image
                            .FileName;
                    }

                    // get all categories
                    foreach (var c in product.Categories)
                    {
                        productModel.Categories.Add(new CategoryModel { Name = c.Category.Name, SeoUrl = c.Category.SeoUrl });
                    }

                    //get product rating
                    var reviews = _reviewService.GetReviewsByProductId(productModel.Id);
                    if (reviews != null && reviews.Count > 0)
                    {
                        productModel.Rating = reviews.Sum(x => x.Rating);
                        productModel.Rating /= reviews.Count;
                        productModel.ReviewCount = reviews.Count;
                    }

                    productList.Add(productModel);
                }

                // sort result if the parameter is provided
                if (sortBy != null && sortBy.Length > 0)
                {
                    SortProductModel(sortBy, ref productList);
                }

                // get all filters to recheck all filters in view
                var allFilters = category.Concat(price).ToList();
                ViewData["SortKey"] = allFilters;
                ViewData["ProductSearchName"] = name;

                return View(productList);
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/ProductReview
        public async Task<IActionResult> ProductReview(string id)
        {
            var model = new List<ReviewModel>();
            if (id != null && id.Length > 0)
            {
                Guid result = Guid.Empty;

                if (Guid.TryParse(id, out result))
                {
                    var reviewEntities = _reviewService.GetReviewsByProductId(result);
                    if (reviewEntities != null && reviewEntities.Count > 0)
                    {
                        foreach (var review in reviewEntities)
                        {
                            var r = _mapper.Map<Review, ReviewModel>(review);

                            var user = await _userManager.FindByIdAsync(review.UserId.ToString());
                            r.Username = await _userManager.GetUserNameAsync(user);

                            r.IsVerifiedOwner = _orderService.GetAllOrdersByUserId(review.UserId)
                                .Where(x => x.Items.Any(a => a.ProductId == result.ToString()) && x.Status == Core.Domain.Sale.OrderStatus.Complete)
                                .Count() > 0;

                            model.Add(r);
                        }
                    }
                }
            }

            return Json(model.OrderByDescending(x => x.CreatedOn));
        }

        // GET Home/ProductReview
        [Authorize]
        public IActionResult CreateReview(string id)
        {
            if(id != null && id.Length > 0)
            {
                Guid result = Guid.Empty;
                var model = new CreateReviewModel();

                if(Guid.TryParse(id, out result))
                {
                    var productEntity = _productService.GetProductById(result);
                    model.ProductId = productEntity.Id;
                    model.ProductName = productEntity.Name;
                    model.ProductSeo = productEntity.SeoUrl;
                    model.Rating = 1;

                    var userReview = _reviewService.GetReviewByProductIdUserId(result, GetCurrentUserId());
                    if(userReview != null)
                    {
                        model.Title = userReview.Title;
                        model.Message = userReview.Message;
                        model.Rating = userReview.Rating;
                        ViewData["ReviewEdited"] = true;
                    }
                }

                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReview(CreateReviewModel model)
        {
            if(ModelState.IsValid)
            {
                var reviewEntity = new Review
                {
                    Id = Guid.NewGuid(),
                    UserId = GetCurrentUserId(),
                    ProductId = model.ProductId,
                    Title = model.Title,
                    Message = model.Message,
                    Rating = model.Rating,
                    CreatedOn = DateTime.Now
                };

                _reviewService.InsertReview(reviewEntity);

                return new RedirectResult("/Product/" + model.ProductSeo + "#!#reviews");
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReview(CreateReviewModel model)
        {
            if (ModelState.IsValid)
            {
                var reviewEntity = _reviewService.GetReviewByProductIdUserId(model.ProductId, GetCurrentUserId());
                if(reviewEntity != null)
                {
                    reviewEntity.Title = model.Title;
                    reviewEntity.Message = model.Message;
                    reviewEntity.Rating = model.Rating;
                    reviewEntity.DateModified = DateTime.Now;

                    _reviewService.UpdateReview(reviewEntity);
                }

                return new RedirectResult("/Product/" + model.ProductSeo + "#!#reviews");
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Helper

        private Guid GetCurrentUserId()
        {
            var id = Guid.Empty;
            
            if(Guid.TryParse(_userManager.GetUserId(HttpContext.User) ?? "", out id))
            {
                return id;
            }

            return id;
        }

        private void SortProductModel(string sortBy, ref List<ProductModel> model)
        {
            switch (sortBy)
            {
                case "LowestPrice":
                    model = model.OrderBy(x => x.Price)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "HighestPrice":
                    model = model.OrderByDescending(x => x.Price)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "BestSelling":
                    break;

                case "MostReviews":
                    model = model.OrderByDescending(x => x.ReviewCount)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                case "NewestToOldest":
                    model = model.OrderByDescending(x => x.DateAdded)
                        .ThenBy(x => x.Name)
                        .ToList();
                    break;

                default:
                    break;
            }

            ViewData["SortBy"] = sortBy;
        }

        #endregion
    }
}
