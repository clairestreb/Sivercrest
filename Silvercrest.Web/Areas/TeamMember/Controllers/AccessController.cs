using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.TeamMember;
using Silvercrest.Web.Helpers.Analytics;
using Silvercrest.Web.Common;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Authorize;
using Silvercrest.Entities.Enums;
using Silvercrest.DataAccess.Model;
using Microsoft.AspNet.Identity;
using Silvercrest.DataAccess.IdentityStore;
using Microsoft.AspNet.Identity.Owin;
using Silvercrest.ViewModels;
using System.Threading.Tasks;
using Silvercrest.Utilities;
using Silvercrest.Services.CommonServices;
using System.Text.RegularExpressions;
using Silvercrest.Web.Helpers.Maintance;

namespace Silvercrest.Web.Areas.TeamMember.Controllers
{
    [Maintance]
    public class AccessController : Controller
    {
        //private SLVR_DEVEntities _context;
        IManagerService _managerService;
        private UserManager<AspNetUser, string> _userManagerFactory;
        private IAdministratorService _administratorService;
        public AccessController(IManagerService service, IAdministratorService administratorService/*, SLVR_DEVEntities context*/)
        {
            //_context = context;
            _managerService = service;
            _administratorService = administratorService;
        }

        private UserManager<AspNetUser, string> Create()
        {
            var userManager = new UserManager<AspNetUser, string>(new UserStore(HttpContext.GetOwinContext().Get<SLVR_DEVEntities>()));
            userManager.UserValidator = new UserValidator<AspNetUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
            return userManager;
        }

        public UserManager<AspNetUser, string> UserManagerFactory
        {
            get
            {
                return _userManagerFactory ?? Create();
            }
            private set
            {
                _userManagerFactory = value;
            }
        }

        public async Task<ActionResult> Index(string contactIdQuery, string familyIdQuery, string firmUserGroupIdQuery)
        {
            int? contactId = null;
            int? familyId = null;
            int? firmUserGroupId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId() : int.Parse(Common.Hash.GetDecryptedValue(contactIdQuery));
            familyId = string.IsNullOrEmpty(familyIdQuery) || familyIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(familyIdQuery));
            firmUserGroupId = string.IsNullOrEmpty(firmUserGroupIdQuery) || firmUserGroupIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(firmUserGroupIdQuery));
            var accountAccessInfo = _managerService.FillInInfo(contactId);
            accountAccessInfo.FamilyId = familyId;
            accountAccessInfo.FirmUserGroupId = firmUserGroupId;
            accountAccessInfo.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            if (!String.IsNullOrEmpty(accountAccessInfo.Email))
            {
                var user = await UserManagerFactory.FindByEmailAsync(accountAccessInfo.Email);
                accountAccessInfo.TwoFactorAuth = (TwoFactorAuth)_administratorService.GetTwoFactorAuthById(user.Id);
                accountAccessInfo.PhoneNumber = await UserManagerFactory.GetPhoneNumberAsync(user.Id);
            }
            else
            {
                accountAccessInfo.TwoFactorAuth = TwoFactorAuth.Inactive;
                accountAccessInfo.PhoneNumber = String.Empty;
            }
            return View(accountAccessInfo);
        }

        public JsonResult GetAccountAccess(int? contactId)
        {
            var a = _managerService.GetAccountAccess(contactId);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNonAccountAccess(int? contactId)
        {
            var a = _managerService.GetNonAccountAccess(GetContactId(), contactId);
            return Json(a,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAccess(int? contactId, string accountIds)
        {
            string fullname = GetContactName(GetContactId());
            return Json(_managerService.RemoveAccess(contactId, accountIds, fullname), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GrantAccess(int? contactId, string accountIds)
        {
            string fullname = GetContactName(GetContactId());
            return Json(_managerService.GrantAccess(contactId, accountIds, fullname), JsonRequestBehavior.AllowGet);
        }

        public int GetContactId()
        {
            return int.Parse(Request.Cookies["MainContactId"].Value);
        }

        public string GetContactName(int? contactId)
        {
            return _managerService.GetContactName(contactId);
        }

        [HttpPost]
        public JsonResult UpdateEmail(int? contactId, string email)
        {
            _managerService.UpdateEmail(contactId, email);
            return Json("");
        }

        [HttpPost]
        public JsonResult UpdateStatus(int? contactId, bool isActive, int firmUserGroupId/*, string email*/)
        {
            //var user = UserManagerFactory.FindByEmail(email);
            //var password = PasswordGenerator.GetPass();
            //user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(password);
            //var urlToFollow = this.Url.Action("Login", "Account", new { Area = "" }, this.Request.Url.Scheme);
            //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            ////int aliveHour = 12;
            //if (isActive)
            //{
            //    //await EmailService.SendActivationMail(user.Email, password, baseUrl, aliveHour);
            //}
            //user.is_active = isActive;

            //var result = await UserManagerFactory.UpdateAsync(user);
            //if (result.Succeeded)
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}
            //return Json(false, JsonRequestBehavior.AllowGet);
            _managerService.UpdateStatus(contactId, isActive);
            return Json("");
        }

        [HttpPost]
        public JsonResult SendEmailNotifications(int? contactId, bool sendNotifications)
        {
            _managerService.SendEmailNotifications(contactId, sendNotifications);
            return Json("");
        }

        [HttpPost]
        public JsonResult PostEqtyWriteUps(int? contactId, bool post)
        {
            _managerService.PostEqtyWriteUps(contactId, post);
            return Json("");
        }

        public ActionResult ActivateUser()
        {
            var view = new GenericViewModel();
            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }

        [HttpPost]
        public async Task<JsonResult> ActivateUser(string email, bool activate)
        {
            var user = UserManagerFactory.FindByEmail(email);
            var password = PasswordGenerator.GetPass();
            user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(password);
            var urlToFollow = this.Url.Action("Login", "Account", new { Area = "" }, this.Request.Url.Scheme);
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            var result = new IdentityResult();
            var prefix = "sendedMessage";
            string cacheKey = prefix + user.Id;
            DateTime dateTimeMessage = DateTime.Now;

            var checkLastMessage = CacheEmailWrapper.Get(cacheKey);
            //var checkLastMessage = CacheWrapper.Get(cacheKey);
            //var currentTimeMessage = dateTimeMessage.AddHours(-12);
            //var currentTimeMessage = dateTimeMessage.AddMinutes(-10);

            if (checkLastMessage == null)
            {
                //if (activate)
                //{
                await EmailService.SendActivationMail(user.Email, password, baseUrl);
                //}
                //CacheWrapper.Insert(cacheKey, dateTimeMessage);
                CacheEmailWrapper.Insert(cacheKey, dateTimeMessage);

                user.is_active = activate;
                result = await UserManagerFactory.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (checkLastMessage != null)
            {
                //if ((DateTime)checkLastMessage < (currentTimeMessage))
                //{
                //await EmailService.SendActivationMail(user.Email, password, baseUrl, aliveHour);

                //CacheWrapper.Remove(cacheKey);
                //CacheWrapper.Insert(cacheKey, dateTimeMessage);
                //CacheEmailWrapper.Remove(cacheKey);
                //CacheEmailWrapper.Insert(cacheKey, dateTimeMessage);

                //user.is_active = activate;
                //result = await UserManagerFactory.UpdateAsync(user);
                //if (result.Succeeded)
                //{
                //    return Json(true, JsonRequestBehavior.AllowGet);
                //}
                return Json(false, JsonRequestBehavior.AllowGet);
                //}
                //return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostEconomicCommentary(int? contactId, bool economicCommentary)
        {
            _managerService.PostEconomicCommentary(contactId, economicCommentary);
            return Json("");
        }

        [HttpPost]
        public async Task<JsonResult> ChangeTwoFactorAuth(int? contactId, int twoFactorAuthInt, string email)
        {
            TwoFactorAuth twoFactorAuth = (TwoFactorAuth)twoFactorAuthInt;
            var user = await UserManagerFactory.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("TwoFactorAuth", "User not found.");
                return Json(new { Error = "User not found." });
            }
            if (twoFactorAuth == TwoFactorAuth.TextMessage && String.IsNullOrEmpty(user.PhoneNumber))
            {
                ModelState.AddModelError("TwoFactorAuth", "Phone Number is empty.");
                return Json(new { Error = "Phone Number is empty." });
            }
            if (twoFactorAuth == TwoFactorAuth.Email && String.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("TwoFactorAuth", "Email is empty.");
                return Json(new { Error = "Email is empty." });
            }
            _administratorService.SetTwoFactorAuth(user.Id, twoFactorAuth);
            return Json("");
        }

        [HttpPost]
        public async Task<JsonResult> ChangePhoneNumber(int? contactId, string phoneNumber, string email)
        {
            var user = await UserManagerFactory.FindByEmailAsync(email);
            if (user != null)
            {
                phoneNumber = phoneNumber.Trim();
                phoneNumber = Regex.Replace(phoneNumber, @"\s+", String.Empty);
                var token = await UserManagerFactory.GenerateChangePhoneNumberTokenAsync(user.Id, phoneNumber);
                var result = await UserManagerFactory.ChangePhoneNumberAsync(user.Id, phoneNumber, token);
            }
            return Json("");
        }
    }
}