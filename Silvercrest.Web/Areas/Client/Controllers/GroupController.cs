using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using Silvercrest.ViewModels.Client.Groups;
using Silvercrest.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Maintance]
    public class GroupController : BaseController
    {
        private IGroupService _groupService;

        public GroupController(IGroupService service)
        {
            _groupService = service;
        }

        public ActionResult Index(string contactIdQuery)
        {
            int? contactId;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined"? (int?)null : int.Parse(Hash.GetDecryptedValue(contactIdQuery));

            ContactId = GetContactId(null);
            var user_role = GetUserRoles();
            if (!user_role)
            {
                if (contactId != ContactId)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                }
            }

            var view = _groupService.GetGroupsList(contactId);
            view.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;
            return View(view);
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
        public int? GetContactId(int? contactId)
        {
            if (contactId == null)
            {
                contactId = int.Parse(Request.Cookies["MainContactId"].Value);
            }
            return contactId;
        }


        [HttpPost]
        public JsonResult UpdateGroup(int? contactId, string groupName, string accountIds, string changerName, int? accountGroupId)
        {
            _groupService.UpdateGroup(contactId, groupName, accountIds, changerName, accountGroupId);
            return Json("");
        }

        [HttpPost]
        public JsonResult DeleteGroup(int? contactId, int accountGroupId)
        {
            _groupService.DeleteGroup(contactId, accountGroupId);
            return Json("");
        }


        [HttpPost]
        public JsonResult CreateGroup(int? contactId, string groupName, string accountIds, string changerName)
        {
            _groupService.UpdateGroup(contactId, groupName, accountIds, changerName, null);
            return Json("");
        }

    }
}