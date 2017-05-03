using aspCart.Core.Domain.Statistics;
using aspCart.Core.Interface.Services.Statistics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Middleware
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IVisitorCountService _visitorCounterService;
        private readonly ILogger _logger;

        public VisitorCounterMiddleware(
            RequestDelegate next,
            IVisitorCountService visitorCounterService,
            ILoggerFactory logger)
        {
            _next = next;
            _visitorCounterService = visitorCounterService;
            _logger = logger.CreateLogger<VisitorCounterMiddleware>();
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Session.GetString("visitor_counter") == null || context.Session.GetString("visitor_counter") != "recorder")
            {
                context.Session.SetString("visitor_counter", "recorder");
                var visitorCountEntity = _visitorCounterService.GetVisitorCountByDate(DateTime.Now);
                if (visitorCountEntity != null)
                {
                    _visitorCounterService.UpdateVisitorCount(visitorCountEntity);
                }
                else
                {
                    var visitorModel = new VisitorCount
                    {
                        Date = DateTime.Now,
                        ViewCount = 1
                    };
                    _visitorCounterService.InsertVisitorCount(visitorModel);
                }
            }

            return _next(context);
        }
    }

    public static class VisitorCounterMiddlewareExtentions
    {
        public static IApplicationBuilder UseVisitorCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitorCounterMiddleware>();
        }
    }
}
