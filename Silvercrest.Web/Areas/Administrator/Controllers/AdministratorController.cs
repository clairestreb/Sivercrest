using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Silvercrest.DataAccess.IdentityStore;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities.Enums;
using Silvercrest.Interfaces;
using Silvercrest.Services.CommonServices;
using Silvercrest.Utilities;
using Silvercrest.ViewModels;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Common;
using Silvercrest.Web.Helpers.Authorize;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Areas.Administrator.Controllers
{
    [AuthorizeRoles(UserRole.SuperUser, UserRole.Administrator)]
    public class AdministratorController : Controller
    {
        private SLVR_DEVEntities _context;
        private IAdministratorService _administratorService;
        private UserManager<AspNetUser, string> _userManagerFactory;

        public AdministratorController(IAdministratorService service, SLVR_DEVEntities context)
        {
            _context = context;
            _administratorService = service;
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

        public ActionResult Index()
        {
            string contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            var view = new GenericViewModel();
            view.contactFullName = contactFullName;
            return View(view);
        }

        public ActionResult CreateUser()
        {
            var view = new SetupUserViewModel();

            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }

        [HttpPost]
        public JsonResult CreateUser(string contactEmail, int contactId)
        {
            var usersByContactId = _context.AspNetUsers.ToList()
                .FindAll(x => x.contact_id == contactId)
                .Select(x => x.is_active).ToList();

            var usersByEmail = _context.AspNetUsers.ToList()
                .FindAll(x => x.Email == contactEmail)
                .Select(x => x.is_active).ToList();

            if (usersByContactId != null && usersByContactId.Find(x => x == true))
                return Json("contact", JsonRequestBehavior.AllowGet);

            if (usersByEmail != null && usersByEmail.Count > 0)
                return Json("email", JsonRequestBehavior.AllowGet);

            var role = _administratorService.GetUserRole(contactId);
            var contact = _context.Contacts.Where(x => x.id == contactId).FirstOrDefault();
            if (contact == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var urlToFollow = this.Url.Action("Login", "Account", new { Area = "" }, this.Request.Url.Scheme);
            var password = PasswordGenerator.GetPass();

            var user = new AspNetUser();
            user.Id = Guid.NewGuid().ToString();
            user.is_active = false;
            user.needs_password_generate = true;
            user.Email = contactEmail;
            user.UserName = contactEmail;
            user.contact_id = contact.id;
            user.insert_by = _administratorService.GetFullName(User.Identity.Name);
            user.insert_date = DateTime.UtcNow;
            user.receives_equity_writeups = true;
            user.gets_email_notifications = true;
            user.receives_econ_commentary = true;
            var result = UserManagerFactory.Create(user, password);
            _context.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCustomers(string prefix)
        {
            return Json(_administratorService.GetContactsByEmailPrefix(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContacts()
        {
            var list = _administratorService.GetContacts();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivateUser()
        {
            var view = new GenericViewModel();
            view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            return View(view);
        }

        public JsonResult GetUsers()
        {
            return Json(_administratorService.GetUsers(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ActivateUser(string id, bool activate)
        {
            var user = UserManagerFactory.FindById(id);
            var password = PasswordGenerator.GetPass();
            user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(password);
            var urlToFollow = this.Url.Action("Login", "Account", new { Area = "" }, this.Request.Url.Scheme);
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);//
            if (activate)
            {
                await EmailService.SendActivationMail(user.Email, password, baseUrl);
            }
            user.is_active = activate;
            var result = await UserManagerFactory.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ContactSettings(string email)
        {
            var user = await UserManagerFactory.FindByEmailAsync(email);
            if (user == null)
            {
                return View(new ViewModels.Administrator.ContactSettingsInfo());
            }
            var contactSettingsInfo = _administratorService.FillInInfo(user.contact_id);
            contactSettingsInfo.contactFullName = URLParameters.GetContactFullName(user.contact_id);
            contactSettingsInfo.TwoFactorAuth = (TwoFactorAuth)_administratorService.GetTwoFactorAuthById(user.Id);
            contactSettingsInfo.PhoneNumber = await UserManagerFactory.GetPhoneNumberAsync(user.Id);
            return View(contactSettingsInfo);
        }

        [HttpPost]
        public JsonResult UpdateEmail(int? contactId, string email)
        {
            _administratorService.UpdateEmail(contactId, email);
            return Json("");
        }

        [HttpPost]
        public JsonResult UpdateStatus(int? contactId, bool isActive)
        {
            _administratorService.UpdateStatus(contactId, isActive);
            return Json("");
        }

        [HttpPost]
        public async Task<JsonResult> ActivateUserContact(string email, bool activate)
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

            if (checkLastMessage == null)
            {
                await EmailService.SendActivationMail(user.Email, password, baseUrl);
                CacheEmailWrapper.Insert(cacheKey, dateTimeMessage);

                user.is_active = activate;
                result = await UserManagerFactory.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
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