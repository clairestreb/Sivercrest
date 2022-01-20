using Silvercrest.Interfaces;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Areas.TeamMember.Controllers
{
    [Maintance]
    public class TeamSettingsController : Controller
    {
        private IManagerService _managerService;

        public TeamSettingsController(IManagerService service)
        {
            _managerService = service;
        }

        public ActionResult Index(string firmUserGroupIdQuery)
        {
            int? firmUserGroupId = null;
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var teamSettings = _managerService.GetTeamSettings(firmUserGroupId);    //5
            var setting = new ViewModels.TeamMember.TeamSettingsViewModel();
            setting.Code = teamSettings.FirstOrDefault().Code;
            setting.ManagerName = teamSettings.FirstOrDefault().ManagerName;
            setting.StatementUploadOnHold = teamSettings.FirstOrDefault().StatementUploadOnHold != null ? teamSettings.FirstOrDefault().StatementUploadOnHold : false;
            setting.FirmUserGroupId = firmUserGroupId.Value;
            setting.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            setting.EmailNotification = teamSettings.FirstOrDefault().email_notifications != null ? teamSettings.FirstOrDefault().email_notifications : true;
            setting.EquityWriteUps = teamSettings.FirstOrDefault().receives_equity_writeups != null ? teamSettings.FirstOrDefault().receives_equity_writeups : true;
            setting.EconomicCommentary = teamSettings.FirstOrDefault().receives_econ_commentary != null ? teamSettings.FirstOrDefault().receives_econ_commentary : false;
            return View(setting);
        }


        [HttpPost]
        public JsonResult UpdateTeamSettings(string firmUserGroupIdQuery, bool onHold, string userName)
        {
            int? firmUserGroupId = null;
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            userName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            _managerService.UpdateTeamSettings(firmUserGroupId, onHold, userName);
            return Json("");
        }


        [HttpPost]
        public JsonResult UpdateTSEmailNotification(string firmUserGroupIdQuery, bool emailNotification)
        {
            int? firmUserGroupId = null;
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var userName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            _managerService.UpdateTSEmailNotification(firmUserGroupId, emailNotification, userName);
            return Json("");
        }

        [HttpPost]
        public JsonResult UpdateTSEquityWriteUps(string firmUserGroupIdQuery, bool equityWriteUps)
        {
            int? firmUserGroupId = null;
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var userName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            _managerService.UpdateTSEquityWriteUps(firmUserGroupId, equityWriteUps, userName);
            return Json("");
        }

        [HttpPost]
        public JsonResult UpdateTSEconomicCommentary(string firmUserGroupIdQuery, bool economicCommentary)
        {
            int? firmUserGroupId = null;
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var userName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            _managerService.UpdateTSEconomicCommentary(firmUserGroupId, economicCommentary, userName);
            return Json("");
        }
    }
}