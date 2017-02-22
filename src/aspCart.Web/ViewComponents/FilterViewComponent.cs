using aspCart.Core.Interface.Services.Catalog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.ViewComponents
{
    [ViewComponent(Name = "Filter")]
    public class FilterViewComponent : ViewComponent
    {
        private readonly IManufacturerService _manufacturerService;

        public FilterViewComponent(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
