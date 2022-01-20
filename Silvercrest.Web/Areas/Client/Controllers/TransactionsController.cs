 using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using Silvercrest.ViewModels.Client.Holdings.Index;
using Silvercrest.ViewModels.Client.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Common;
using Silvercrest.Web.Helpers.Maintance;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Authorize]
    [Maintance]
    public class TransactionsController : BaseController
    {
        private ITransactions _transactionsService;
        private ITransactionsAdoService _transactionsAdoService;

        public TransactionsController(ITransactions service, ITransactionsAdoService serviceAdo)
        {
            _transactionsService = service;
            _transactionsAdoService = serviceAdo;
        }

        [IgnoreAnalytics]
        public ActionResult Index(string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            bool? isGroup = null;
            bool? isClientGroup = null;
            int? contactId = null;
            int? entityId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            entityId = string.IsNullOrEmpty(entityIdQuery) || entityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(entityIdQuery));
            isGroup = string.IsNullOrEmpty(isGroupQuery) || isGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isGroupQuery));
            isClientGroup = string.IsNullOrEmpty(isClientGroupQuery) || isClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isClientGroupQuery));
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

            parameters.resolveHoldingsParameters(entityId, isGroup, isClientGroup);

            var userInfo = GetUserInfo(parameters.isGroup, parameters.isClientGroup, parameters.contactId, parameters.entityId);
            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            TopSectionViewModel pageData = new TopSectionViewModel();
            var view = new TransactionsViewModel();
//            var userInfo = GetUserInfo(isGroup, isClientGroup, contactId, entityId);
//            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);
            _transactionsAdoService.FillTransactionsView(view, userInfo, accounts, groups, accountsWithinGroup, pageData);
            view.PageData = new TopSectionViewModel();
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            _transactionsService.FillInfoView(view.PageData, userInfo);
            view.contactFullName = parameters.contactFullName;
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

        public ActionResult Purchases(string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            bool? isGroup = null;
            bool? isClientGroup = null;
            int? contactId = null;
            int? entityId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            entityId = string.IsNullOrEmpty(entityIdQuery) || entityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(entityIdQuery));
            isGroup = string.IsNullOrEmpty(isGroupQuery) || isGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isGroupQuery));
            isClientGroup = string.IsNullOrEmpty(isClientGroupQuery) || isClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isClientGroupQuery));

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

            parameters.resolveHoldingsParameters(entityId, isGroup, isClientGroup);

            var userInfo = GetUserInfo(parameters.isGroup, parameters.isClientGroup, parameters.contactId, parameters.entityId);
            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            var view = new TransactionsPurchasesViewModel();
            TopSectionViewModel pageData = new TopSectionViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<Purchase> tableList = new List<Purchase>();
            //            var userInfo = GetUserInfo(isGroup, isClientGroup, contactId, entityId);
            //            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            string startDate = null;
            string endDate = null;
            if (Request.Cookies["searchFrom"] != null && Request.Cookies["searchTo"] != null)
            {
                startDate = Request.Cookies["searchFrom"].Value;
                endDate = Request.Cookies["searchTo"].Value;
            }

            _transactionsAdoService.FillPurchasesView(view, userInfo, accounts, groups, accountsWithinGroup, tableList, pageData, startDate, endDate);
            view.Error = "";
            if (tableList.Count > 1000)
            {
                tableList.RemoveRange(999, tableList.Count - 1000);
                view.Error = "Dataset is too large, please narrow your date range!";
            }
            view.PageData = pageData;
            view.TableData = Json(tableList, JsonRequestBehavior.AllowGet);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }

        public ActionResult Sales(string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            bool? isGroup = null;
            bool? isClientGroup = null;
            int? contactId = null;
            int? entityId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            entityId = string.IsNullOrEmpty(entityIdQuery) || entityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(entityIdQuery));
            isGroup = string.IsNullOrEmpty(isGroupQuery) || isGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isGroupQuery));
            isClientGroup = string.IsNullOrEmpty(isClientGroupQuery) || isClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isClientGroupQuery));

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

            parameters.resolveHoldingsParameters(entityId, isGroup, isClientGroup);

            var userInfo = GetUserInfo(parameters.isGroup, parameters.isClientGroup, parameters.contactId, parameters.entityId);
            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            var view = new TransactionsSalesViewModel();
            TopSectionViewModel pageData = new TopSectionViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<Sale> tableList = new List<Sale>();
//            var userInfo = GetUserInfo(isGroup, isClientGroup, contactId, entityId);
//            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            string startDate = null;
            string endDate = null;
            if (Request.Cookies["searchFrom"] != null && Request.Cookies["searchTo"] != null)
            {
                startDate = Request.Cookies["searchFrom"].Value;
                endDate = Request.Cookies["searchTo"].Value;
            }

            _transactionsAdoService.FillSalesView(view, userInfo, accounts, groups, accountsWithinGroup, tableList, pageData, startDate, endDate);
            view.Error = "";
            if (tableList.Count > 1000)
            {
                tableList.RemoveRange(999, tableList.Count - 1000);
                view.Error = "Dataset is too large, please narrow your date range!";
            }
            view.PageData = pageData;
            view.TableData = Json(tableList, JsonRequestBehavior.AllowGet);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }

        public ActionResult Contributions(string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            bool? isGroup = null;
            bool? isClientGroup = null;
            int? contactId = null;
            int? entityId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Hash.GetDecryptedValue(contactIdQuery));
            entityId = string.IsNullOrEmpty(entityIdQuery) || entityIdQuery == "undefined" ? (int?)null : int.Parse(Hash.GetDecryptedValue(entityIdQuery));
            isGroup = string.IsNullOrEmpty(isGroupQuery) || isGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isGroupQuery));
            isClientGroup = string.IsNullOrEmpty(isClientGroupQuery) || isClientGroupQuery == "undefined" ? (bool?)null : bool.Parse(Hash.GetDecryptedValue(isClientGroupQuery));

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

            parameters.resolveHoldingsParameters(entityId, isGroup, isClientGroup);

            var userInfo = GetUserInfo(parameters.isGroup, parameters.isClientGroup, parameters.contactId, parameters.entityId);
            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            var view = new TransactionsContributionsViewModel();
            TopSectionViewModel pageData = new TopSectionViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<Contribution> tableList = new List<Contribution>();
//            var userInfo = GetUserInfo(isGroup, isClientGroup, contactId, entityId);
//            SaveAccountInfo(userInfo.IsGroup, userInfo.IsClientGroup, userInfo.ContactId, userInfo.EntityId);

            string startDate = null;
            string endDate = null;
            if (Request.Cookies["searchFrom"] != null && Request.Cookies["searchTo"] != null)
            {
                startDate = Request.Cookies["searchFrom"].Value;
                endDate = Request.Cookies["searchTo"].Value;
            }

            _transactionsAdoService.FillContributionsView(view, userInfo, accounts, groups, accountsWithinGroup, tableList, pageData, startDate, endDate);
            view.Error = "";
            if (tableList.Count > 1000)
            {
                tableList.RemoveRange(999, tableList.Count - 1000);
                view.Error = "Dataset is too large, please narrow your date range!";
            }
            view.PageData = pageData;
            view.TableData = Json(tableList, JsonRequestBehavior.AllowGet);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }
        
        public ActionResult GetContributions(string searchFrom, string searchTo, string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
                Response.Cookies["searchFrom"].Value = searchFrom;
                Response.Cookies["searchTo"].Value = searchTo;
            return RedirectToAction("Contributions", "Transactions", new { isGroupQuery = isGroupQuery, isClientGroupQuery = isClientGroupQuery, contactIdQuery = contactIdQuery, entityIdquery = entityIdQuery });
        }
        
        public ActionResult GetPurchases(string searchFrom, string searchTo, string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            Response.Cookies["searchFrom"].Value = searchFrom;
            Response.Cookies["searchTo"].Value = searchTo;
            return RedirectToAction("Purchases", "Transactions", new { isGroupQuery = isGroupQuery, isClientGroupQuery = isClientGroupQuery, contactIdQuery = contactIdQuery, entityIdquery = entityIdQuery });
            
        }

        public ActionResult GetSales(string searchFrom, string searchTo, string isGroupQuery, string isClientGroupQuery,
            string contactIdQuery, string entityIdQuery)
        {
            Response.Cookies["searchFrom"].Value = searchFrom;
            Response.Cookies["searchTo"].Value = searchTo;
            return RedirectToAction("Sales", "Transactions", new { isGroupQuery = isGroupQuery, isClientGroupQuery = isClientGroupQuery, contactIdQuery = contactIdQuery, entityIdquery = entityIdQuery });
        }

        //Sets the right cookies so that we remember what entity is selected, but MORE IMPORTANTLY,
        //If the selected entity is a group, then we update the Group Cookies for the Client/Home
        public void SaveAccountInfo(bool? isGroup = null, bool? isClientGroup = null,
            int? contactId = null, int? entityId = null)
        {
            Response.Cookies["accountContactId"].Value = contactId.ToString();
/*            Response.Cookies["entityId"].Value = entityId.ToString();
            Response.Cookies["isGroup"].Value = isGroup.ToString();
            Response.Cookies["isClientGroup"].Value = isClientGroup.ToString();

            if (isGroup == true)
            {
                Response.Cookies["groupEntityId"].Value = entityId.ToString();
                Response.Cookies["groupIsClientGroup"].Value = isClientGroup.ToString();
            }
*/
        }

        public UserInfo GetAccountInfo()
        {
            UserInfo info;
                 
            try
            {
                info = new UserInfo
                {
                    ContactId = int.Parse(Request.Cookies["accountContactId"].Value),
                    EntityId = int.Parse(Request.Cookies["entityId"].Value),
                    IsGroup = bool.Parse(Request.Cookies["isGroup"].Value),
                    IsClientGroup = bool.Parse(Request.Cookies["isClientGroup"].Value)
                };
            }
            catch { info = null; }

            return info;

        }
        public UserInfo GetAccountInfo(int? contactId = null)
        {
            UserInfo info = null;

            try
            {
                info = new UserInfo
                {
                    ContactId = int.Parse(Request.Cookies["accountContactId"].Value),
                    EntityId = int.Parse(Request.Cookies["entityId"].Value),
                    IsGroup = bool.Parse(Request.Cookies["isGroup"].Value),
                    IsClientGroup = bool.Parse(Request.Cookies["isClientGroup"].Value)
                };
            }
            catch { info = null; }

            return info;

        }
        public UserInfo GetUserInfo(bool? isGroup = null, bool? isClientGroup = null,
          int? contactId = null, int? entityId = null)
        {
            UserInfo userInfo = null;
            if (contactId != null)
            {
                userInfo = new UserInfo(contactId, entityId, isGroup, isClientGroup);
            }
            if (userInfo == null)
            {
                userInfo = GetAccountInfo(contactId);
            }
            if (userInfo == null)
            {
                userInfo = new UserInfo
                {
                    ContactId = GetContactId(contactId),
                    EntityId = -1,
                    IsClientGroup = false,
                    IsGroup = true
                };
            }

            return userInfo;
        }

        public int? GetContactId(int? contactId)
        {
            if (contactId == null)
            {
                contactId = int.Parse(Request.Cookies["MainContactId"].Value);
            }
            return contactId;
        }

    }
}