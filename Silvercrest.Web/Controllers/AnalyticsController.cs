using Microsoft.AspNet.Identity;
using Silvercrest.Interfaces.CommonServices;
using System.Web.Mvc;

namespace Silvercrest.Web.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public void UpdateTime()
        {
            var loginRecordId = Request.Cookies.Get("LoginRecord")?.Value;
            _analyticsService.UpdateLoggedInTime(loginRecordId);
        }
    }
}