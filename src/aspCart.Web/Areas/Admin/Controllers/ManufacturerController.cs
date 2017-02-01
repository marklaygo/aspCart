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
    public class ManufacturerController : AdminController
    {
        #region Fields

        private readonly IManufacturerService _manufacturerService;
        private readonly IImageManagerService _imageManagerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ViewHelper _viewHelper;
        private readonly DataHelper _dataHelper;
        private readonly IMapper _mapper;

        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private string _sessionKey = "ManufacturerModel";

        #endregion

        #region Constructor

        public ManufacturerController(
            IManufacturerService manufacturerService,
            IImageManagerService imageMangerService,
            IHttpContextAccessor httpContextAccessor,
            ViewHelper viewHelper,
            DataHelper dataHelper,
            IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _imageManagerService = imageMangerService;
            _httpContextAccessor = httpContextAccessor;
            _viewHelper = viewHelper;
            _dataHelper = dataHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Manufacturer/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /Manufacturer/List
        public IActionResult List()
        {
            var manufacturerEntities = _manufacturerService.GetAllManufacturers();
            var manufacturerList = new List<ManufacturerListModel>();

            foreach (var manufacturer in manufacturerEntities)
            {
                var manufacturerModel = _mapper.Map<Manufacturer, ManufacturerListModel>(manufacturer);

                // get the image
                if (manufacturer.MainImage != null)
                {
                    var image = _imageManagerService.GetImageById(Guid.Parse(manufacturer.MainImage));
                    if (image != null)
                        manufacturerModel.MainImageFileName = image.FileName;
                }

                manufacturerList.Add(manufacturerModel);
            }

            return View(manufacturerList);
        }

        // GET: /Manufacturer/Create
        public IActionResult Create()
        {
            var model = new ManufacturerCreateOrUpdateModel();
            return View(model);
        }

        // POST: /Manufacturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ManufacturerCreateOrUpdateModel model, bool continueEditing)
        {
            bool hasError = false;

            if (ModelState.IsValid)
            {
                // check if name exist
                if (_dataHelper.CheckForDuplicate(ServiceType.Manufacturer, DataType.Name, model.Name))
                {
                    ModelState.AddModelError(string.Empty, "Manufacturer name already exist");
                    hasError = true;
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Manufacturer, model.Name);
                }
                else
                {
                    // check if seo already exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Manufacturer, DataType.Seo, model.SeoUrl))
                    {
                        ModelState.AddModelError(string.Empty, "SEO Url already exist");
                        hasError = true;
                    }
                }

                // if everything is valid
                if (!hasError)
                {
                    // map model to entity
                    var manufacturerEntity = _mapper.Map<ManufacturerCreateOrUpdateModel, Manufacturer>(model);
                    manufacturerEntity.DateAdded = DateTime.Now;
                    manufacturerEntity.DateModified = DateTime.Now;

                    // save
                    _manufacturerService.InsertManufacturer(manufacturerEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = manufacturerEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            // something went wrong, redisplay form
            return View(model);
        }

        // GET: /Manufacturer/Edit
        public IActionResult Edit(Guid? id, string ActiveTab)
        {
            if (id == null)
                return RedirectToAction("List");

            // check manufacturer id exist
            var manufacturerEntity = _manufacturerService.GetManufacturerById(id ?? Guid.Empty);
            if (manufacturerEntity == null)
                return RedirectToAction("List");

            // map entity to view model
            var model = _mapper.Map<Manufacturer, ManufacturerCreateOrUpdateModel>(manufacturerEntity);
            model.ActiveTab = ActiveTab ?? model.ActiveTab;

            // get the image
            if (manufacturerEntity.MainImage != null)
            {
                var image = _imageManagerService.GetImageById(Guid.Parse(manufacturerEntity.MainImage));
                if (image != null)
                    model.MainImageFileName = image.FileName;
            }

            // add model to session
            Session.SetString(_sessionKey, JsonConvert.SerializeObject(model));

            return View(model);
        }

        // POST: /Manufacturer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ManufacturerCreateOrUpdateModel model, bool continueEditing)
        {
            bool hasError = false;

            if(ModelState.IsValid)
            {
                // get model from session
                var sessionModel = JsonConvert.DeserializeObject<ManufacturerCreateOrUpdateModel>(Session.GetString(_sessionKey));
                model.Id = sessionModel.Id;
                model.DateAdded = sessionModel.DateAdded;

                // check if user edit the name
                if (model.Name.ToLower() != sessionModel.Name.ToLower())
                {
                    // check if name exist
                    if (_dataHelper.CheckForDuplicate(ServiceType.Manufacturer, DataType.Name, model.Name))
                    {
                        ModelState.AddModelError(string.Empty, "Manufacturer name already exist");
                        hasError = true;
                    }
                }

                // create seo friendly url if the user didn't provide
                if (string.IsNullOrEmpty(model.SeoUrl))
                {
                    model.SeoUrl = _dataHelper.GenerateSeoFriendlyUrl(ServiceType.Manufacturer, model.Name);
                }
                else
                {
                    // check if user change seo url
                    if (model.SeoUrl.ToLower() != sessionModel.SeoUrl.ToLower())
                    {
                        // check if seo already exist
                        if (_dataHelper.CheckForDuplicate(ServiceType.Manufacturer, DataType.Seo, model.SeoUrl))
                        {
                            ModelState.AddModelError(string.Empty, "SEO Url already exist");
                            hasError = true;
                        }
                    }
                }

                // if everything works
                if (!hasError)
                {
                    // map model to entity
                    var manufacturerEntity = _mapper.Map<ManufacturerCreateOrUpdateModel, Manufacturer>(model);
                    manufacturerEntity.DateModified = DateTime.Now;

                    // save
                    _manufacturerService.UpdateManufacturer(manufacturerEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = manufacturerEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            // something went wrong, redisplay form
            return View(model);
        }

        // POST: /Manufacturer/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return RedirectToAction("List");

            _manufacturerService.DeleteManufacturers(ids);

            return RedirectToAction("List");
        }

        #endregion
    }
}
