using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspCart.Core.Interface.Services.Statistics;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class ChartController : AdminController
    {
        #region Fields

        private readonly IOrderCountService _orderCountService;
        private readonly IVisitorCountService _visitorCountService;

        #endregion

        #region Constructor

        public ChartController(
            IOrderCountService orderCountService,
            IVisitorCountService visitorCountService)
        {
            _orderCountService = orderCountService;
            _visitorCountService = visitorCountService;
        }

        #endregion

        #region Methods

        public IActionResult GetVisitorCount()
        {
            var results = _visitorCountService.GetAllVisitorCount(7)
                .OrderBy(x => x.Date);

            return Json(results);
        }

        public IActionResult GetOrderCount()
        {
            var results = _orderCountService.GetAllOrderCount(7)
                .OrderBy(x => x.Date);

            return Json(results);
        }

        #endregion
    }
}
