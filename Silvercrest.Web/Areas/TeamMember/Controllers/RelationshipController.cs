using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using Silvercrest.ViewModels.TeamMember;
using Silvercrest.Web.Common;
using Silvercrest.Web.Helpers.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;

namespace Silvercrest.Web.Areas.TeamMember.Controllers
{
    [Authorize]
    [Maintance]
    public class RelationshipController : Controller
    {
        private IManagerService _managerService;
        private IManagerAdo _managerAdoService;
        public RelationshipController(IManagerService service, IManagerAdo serviceAdo)
        {
            _managerService = service;
            _managerAdoService = serviceAdo;
        }

        public ActionResult Index(string familyIdQuery, string firmUserGroupIdQuery)
        {
            int? familyId;
            int? firmUserGroupId;
            familyId = string.IsNullOrEmpty(familyIdQuery) || familyIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(familyIdQuery));
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var view = new TeamMemberRelationshipViewModel();
            view.FamilyId = familyId;
            view.FirmUserGroupId = firmUserGroupId;
            List<Account> accounts = new List<Account>();
            _managerAdoService.GetAccountsByFamily(view, familyId, firmUserGroupId, accounts);
            view.TableData = Json(accounts, JsonRequestBehavior.AllowGet);
            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }

    }
}