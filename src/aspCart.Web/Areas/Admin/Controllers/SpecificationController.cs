using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Areas.Admin.Helpers;
using aspCart.Web.Areas.Admin.Models.Catalog;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class SpecificationController : AdminController
    {
        #region Fields

        private readonly ISpecificationService _specificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataHelper _dataHelper;
        private readonly IMapper _mapper;

        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private string _sessionKey = "SpecificationModel";

        #endregion

        #region Constructor

        public SpecificationController(
            ISpecificationService specificationService,
            IHttpContextAccessor httpContextAccessor,
            DataHelper dataHelper,
            IMapper mapper)
        {
            _specificationService = specificationService;
            _httpContextAccessor = httpContextAccessor;
            _dataHelper = dataHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        // GET: /Specification/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /Specification/List
        public IActionResult List()
        {
            var specificationEntities = _specificationService.GetAllSpecifications();
            var specificationList = new List<SpecificationListModel>();

            foreach (var entity in specificationEntities)
            {
                // map entity to view model
                var specification = _mapper.Map<Specification, SpecificationListModel>(entity);
                specificationList.Add(specification);
            }

            return View(specificationList);
        }

        // GET: /Specification/Create
        public IActionResult Create()
        {
            var model = new SpecificationCreateOrUpdateModel();
            return View(model);
        }

        // POST: /Specification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SpecificationCreateOrUpdateModel model, bool continueEditing)
        {
            var error = false;

            if (ModelState.IsValid)
            {
                // check if name exist
                if (_dataHelper.CheckForDuplicate(ServiceType.Specification, DataType.Name, model.Name))
                {
                    ModelState.AddModelError(string.Empty, "Specification name already exist");
                    error = true;
                }

                // if everything works
                if (!error)
                {
                    // map model to entity
                    var specificationEntity = _mapper.Map<SpecificationCreateOrUpdateModel, Specification>(model);
                    specificationEntity.DateAdded = DateTime.Now;
                    specificationEntity.DateModified = DateTime.Now;

                    // save
                    _specificationService.InsertSpecification(specificationEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = specificationEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            return View(model);
        }

        // GET: /Specification/Edit
        public IActionResult Edit(Guid? id, string ActiveTab)
        {
            if (id == null)
                return RedirectToAction("List");

            // check if product specification exist
            var specicationEntity = _specificationService.GetSpecificationById(id ?? Guid.Empty);
            if (specicationEntity == null)
                return RedirectToAction("List");

            // map entity to view model
            var model = _mapper.Map<Specification, SpecificationCreateOrUpdateModel>(specicationEntity);
            model.ActiveTab = ActiveTab ?? model.ActiveTab;

            // add model to session
            Session.SetString(_sessionKey, JsonConvert.SerializeObject(model));

            return View(model);
        }

        // POST: /Specification/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SpecificationCreateOrUpdateModel model, bool continueEditing)
        {
            var error = false;

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
                    if (_dataHelper.CheckForDuplicate(ServiceType.Specification, DataType.Name, model.Name))
                    {
                        ModelState.AddModelError(string.Empty, "Specification name already exist");
                        error = true;
                    }
                }

                // if everything works
                if (!error)
                {
                    // map model to entity
                    var specificationEntity = _mapper.Map<SpecificationCreateOrUpdateModel, Specification>(model);
                    specificationEntity.DateModified = DateTime.Now;

                    // save
                    _specificationService.UpdateSpecification(specificationEntity);

                    if (continueEditing)
                        return RedirectToAction("Edit", new { id = specificationEntity.Id, ActiveTab = model.ActiveTab });

                    return RedirectToAction("List");
                }
            }

            return View(model);
        }

        // POST: /Specification/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return RedirectToAction("List");

            _specificationService.DeleteSpecifications(ids);

            return RedirectToAction("List");
        }

        #endregion
    }
}
