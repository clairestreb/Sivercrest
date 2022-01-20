using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Interfaces.TeamMember;
using Silvercrest.ViewModels.TeamMember;
using Silvercrest.Web.Common;
using Silvercrest.Web.Helpers.Analytics;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;

namespace Silvercrest.Web.Areas.TeamMember.Controllers
{
    [Authorize]
    [Analytics]
    [Maintance]
    public class ManagerController : Controller
    {
        private IManagerService _managerService;
        private IManagerAdo _managerAdoService;
        private IUserAnalyticsService _userAnalyticsService;


        private string _defaultCode = "BMB0100800";
        public ManagerController(IManagerService service, IManagerAdo serviceAdo, IUserAnalyticsService serviceAnalytics)
        {
            _managerService = service;
            _managerAdoService = serviceAdo;
            _userAnalyticsService = serviceAnalytics;
        }
        
        public ActionResult Index(TeamMemberFamiliesViewModel view)
        {
            var name = User.Identity.Name;
            var code = _managerService.GetCode(name);
            if (view == null)
            {
                view = new TeamMemberFamiliesViewModel();
            }
            if (code == null)
            {
                code = _defaultCode;
            }
            view.Managers = _managerService.GetManagers(code);
//            view.HashData = Hash.GetHash();
            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }

        public ActionResult ClientAnalyticsList()
        {
            int? contactId = _managerService.GetContactId(User.Identity.Name);
            ClientAnalyticsViewModel model = new ClientAnalyticsViewModel();
            var analytics = _userAnalyticsService.GetAllAnalytics(contactId);
            foreach (var item in analytics)
            {
                item.ContactIdQuery = Hash.GetEncryptedValue(item.ContactId.ToString());
                item.FamilyIdQuery = Hash.GetEncryptedValue(item.FamilyId.ToString());
                item.FirmUserGroupIdQuery = Hash.GetEncryptedValue(item.FirmUserGroupId.ToString());
                item.IsFromQuery = Hash.GetEncryptedValue(true.ToString());
            }

            model.TableData = Json(analytics, JsonRequestBehavior.AllowGet);
            model.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(model);
        }

        public ActionResult ManagerContactList()
        {
            int? contactId = _managerService.GetContactId(User.Identity.Name);
            TeamMemberContactListViewModel view = new TeamMemberContactListViewModel();
            List<ManagerContactComplete> contacts = new List<ManagerContactComplete>();
            _managerAdoService.GetContactList(contactId, contacts);
            foreach (var item in contacts)
            {
                item.ContactIdQuery = Hash.GetEncryptedValue(item.ContactId.ToString());
                item.FamilyIdQuery = Hash.GetEncryptedValue(item.FamilyId.ToString());
                item.FirmUserGroupIdQuery = Hash.GetEncryptedValue(item.FirmUserGroupId.ToString());
            }
            view.TableData = Json(contacts, JsonRequestBehavior.AllowGet);
            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }
    }
}