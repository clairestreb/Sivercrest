using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.Client;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.Interfaces.Client;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    public class AccountsController : Controller
    {
        IAccountService _accountService;

        public AccountsController(IAccountService service)
        {
            _accountService = service;
        }
        
        [HttpGet]
        public JsonResult ViewAccounts(int? contactId = null)
        {
            return Json(_accountService.GetAccounts(contactId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ViewGroups(int? contactId = null)
        {
            return Json(_accountService.GetGroups(contactId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ViewAccountsInGroups(int? contactId = null, int? entityId = null, bool? isGroup = null, bool? isClientGroup = null)
        {
            return Json(_accountService.GetAccountsWithinGroup(contactId, entityId, isGroup, isClientGroup), JsonRequestBehavior.AllowGet);
        }
    }
}