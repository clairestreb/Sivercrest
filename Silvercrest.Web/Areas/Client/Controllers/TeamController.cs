using Silvercrest.Interfaces;
using Silvercrest.Services.CommonServices;
using Silvercrest.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.ViewModels.Client;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Areas.Client.Controllers
{ 
    [Authorize]
    [Maintance]
    public class TeamController : BaseController
    {
        private ITeam _teamService;

        public TeamController(ITeam teamService)
        {
            _teamService = teamService;
        }

        public ActionResult Index(string contactIdQuery)
        {
            int? contactId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            var model = new ClientTeamListViewModel();
            model.teamModels = _teamService.GetClientTeam(GetContactId(contactId));

            ContactId = GetContactId(null);
            var user_role = GetUserRoles();
            if (!user_role)
            {
                if (contactId != ContactId)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                }
            }

            URLParameters parameters = null;
            if (Session[contactId.ToString()] == null)
            {
                parameters = new URLParameters((int)contactId);
            }
            else
            {
                parameters = (URLParameters)Session[contactId.ToString()];
            }

//          view.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;
            model.contactFullName = parameters.contactFullName;
            return View(model);
        }
        public bool GetUserRoles()
        {
            if (Request.Cookies["UserRole"] != null)
            {
                var role = Request.Cookies["UserRole"].Value;

                if (role == UserRole.Administrator.ToString() || role == UserRole.SuperUser.ToString() || role == UserRole.TeamMember.ToString())
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        [HttpPost]
        public JsonResult SendEmails(string subject, string message, string senderName)
        {
            EmailService.SendEmailToTeam(subject, message,
                _teamService
                .GetClientTeam(GetContactId(null))
                .Select(x => x.Email).ToArray(), User.Identity.Name, senderName);

            return Json(null, JsonRequestBehavior.AllowGet);  
        }

        public int? GetContactId(int? contactId)
        {        
            return contactId ?? int.Parse(Request.Cookies["contactId"].Value);
        }
    }
}