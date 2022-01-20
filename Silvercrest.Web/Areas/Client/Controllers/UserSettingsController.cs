using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Silvercrest.DataAccess.IdentityStore;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client.UserSettings;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Maintance;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Authorize]
    [IgnoreAnalytics]
    [Maintance]
    public class UserSettingsController : BaseController
    {
        private ContactRepository _contactRepository;
        private readonly SLVR_DEVEntities _context;
        private AdministratorService _userService;
        private UserManager<AspNetUser, string> _userManagerFactory;
        private IClientAdoService _service;

        public UserSettingsController(SLVR_DEVEntities context, IClientAdoService service)
        {
            _context = context;
            _contactRepository = new ContactRepository(_context);
            _userService = new AdministratorService(_context);
            _service = service;
        }

        private UserManager<AspNetUser, string> Create()
        {
            var userManager = new UserManager<AspNetUser, string>(new UserStore(HttpContext.GetOwinContext().Get<SLVR_DEVEntities>()));
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Silvercrest");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<Silvercrest.DataAccess.Model.AspNetUser>(provider.Create("PasswordReset"));
            userManager.UserValidator = new UserValidator<AspNetUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
            return userManager;
        }

        public UserManager<AspNetUser, string> UserManagerFactory
        {
            get
            {
                return Create();
            }
            private set
            {
                _userManagerFactory = value;
            }
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

        public ActionResult Index(string mainContactIdQuery, string contactIdQuery)
        {
            int? mainContactId;
            int? contactId;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? int.Parse(Request.Cookies["contactId"].Value) : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(contactIdQuery));
            mainContactId = string.IsNullOrEmpty(mainContactIdQuery) || mainContactIdQuery == "undefined" ? (int?)null : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(mainContactIdQuery));
            var view = new IndexViewModel();
            List<ClientAccount> list = new List<ClientAccount>();

            ContactId = GetContactId(null);
            var user_role = GetUserRoles();
            if (!user_role)
            {
                if (contactId != ContactId)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                }
            }

            view.ShowCustomizeSection = true;
            if (mainContactId == contactId)
            {
                view.ShowSecuritySection = true;
                //This is  Team Member, not a client
                if (Session[contactId.ToString()] == null)
                {
                    view.ShowCustomizeSection = false;
                }
            }
            else
            {
                view.ShowSecuritySection = false;
            }

            view.FullName = _contactRepository.GetFullNameByContactId(contactId);
            view.SecretQuestions = _userService.GetQuestionList();
            _service.ContactEntities(list, (int)contactId);
            view.defaultGroup = (string)list.Where(l => l.IsDefault).Select(d => d.CompositeGroupId).FirstOrDefault();
            view.GroupList = list;

            var res = _context.Web_User_Settings.Where(u => u.contact_id == contactId).FirstOrDefault();
            if (res == null)
            {
                view.CurrentUnsupervisedValue = false.ToString();
            }
            else
            {
                view.CurrentUnsupervisedValue = res.unsupervised_assets.ToString();
            }

            if (Session[contactId.ToString()] == null)
            {
                view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            }
            else
            {
                view.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;
            }
            return View(view);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeTwoFactorAuth(IndexViewModel view)
        {
            _contactRepository.SetTwoFactorAuth(User.Identity.GetUserId(), view.TwoFactorAuth);
            return Json(new { result = "2FA has been changed succesfully" });
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(IndexViewModel view)
        {
            if (string.IsNullOrWhiteSpace(view.CurrentPassword))
            {
                return Json(new { result = "Please enter current password" });
            }

            var user = await UserManagerFactory.FindAsync(User.Identity.Name, view.CurrentPassword);
            if (user == null)
            {
                return Json(new { result = "Current password is incorrect!" });
            }

            if (string.IsNullOrWhiteSpace(view.NewPassword))
            {
                return Json(new { result = "Please enter new password" });
            }
            var validation = Common.PasswordValidator.ValidatePassword(view.NewPassword);

            if (!validation.IsValid)
            {
                return Json(new { result = "New password not accepted" });
            }

            if (view.NewPassword != view.ConfirmPassword)
            {
                return Json(new { result = "New passwords must match" });
            }

            user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(view.NewPassword);
            user.needs_password_generate = false;
            user.is_active = true;
            var result = await UserManagerFactory.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Json(new { result = "User update was failed. Try again" });
            }
            return Json(new { result = "Password has been changed succesfully" });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeQuestions(IndexViewModel view)
        {
            if (string.IsNullOrEmpty(view.CurrentPassword))
            {
                return Json(new { result = "Please enter current password" });
            }
            if (!ModelState.IsValid)
            {
                return Json(new { result = "Not all questions have answers" });
            }
            var user = await UserManagerFactory.FindAsync(User.Identity.Name, view.CurrentPassword);

            if (user == null)
            {
                return Json(new { result = "Current password is incorrect!" });
            }
            SaveSecretQuestions(view);
            return Json(new { result = "Security Questions have been changed succesfully!" });
        }

        [HttpPost]
        public JsonResult UpdateGroup(IndexViewModel view)
        {
            //            if (string.IsNullOrEmpty(Request.Cookies["ContactId"].Value))
            //            {
            //               return Json(new { result = "Error" });
            //           }
            //            List<ClientAccount> list = new List<ClientAccount>();
            //           var id = int.Parse(Request.Cookies["ContactId"].Value);
            //           _service.ContactEntities(list, id);
            var contactId = string.IsNullOrEmpty(view.ContactIdQuery) || view.ContactIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(view.ContactIdQuery));

            //            var group = list.Where(a => a.EntityId == view.SelectedGroup).FirstOrDefault();
            var composite = view.SelectedGroup;
            int entityId = int.Parse(composite.Split('_')[0]);
            bool isClientGroup = bool.Parse(composite.Split('_')[1]);

            var res = _context.Web_Contact_Default_Entity.Where(e => e.contact_id == contactId).FirstOrDefault();

            if (res == null)
            {
                _context.Web_Contact_Default_Entity.Add(new Web_Contact_Default_Entity()
                {
                    contact_id = (int)contactId,
                    entity_id = entityId,
                    is_client_group = isClientGroup,
                    insert_date = DateTime.Now,
                    insert_by = view.FullName,
                    is_group = true
                });
            }
            else
            {
                res.contact_id = (int)contactId;
                res.entity_id = entityId;
                res.is_client_group = isClientGroup;
                res.update_date = DateTime.Now;
                res.update_by = view.FullName;
                res.is_group = true;

            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return Json(new { result = "Error during update" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "Default Group updated!" });
        }

        public JsonResult UpdateAsset(IndexViewModel view)
        {
            var contactId = string.IsNullOrEmpty(view.ContactIdQuery) || view.ContactIdQuery == "undefined" ? (int?)null : int.Parse(Common.Hash.GetDecryptedValue(view.ContactIdQuery));
            var res = _context.Web_User_Settings.Where(u => u.contact_id == contactId).FirstOrDefault();
            if (res == null)
            {
                _context.Web_User_Settings.Add(new Web_User_Settings()
                {
                    contact_id = (int)contactId,
                    unsupervised_assets = bool.Parse(view.NewUnsupervisedValue),
                });

            }
            else
            {
                res.unsupervised_assets = bool.Parse(view.NewUnsupervisedValue);
            }
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return Json(new { result = "Error during update" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "Settings Updated!" }, JsonRequestBehavior.AllowGet);
        }



        private void SaveSecretQuestions(IndexViewModel view)
        {
            var user = _context.AspNetUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            //Clear existing Security Questions to avoid user having 6, 9, 12 questions and answers
            _context.Web_Security_Answer.RemoveRange(_context.Web_Security_Answer.Where(x => x.web_user_id == user.Id));
            _context.SaveChanges();

            var secretQuestionApplicationUserAnswerInSecretQuestion1 = new Web_Security_Answer();
            secretQuestionApplicationUserAnswerInSecretQuestion1.id = 0;
            secretQuestionApplicationUserAnswerInSecretQuestion1.AspNetUser = user;
            secretQuestionApplicationUserAnswerInSecretQuestion1.web_user_id = user.Id;
            secretQuestionApplicationUserAnswerInSecretQuestion1.insert_by = string.Empty;
            secretQuestionApplicationUserAnswerInSecretQuestion1.insert_date = DateTime.UtcNow;
            secretQuestionApplicationUserAnswerInSecretQuestion1.Web_Security_Question = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId1).FirstOrDefault();
            secretQuestionApplicationUserAnswerInSecretQuestion1.question_id = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId1).FirstOrDefault().id;
            secretQuestionApplicationUserAnswerInSecretQuestion1.answer_hash = Encrypter.GetSHA512(view.QuestionAnswer1.ToLower());
            _context.Web_Security_Answer.Add(secretQuestionApplicationUserAnswerInSecretQuestion1);

            var secretQuestionApplicationUserAnswerInSecretQuestion2 = new Web_Security_Answer();
            secretQuestionApplicationUserAnswerInSecretQuestion2.id = 1;
            secretQuestionApplicationUserAnswerInSecretQuestion2.AspNetUser = user;
            secretQuestionApplicationUserAnswerInSecretQuestion2.web_user_id = user.Id;
            secretQuestionApplicationUserAnswerInSecretQuestion2.insert_by = string.Empty;
            secretQuestionApplicationUserAnswerInSecretQuestion2.insert_date = DateTime.UtcNow;
            secretQuestionApplicationUserAnswerInSecretQuestion2.Web_Security_Question = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId2).FirstOrDefault();
            secretQuestionApplicationUserAnswerInSecretQuestion2.question_id = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId2).FirstOrDefault().id;
            secretQuestionApplicationUserAnswerInSecretQuestion2.answer_hash = Encrypter.GetSHA512(view.QuestionAnswer2.ToLower());
            _context.Web_Security_Answer.Add(secretQuestionApplicationUserAnswerInSecretQuestion2);

            var secretQuestionApplicationUserAnswerInSecretQuestion3 = new Web_Security_Answer();
            secretQuestionApplicationUserAnswerInSecretQuestion3.id = 2;
            secretQuestionApplicationUserAnswerInSecretQuestion3.AspNetUser = user;
            secretQuestionApplicationUserAnswerInSecretQuestion3.web_user_id = user.Id;
            secretQuestionApplicationUserAnswerInSecretQuestion3.insert_by = string.Empty;
            secretQuestionApplicationUserAnswerInSecretQuestion3.insert_date = DateTime.UtcNow;
            secretQuestionApplicationUserAnswerInSecretQuestion3.Web_Security_Question = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId3).FirstOrDefault();
            secretQuestionApplicationUserAnswerInSecretQuestion3.question_id = _context.Web_Security_Question.Where(x => x.id == view.SecretQuestionsId3).FirstOrDefault().id;
            secretQuestionApplicationUserAnswerInSecretQuestion3.answer_hash = Encrypter.GetSHA512(view.QuestionAnswer3.ToLower());
            _context.Web_Security_Answer.Add(secretQuestionApplicationUserAnswerInSecretQuestion3);
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
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