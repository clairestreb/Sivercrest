using System.Web.Mvc;
using Silvercrest.Interfaces.TeamMember;
using System.Linq;
using Silvercrest.Web.Common;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;

namespace Silvercrest.Web.Areas.TeamMember.Controllers
{
    [Authorize]
    [Maintance]
    public class UserAnalyticsController : Controller
    {
        private IUserAnalyticsService _userAnalyticsService;

        public UserAnalyticsController(IUserAnalyticsService service)
        {
            _userAnalyticsService = service;
        }

        public ActionResult Index(string contactIdQuery,string familyIdQuery, string firmUserGroupIdQuery, string isFromQuery)
        {
            int? contactId, familyId;
            int? firmUserGroupId;
            bool? isFrom;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? int.Parse(Request.Cookies["mainContactId"].Value) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            familyId = string.IsNullOrEmpty(familyIdQuery) || familyIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(familyIdQuery));
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(firmUserGroupIdQuery));
            isFrom = string.IsNullOrEmpty(isFromQuery) || isFromQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isFromQuery));
            var userAnalytics = _userAnalyticsService.GetAnalytics((int)contactId, (int)familyId);
            userAnalytics.IsFromClientAnalytics = isFrom ?? false;
            userAnalytics.FirmUserGroupId = firmUserGroupId;
            userAnalytics.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));

            return View(userAnalytics);
        }

        public ActionResult LoginHistory(string webUserIdQuery,string familyIdQuery)
        {
            int? familyId;
            string webUserId;
            familyId = string.IsNullOrEmpty(familyIdQuery) || familyIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(familyIdQuery));
            webUserId = string.IsNullOrEmpty(webUserIdQuery) || webUserIdQuery == "undefined" ? "" : webUserIdQuery;
            var loginHistory = _userAnalyticsService.GetLoginHistoryViewModel(webUserId, (int)familyId);
            loginHistory.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(loginHistory);
        }

        public JsonResult GetLoginHistory(string webUserId)
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            var usaDateFormatInfo = culture.DateTimeFormat;
            var result = _userAnalyticsService.GetLoginHistory(webUserId).Select(lsr => new
            {
                LoginDate = new System.DateTime(lsr.LoginDate.Ticks, System.DateTimeKind.Utc).ToLocalTime().ToString("M/d/yyyy HH:mm", usaDateFormatInfo),
                LogoutDate = lsr.LogoutDate.HasValue ? new System.DateTime(lsr.LogoutDate.Value.Ticks, System.DateTimeKind.Utc).ToLocalTime().ToString("M/d/yyyy HH:mm", usaDateFormatInfo) : string.Empty,
                TimeOnline = _userAnalyticsService.GetTimeOnline(lsr.MinutesOnline)
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}