using Silvercrest.Interfaces;
using Silvercrest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.ViewModels.Client.Holdings;
using System.Web.UI;
using Silvercrest.Entities;
using Silvercrest.ViewModels.Client.Holdings.Index;
using Silvercrest.Web.Helpers.Analytics;
using Silvercrest.Interfaces.Client;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Common;
using Silvercrest.Web.Helpers.Maintance;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Authorize]
    [Maintance]
    public class HoldingsController : BaseController
    {
        private IHoldingsMainService _holdingsMainService;
        private IHoldingsAdoService _adoService;
        public HoldingsController(IHoldingsMainService service, IHoldingsAdoService adoService)
        {
            _holdingsMainService = service;
            _adoService = adoService;
        }

        public ActionResult Index(int? contactId = null)
        {
            contactId = GetContactId(contactId);
            var view = new TopSectionViewModel();
            _holdingsMainService.FillInfoView(view, new UserInfo { ContactId = contactId });
            return View(view);
        }

        public ActionResult ViewAccount(string isGroupQuery, string isClientGroupQuery,
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

            Session[contactId.ToString()] = parameters;


            HoldingsViewModel view = new HoldingsViewModel();
            List<PieChart> charts = new List<PieChart>();
            List<PieChart> chartsStrategy = new List<PieChart>();
            List<BalanceViewModel> balances = new List<BalanceViewModel>();
            List<BalanceViewModel> fullBalances = new List<BalanceViewModel>();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            _adoService.FillHoldingsView(userInfo, view, charts, balances, fullBalances, chartsStrategy, accounts, groups, accountsWithinGroup);
            view.ChartData = Json(charts, JsonRequestBehavior.AllowGet);
            view.TableData = Json(balances, JsonRequestBehavior.AllowGet);
            view.FullBalancesData = Json(fullBalances, JsonRequestBehavior.AllowGet);
            view.ChartStrategyData = Json(chartsStrategy, JsonRequestBehavior.AllowGet);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
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
        public ActionResult Cash(string isGroupQuery, string isClientGroupQuery,
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

            Session[contactId.ToString()] = parameters;
            HoldingsCashViewModel view = new HoldingsCashViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<CashViewModel> cashList = new List<CashViewModel>();
            _adoService.FillCashView(userInfo, view, accounts, groups, accountsWithinGroup, cashList);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.CashData = Json(cashList, JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;
            return View(view);
        }

        public ActionResult Income(string isGroupQuery, string isClientGroupQuery,
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

            Session[contactId.ToString()] = parameters;
            HoldingsIncomeViewModel view = new HoldingsIncomeViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<PieChart> chartsType = new List<PieChart>();
            List<PieChart> chartsMunic = new List<PieChart>();
            List<IncomeViewModel> income = new List<IncomeViewModel>();
            _adoService.FillIncomeView(userInfo, view, accounts, groups, accountsWithinGroup, chartsType, chartsMunic, income);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.ChartTypeData = Json(chartsType, JsonRequestBehavior.AllowGet);
            view.ChartMunicData = Json(chartsMunic, JsonRequestBehavior.AllowGet);
            view.IncomeData = Json(income, JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }

        public ActionResult Equities(string isGroupQuery, string isClientGroupQuery,
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

            Session[contactId.ToString()] = parameters;
            HoldingsIncomeViewModel view = new HoldingsIncomeViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<PieChart> chartsType = new List<PieChart>();
            List<PieChart> chartsMunic = new List<PieChart>();
            List<IncomeViewModel> income = new List<IncomeViewModel>();
            _adoService.FillEquitiesView(userInfo, view, accounts, groups, accountsWithinGroup, chartsType, chartsMunic, income);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.ChartTypeData = Json(chartsType, JsonRequestBehavior.AllowGet);
            view.ChartMunicData = Json(chartsMunic, JsonRequestBehavior.AllowGet);
            view.IncomeData = Json(income, JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
        }

        public ActionResult OtherAssets(string isGroupQuery, string isClientGroupQuery,
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

            Session[contactId.ToString()] = parameters;
            HoldingsCashViewModel view = new HoldingsCashViewModel();
            List<Account> accounts = new List<Account>();
            List<Account> groups = new List<Account>();
            List<Account> accountsWithinGroup = new List<Account>();
            List<CashViewModel> assetsList = new List<CashViewModel>();
            _adoService.FillOtherAssetsView(userInfo, view, accounts, groups, accountsWithinGroup, assetsList);
            view.AccountsData = Json(Hash.EncryptEntities(accounts), JsonRequestBehavior.AllowGet);
            view.GroupsData = Json(Hash.EncryptEntities(groups), JsonRequestBehavior.AllowGet);
            view.AccountsWithinGroupsData = Json(Hash.EncryptEntities(accountsWithinGroup), JsonRequestBehavior.AllowGet);
            view.CashData = Json(assetsList, JsonRequestBehavior.AllowGet);
            view.contactFullName = parameters.contactFullName;

            return View(view);
            //return View(GetTopInfo(userInfo, 4));
        }
        
        //Sets the right cookies so that we remember what entity is selected, but MORE IMPORTANTLY,
        //If the selected entity is a group, then we update the Group Cookies for the Client/Home
        public void SaveAccountInfo(bool? isGroup = null, bool? isClientGroup = null,
            int? contactId = null, int? entityId = null)
        {
            Response.Cookies["accountContactId"].Value =     contactId.ToString();
/*            Response.Cookies["entityId"].Value =      entityId.ToString();
            Response.Cookies["isGroup"].Value =       isGroup.ToString();
            Response.Cookies["isClientGroup"].Value = isClientGroup.ToString();

            if(isGroup == true)
            {
                Response.Cookies["groupEntityId"].Value = entityId.ToString();
                Response.Cookies["groupIsClientGroup"].Value = isClientGroup.ToString();
            }
            else  //Have to put it in this way otherwise it gets reset to empty string because it is a req response cookie
            {
                Response.Cookies["groupEntityId"].Value = Request.Cookies["groupEntityId"].Value;
                Response.Cookies["groupIsClientGroup"].Value = Request.Cookies["groupIsClientGroup"].Value;
            }
 */
        }
        [HttpPost]
        public JsonResult GetModalTable(string securityId, string isGroupQuery, string isClientGroupQuery,
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

            List<IncomeViewModel> table = new List<IncomeViewModel>();
            var userInfo = GetUserInfo(isGroup, isClientGroup, contactId, entityId);
            _adoService.FillModalView(userInfo, int.Parse(securityId), table);
            return Json(table, JsonRequestBehavior.AllowGet);
        }

        public int? GetContactId(int? contactId)
        {
            if (contactId == null)
            {
                contactId = int.Parse(Request.Cookies["MainContactId"].Value);
            }
            return contactId;
        }

        public TopSectionViewModel GetTopInfo(UserInfo info, int? type = null)
        {
            info.ContactId = GetContactId(info.ContactId);
            var view = new TopSectionViewModel();
            _holdingsMainService.FillInfoView(view, info, type);
            return view;
        }

        public UserInfo GetAccountInfo(int? contactId = null)
        {
            UserInfo info= null;
            
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
/*
            if (userInfo == null)
            {
                userInfo = new UserInfo {
                    ContactId = GetContactId(contactId),
                    EntityId = -1,
                    IsClientGroup = false,
                    IsGroup = true };
            }
*/
            return userInfo;
        }
    }
}