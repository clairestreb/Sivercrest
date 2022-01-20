using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using Silvercrest.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Common;
using System.Reflection;
using Silvercrest.Web.Helpers.Maintance;
using System.Text.RegularExpressions;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Authorize]
    [Maintance]
    public class HomeController : BaseController
    {
        private IClientService _service;
        private IClientAdoService _adoService;

        public HomeController(IClientService service, IClientAdoService adoService)
        {
            _service = service;
            _adoService = adoService;
        }
        [IgnoreAnalytics]
        public ActionResult Index(string contactIdQuery, string groupEntityIdQuery, string groupIsClientGroupQuery, string entityIdQuery, string isGroupQuery, string isClientGroupQuery, bool allow = false)
        {
            int? contactId, groupEntityId, entityId;
            bool? groupIsClientGroup, isGroup, isClientGroup;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            groupEntityId = string.IsNullOrEmpty(groupEntityIdQuery) || groupEntityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(groupEntityIdQuery));
            groupIsClientGroup = string.IsNullOrEmpty(groupIsClientGroupQuery) || groupIsClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(groupIsClientGroupQuery));
            entityId = string.IsNullOrEmpty(entityIdQuery) || entityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(entityIdQuery));
            isClientGroup = string.IsNullOrEmpty(isClientGroupQuery) || isClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isClientGroupQuery));
            isGroup = string.IsNullOrEmpty(isGroupQuery) || isGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isGroupQuery));

            var check_name = User.Identity.Name;
            ContactId = _service.GetContactId(check_name);
            var user_role = GetUserRoles();
            if (!user_role)
            {
                // client
                if (!allow && contactId != ContactId)
                {
                    // Check Home link in DocumentDownload page
                    if (contactId == null)
                    {
                        var contactId_for_document = Int32.Parse(Request.Cookies["contactId"].Value);
                        if (contactId_for_document != ContactId)
                        {
                            return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                        }
                    }
                }
            }


            List<PieChart> charts = new List<PieChart>();
            List<ClientGroupAccount> mappedAccountList = new List<ClientGroupAccount>();
            if (contactId != null)
            {
                Response.Cookies["contactId"].Value = contactId.ToString();
            }
            var view = new IndexInitViewModel();
            if (!contactId.HasValue && (Request.Cookies["contactId"] == null || Request.Cookies["contactId"].Value == null || Request.Cookies["contactId"].Value == ""))
            {
                var name = User.Identity.Name;
                ContactId = _service.GetContactId(name);
            }
            if (!contactId.HasValue && Request.Cookies["contactId"] != null && Request.Cookies["contactId"].Value != null && Request.Cookies["contactId"].Value != "")
            {
                ContactId = Int32.Parse(Request.Cookies["contactId"].Value);
            }
            if (contactId.HasValue)
            {
                Response.Cookies["contactId"].Value = contactId.ToString();
                ContactId = Int32.Parse(Response.Cookies["contactId"].Value);
            }

            view.ContactId = (int)ContactId;
            URLParameters parameters = null;
            if (Session[ContactId.ToString()] == null)
            {
                parameters = new URLParameters((int)ContactId);
            }
            else
            {
                parameters = (URLParameters)Session[ContactId.ToString()];
            }


            parameters.resolveHomeParameters(groupEntityId, groupIsClientGroup, entityId, isGroup, isClientGroup);
            _adoService.FillHomeView(view, charts, new UserInfo { ContactId = ContactId, EntityId = parameters.groupEntityId, IsGroup = true, IsClientGroup = parameters.groupIsClientGroup, SubEntityId = parameters.entityId}, mappedAccountList);

            int? resolvedGroupEntityId = null;
            bool? resolvedIsClientGroup = null;
            foreach (var acct in mappedAccountList)
            {
                if(acct.AccountEntityId != null)
                {
                    resolvedGroupEntityId = acct.GroupEntityId;
                    resolvedIsClientGroup = acct.GroupIsClientGroup;
                    break;
                }

            }

            parameters.resolveHomeParameters(resolvedGroupEntityId, resolvedIsClientGroup, parameters.entityId, parameters.isGroup, parameters.isClientGroup);

            Session[ContactId.ToString()] = parameters;

            Response.Cookies["accountContactId"].Value = view.ContactId.ToString();
/*            Response.Cookies["entityId"].Value = entityId==null ? resolvedGroupEntityId.ToString() : entityId.ToString();
            Response.Cookies["isGroup"].Value = entityId == null ? "true" : "false"; //isGroup.ToString();
            Response.Cookies["isClientGroup"].Value = entityId == null ? resolvedIsClientGroup.ToString() : false.ToString();// isClientGroup.ToString(); 
            Response.Cookies["groupEntityId"].Value = resolvedGroupEntityId.ToString();
            Response.Cookies["groupIsClientGroup"].Value = resolvedIsClientGroup.ToString(); 
*/
            view.ChartData = Json(charts, JsonRequestBehavior.AllowGet);
            view.TableData = Json(mappedAccountList, JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }

        public ActionResult AssignNicknames()
        {
            var model = new NicknamesClientViewModel();
            int contactId = int.Parse(Request.Cookies["contactId"].Value);
            var nicknames = _service.GetNicknames(contactId);
            model.TableData = Json(nicknames, JsonRequestBehavior.AllowGet);
            model.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;
            return View(model);
        }

        public JsonResult DeleteNickname(int contactId, int accountId)
        {

            _service.DeleleNickname(contactId, accountId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNickname(int contactId, int accountId, string nickname)
        {

            Regex r = new Regex("^[a-zA-Z0-9 ]*$");
            if (r.IsMatch(nickname))
            {
                _service.AddNickname(contactId, accountId, Request.Cookies["FullName"].Value, nickname);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AcceptTerms()
        {
            _service.AcceptTerms(Request.Cookies["MainContactId"].Value);
            Response.Cookies["IsFirstLogin"].Value = "false";

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomFeatures(string contactIdQuery)
        {
            int? contactId;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(contactIdQuery));

            var check_name = User.Identity.Name;
            ContactId = _service.GetContactId(check_name);

            var user_role = GetUserRoles();
            if (!user_role)
            {
                if (contactId != ContactId)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                }
            }

            var view = new IndexInitViewModel
            {
                ContactId = (int)contactId,
            };
            view.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;

            return View(view);
        }
        public bool GetUserRoles()
        {
            if (Request.Cookies["UserRole"] != null)
            {
                var role = Request.Cookies["UserRole"].Value;

                if (role == UserRole.Administrator.ToString() || role == UserRole.SuperUser.ToString() || role == UserRole.TeamMember.ToString() )
                {
                    return true;
                }

                return false;
            }
            return false;
        }
    }
}