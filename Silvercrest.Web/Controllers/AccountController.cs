using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Silvercrest.ViewModels.Main.AccountViewModels;
using System.Data.Entity.Validation;
using Silvercrest.Utilities;
using Silvercrest.Services.CommonServices;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.IdentityStore;
using Silvercrest.Services;
using System.Text.RegularExpressions;
using Silvercrest.DataAccess.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Web.Routing;
using Silvercrest.Web.Common;
using System.Threading;
using Silvercrest.ViewModels;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Helpers.Assembly;
using Silvercrest.Services.CommonProviders;
using Silvercrest.ViewModels.Common.Constants;
using Silvercrest.Interfaces;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SLVR_DEVEntities _context;
        private UserManager<AspNetUser, string> _userManagerFactory;
        private AdministratorService _userService;
        private readonly AnalyticsService _analyticsService;
        private ContactRepository _contactRepository;

        public AccountController(SLVR_DEVEntities context)
        {
            _context = context;
            _userService = new AdministratorService(_context);
            _analyticsService = new AnalyticsService(_context);
            _contactRepository = new ContactRepository(_context);
        }

        private UserManager<AspNetUser, string> Create()
        {
            var userManager = new UserManager<AspNetUser, string>(new UserStore(HttpContext.GetOwinContext().Get<SLVR_DEVEntities>()));
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Silvercrest");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<Silvercrest.DataAccess.Model.AspNetUser>(provider.Create("PasswordReset"));
            userManager.UserValidator = new UserValidator<AspNetUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
            userManager.RegisterTwoFactorProvider(NotificationProviderConstants.PhoneNumberProvider, new PhoneNumberTokenProvider<AspNetUser>
            {
                MessageFormat = "This is your Silvercrest Verification Code: {0}"
            });
            userManager.SmsService = new SmsService();
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

        #region
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!User.Identity.IsAuthenticated)
            {
                InitializeCookies();
            }
            return View(new LoginViewModel { Email = Request.Cookies["RememberLogin"] != null ? Request.Cookies["RememberLogin"].Value : "" });
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [LoginAntiforgeryHandleError]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            HttpBrowserCapabilitiesBase browser = Request.Browser;
            //            var hash = Common.Hash.GetHash();
            Response.Cookies["IsFirstLogin"].Value = "false";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AspNetUser user = await UserManagerFactory.FindAsync(model.Email, model.Password);
            
            if (user != null)
            {
                Response.Cookies["IsFirstLogin"].Value = "true";
                Response.Cookies["IsSecondaryClient"].Value = "false";
                if (user.accepted_terms != null)
                    Response.Cookies["IsFirstLogin"].Value = "false";

                if (user.is_active == false)
                {
                    ModelState.AddModelError("", "Please contact your Silvercrest team");
                    return View(model);
                }

                var IsTrustedDevice = Request.Cookies["IsTrustedDevice"] != null? Request.Cookies["IsTrustedDevice"].Value.ToString():"none";

                if (Decryptdata(IsTrustedDevice) != user.Email.ToLower() && (user.TwoFactorAuth != (int)TwoFactorAuth.Inactive && !model.TokenVerified))
                {
                    var code = await UserManagerFactory.GenerateTwoFactorTokenAsync(user.Id, "PhoneCode");
                    ViewBag.ReturnUrl = returnUrl;

                    if (user.TwoFactorAuth == (int)TwoFactorAuth.TextMessage && !String.IsNullOrEmpty(user.PhoneNumber))
                    {
                        await UserManagerFactory.NotifyTwoFactorTokenAsync(user.Id, "PhoneCode", code);
                    }
                    if (user.TwoFactorAuth == (int)TwoFactorAuth.Email)
                    {
                        await EmailService.SendTwoFactorAuthCodeEmail(user.Email, code);
                    }

                    var maxAttemps = 5;
                    var attempts = maxAttemps;
                    if (Request.Cookies["VerifyAttempt"] != null && Request.Cookies["VerifyAttempt"].Value != "")
                    {
                        attempts = int.Parse(Request.Cookies["VerifyAttempt"].Value);
                        if (attempts == 0)
                        {
                            Response.Cookies["VerifyAttempt"].Value = maxAttemps.ToString();
                            return RedirectToAction("VerifyAttempt", "Account");
                        }
                    }
                    else
                    {
                        Response.Cookies["VerifyAttempt"].Value = (attempts - 1).ToString();
                    }

                    return View("TwoFactAuth", new VerifyTwoFactorTokenViewModel()
                    {
                        Email = Common.Hash.GetEncryptedValue(model.Email),
                        Password = Encryptdata(model.Password),
                        IsTokenError = false,
                        VerifyTwoFactorToken = String.Empty,
                        ReturnUrl = returnUrl
                    });

                    //return View("TwoFactAuth", new VerifyTwoFactorTokenViewModel()
                    //{
                    //    Email = model.Email,
                    //    Password = model.Password,
                    //    IsTokenError = false,
                    //    VerifyTwoFactorToken = String.Empty,
                    //    ReturnUrl = returnUrl
                    //});
                }
                if (model.TokenVerified)
                {
                    var updateResult = await UserManagerFactory.UpdateSecurityStampAsync(user.Id);
                    if (!updateResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Can`t update security stamp");
                        return View(model);
                    }
                }
                await SignInAsync(user, false);
                
                if (user.needs_password_generate == true)
                {
                    return RedirectToAction("ChangePassworForFirstLogin", "Account", new { userEmail = model.Email });
                }
                var c = _userService.GetAllUserRoles(user.contact_id);
                if (c.Count == 0)
                {
                    UserManagerFactory.AddToRole(user.Id, "Client");
                }
                else
                {
                    Response.Cookies["UserRole"].Value = Common.PasswordValidator.ValidateRole(c);
                    user.last_visit = DateTime.UtcNow;
                    await UserManagerFactory.UpdateAsync(user);
                    //await SignInAsync(user, false);
                    //var c = await UserManagerFactory.GetRolesAsync(user.Id);
                    var previousLoginRecordId = Request.Cookies.Get("LoginRecord")?.Value;
                    var loginRecordId = _analyticsService.CreateLoginRecord(user.Id, previousLoginRecordId);
                    StoreValueIntoCookie("LoginRecord", loginRecordId.ToString());
                }
                //-- store contactId to cookies
                StoreIdToCookies(user.Email);
                StoreValueIntoCookie("FullName", _contactRepository.GetFullNameByEmail(user.Email));
                SaveAccountInfo();

                //if (c.IndexOf("Administrator") != -1 || c.IndexOf("SuperUser") != -1)//c[0] == "Administrator" || c[0] == "SuperUser"
                //{
                //    return RedirectToAction("Index", "Administrator", new { Area = "Administrator" });
                //}

                //if (c.IndexOf("TeamMember") != -1)//c[0] == "TeamMember"
                //{
                //    return RedirectToAction("Index", "Manager", new { Area = "TeamMember" });
                //}

                //if (c.IndexOf("Client") != -1)//c[0] == "Client"
                //{
                //    return RedirectToAction("Index", "Home", new { Area = "Client" });
                //}
                if (c.Contains(Entities.Enums.UserRole.Administrator) || c.Contains(Entities.Enums.UserRole.SuperUser))
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction("Index", "Administrator", new { Area = "Administrator" });
                }

                if (c.Contains(Entities.Enums.UserRole.TeamMember) || c.Contains(Entities.Enums.UserRole.SecondaryTeamMember))
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction("Index", "Manager", new { Area = "TeamMember" });
                }

                if (c.Contains(Entities.Enums.UserRole.Client))
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction("Index", "Home", new { Area = "Client", contactIdQuery = Common.Hash.GetEncryptedValue(user.contact_id.ToString()) });
                }

                if (c.Contains(Entities.Enums.UserRole.SecondaryClient))
                {
                    Response.Cookies["IsSecondaryClient"].Value = "true";
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction("Index", "Documents", new { Area = "Client", contactIdQuery = Common.Hash.GetEncryptedValue(user.contact_id.ToString()) });
                }
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Invalid login attempt. Please try again.");
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [LoginAntiforgeryHandleError]
        public async Task<ActionResult> VerifyTwoFactorToken(VerifyTwoFactorTokenViewModel model)
        {
            model.Email = Common.Hash.GetDecryptedValue(model.Email).ToUpper();
            var temp = Decryptdata(model.Password);
            model.Password = temp;
            var result = await TwoFactorSignIn(model);
            if (result == SignInStatus.Success)
            {
                if (model.IsTrustedDevice)
                {
                    HttpCookie myCookie = new HttpCookie("IsTrustedDevice");
                    DateTime now = DateTime.Now;
                    myCookie.Value = Encryptdata(model.Email.ToLower());
                    myCookie.Expires = now.AddMonths(1);
                    Response.Cookies.Add(myCookie);
                }

                return await Login(new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = true,
                    TokenVerified = true
                },
                    model.ReturnUrl);
            }
            else
            {
                model.IsTokenError = true;

                model.Email = Common.Hash.GetEncryptedValue(model.Email);
                model.Password = Encryptdata(model.Password);

                return View("TwoFactAuth", model);
            }
        }

        private async Task<SignInStatus> TwoFactorSignIn(VerifyTwoFactorTokenViewModel model)
        {
            var user = await UserManagerFactory.FindAsync(model.Email, model.Password);
            if (user == null)
            {
                return SignInStatus.Failure;
            }
            if (await UserManagerFactory.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }
            if (await UserManagerFactory.VerifyTwoFactorTokenAsync(user.Id, NotificationProviderConstants.PhoneNumberProvider, model.VerifyTwoFactorToken))
            {
                await UserManagerFactory.ResetAccessFailedCountAsync(user.Id);
                return SignInStatus.Success;
            }
            await UserManagerFactory.AccessFailedAsync(user.Id);
            return SignInStatus.Failure;
        }

        private async Task SignInAsync(AspNetUser user, bool isPersistent)
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            var identity = await UserManagerFactory.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ChangePassworForFirstLogin(string userEmail, string code, string alive)
        {
            ChangePasswordViewModel view = new ChangePasswordViewModel();
            string userEmailDecrypt = Decrypt(userEmail, "");
            if (DateTime.Now >= DateTime.Parse(HttpUtility.UrlDecode(Decrypt(alive, ""))))
            {
                view.Email = userEmailDecrypt;
                view.SecretQuestions = new List<Entities.SecretQuestion>();
                ModelState.AddModelError("", "This link is no longer active.");
                return View(view);
            }
            var password = System.Web.HttpUtility.UrlDecode(Decrypt(code, ""));
            var user = UserManagerFactory.Find(userEmailDecrypt, password);
            view.Email = userEmailDecrypt;
            view.OldPassword = password;
            view.SecretQuestions = _userService.GetQuestionList();
            return View(view);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ChangePassworForFirstLogin(ChangePasswordViewModel view)
        {
            LogOff();
            if (!ModelState.IsValid)
            {
                view.Email = User.Identity.Name;
                view.SecretQuestions = _userService.GetQuestionList();
                return View(view);
            }
            var validation = Common.PasswordValidator.ValidatePassword(view.NewPassword);

            var user = await UserManagerFactory.FindAsync(view.Email, view.OldPassword);

            if (user == null)
            {
                validation.Errors.Add("You have already setup your Password and Security Questions. Please use your existing credentials to login.");
                validation.IsValid = false;
            }

            if (!validation.IsValid)
            {
                view.Email = User.Identity.Name;
                view.SecretQuestions = _userService.GetQuestionList();
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(view);
            }
            user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(view.NewPassword);
            user.needs_password_generate = false;
            user.is_active = true;
            var result = await UserManagerFactory.UpdateAsync(user);
            if (user != null)
            {
                await SignInAsync(user, false);
            }
            SaveSecretQuestions(view);
            await SignInAsync(user, false);
            return RedirectToAction("Login", "Account");
        }
        private List<string> GetSecretQuestions(string webUserId)
        {
            var questionsId = _context.Web_Security_Answer
                .Where(a => a.web_user_id == webUserId)
                .Select(s => s.question_id).ToArray();
            questionsId = questionsId.Reverse().Take(3).ToArray();
            List<string> questions = new List<string>();
            foreach (var item in questionsId)
            {
                questions.AddRange(_context.Web_Security_Question
               .Where(x => item == x.id).Select(s => s.question).ToList());
            }
            questions.Reverse();
            return questions;
        }
        private void SaveSecretQuestions(ChangePasswordViewModel view)
        {
            var user = _context.AspNetUsers.Where(x => x.Email == view.Email).FirstOrDefault();

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
        // GET: /Account/VerifyAttempt

        [AllowAnonymous]
        public ActionResult VerifyAttempt()
        {
            return View();
        }
        // GET: /Account/ForgotPasswordInvalidAccount

        [AllowAnonymous]
        public ActionResult ForgotPasswordInvalidAccount()
        {
            return View();
        }
        // GET: /Account/ForgotPassword
        #endregion

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManagerFactory.FindByEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email");
                    //return View("ForgotPassword");
                    return RedirectToAction("ForgotPasswordInvalidAccount", "Account");
                }
                var questions = GetSecretQuestions(user.Id);
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);//
                if (questions.Count != 3)
                {
                    if (questions.Count == 0)
                    {
                        // If user has no security questions sent send this user a brand new “Activation Email”
                        var password = PasswordGenerator.GetPass();
                        user.PasswordHash = UserManagerFactory.PasswordHasher.HashPassword(password);
                        var result = await UserManagerFactory.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            await EmailService.SendActivationMail(user.Email, password, baseUrl);
                            return RedirectToAction("ForgotPasswordConfirmation", "Account");
                        }
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Something went wrong updating your data. Please, try again later.");
                            return View("ForgotPassword");
                        }
                    }
                    ModelState.AddModelError("", "Please, complete registration first");
                    return View("ForgotPassword");
                }
                int aliveHour = 3;
                var token = UserManagerFactory.GeneratePasswordResetToken(user.Id);//
                var retoken = System.Web.HttpUtility.UrlEncode(token);//

                //string code = UserManagerFactory.GeneratePasswordResetToken(user.Id);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol: Request.Url.Scheme);
                //await EmailService.SendEmailForgotPassword(user.Email, callbackUrl);

                await EmailService.SendEmailForgotPassword(user.Email, retoken, baseUrl, user.Id, aliveHour);//
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string alive, string id = null)
        {

            var user = await UserManagerFactory.FindByIdAsync(Decrypt(id, ""));
            var view = new ResetPasswordViewModel { Email = user.Email };
            if (DateTime.Now >= DateTime.Parse(HttpUtility.UrlDecode(Decrypt(alive, ""))))
            {
                view.SecretQuestion1 = "";
                view.SecretQuestion2 = "";
                view.SecretQuestion3 = "";
                ModelState.AddModelError("", "This link is no longer active.");
                return View(view);
            }
            var questions = GetSecretQuestions(user.Id);
            if (questions.Count > 2)
            {
                view.SecretQuestion1 = questions[0];
                view.SecretQuestion2 = questions[1];
                view.SecretQuestion3 = questions[2];
            }
            return code == null ? View("Error") : View(view);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Code = System.Web.HttpUtility.UrlDecode(Decrypt(model.Code, ""));
            var user = await UserManagerFactory.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View(model);
            }
            var validation = Common.PasswordValidator.ValidatePassword(model.Password);
            await ValidateQuestions(model, user.Id, validation);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
            Response.Cookies["secretQuestionsAttempts"].Value = null;
            var result = await UserManagerFactory.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View(model);
        }

        private async Task ValidateQuestions(ResetPasswordViewModel model, string id, PasswordValidationViewModel validation)
        {

            if (UserManagerFactory.FindById(id).is_active == false)
            {
                validation.IsValid = false;
                validation.Errors.Add("Your account is locked. Please contact your Silvercrest team to unlock it.");
                return;
            }
            var maxAttemps = 5;
            var attempts = maxAttemps;
            if (Request.Cookies["secretQuestionsAttempts"] != null && Request.Cookies["secretQuestionsAttempts"].Value != "")
            {
                attempts = int.Parse(Request.Cookies["secretQuestionsAttempts"].Value);
                if (attempts == 0)
                {
                    var user = UserManagerFactory.FindById(id);
                    user.is_active = false;
                    var result = await UserManagerFactory.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        validation.IsValid = false;
                        validation.Errors.Add("Error with update try again");
                        return;
                    }
                    Response.Cookies["secretQuestionsAttempts"].Value = maxAttemps.ToString();
                    _context.SaveChanges();
                    validation.IsValid = false;
                    validation.Errors.Add("You have no more attempts and your account is now locked. Please contact your Silvercrest team to unlock it.");
                    return;
                }
            }
            var answersHash = _context.Web_Security_Answer
               .Where(a => a.web_user_id == id)
               .Select(s => s.answer_hash)
               .ToArray();
            int correctAnswers = 0;
            if (answersHash[answersHash.Length - 3] == Encrypter.GetSHA512(model.QuestionAnswer1.ToLower()))
                correctAnswers++;
            if (answersHash[answersHash.Length - 2] == Encrypter.GetSHA512(model.QuestionAnswer2.ToLower()))
                correctAnswers++;
            if (answersHash[answersHash.Length - 1] == Encrypter.GetSHA512(model.QuestionAnswer3.ToLower()))
                correctAnswers++;


            if (correctAnswers != 3)
            {
                attempts--;
                validation.IsValid = false;
                validation.Errors.Add("Not all of your answers matched! You have " + attempts + " attempts left.");
                Response.Cookies["secretQuestionsAttempts"].Value = attempts.ToString();
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            var view = new GenericViewModel();
            view.contactFullName = "";//URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));

            return View(view);
        }

        ////
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var loginRecordId = Request.Cookies.Get("loginRecord")?.Value;
            _analyticsService.UpdateRecordOnLogout(loginRecordId);

            string[] myCookie = Request.Cookies.AllKeys;

            //foreach (string cookie in myCookie)
            //{
            //    if (cookie == "RememberLogin")
            //        continue;
            //    var kk = Request.Cookies[cookie];
            //    var ck = Response.Cookies[cookie];
            //    ck.Expires = DateTime.Now.AddDays(-1);
            //    ck.Value = "";
            //    Response.Cookies.Add(ck);
            //    kk.Expires = DateTime.Now.AddDays(-1);
            //    kk.Value = "";
            //    Request.Cookies.Add(kk);
            //}

            foreach (string cookie in Request.Cookies.AllKeys)
            {
                if (cookie == "RememberLogin")
                    continue;
                var names = Request.Cookies[cookie].Name;
                HttpCookie httpCookie = new HttpCookie(cookie);
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(httpCookie);
            }

            Request.Cookies.Clear();   //Does not work on clients side
            Response.Cookies.Clear();   //Does not work on clients side
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManagerFactory != null)
                {
                    _userManagerFactory.Dispose();
                    _userManagerFactory = null;
                }
            }
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void StoreValueIntoCookie(string key, string value)
        {
            Response.Cookies[key].Value = value;
        }

        private void StoreIdToCookies(string email)
        {
            var contactId = _context.AspNetUsers
                      .Where(y => y.Email == email)
                      .Select(y => y.contact_id)
                      .FirstOrDefault();
            HttpCookie myCookie = new HttpCookie("MainContactId");
            DateTime now = DateTime.Now;
            myCookie.Value = contactId.ToString();
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("RememberLogin");
            now = DateTime.Now;
            myCookie.Value = email.ToString();
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("contactId");
            now = DateTime.Now;
            myCookie.Value = contactId.ToString();
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);
        }

        public void SaveAccountInfo(bool? isGroup = null, bool? isClientGroup = null, int? contactId = null, int? entityId = null)
        {
            try
            {
                Response.Cookies["accountContactId"].Value = contactId.ToString();
                Response.Cookies["accountEntityId"].Value = entityId.ToString();
                Response.Cookies["accountIsGroup"].Value = isGroup.ToString();
                Response.Cookies["accountIsClientGroup"].Value = isClientGroup.ToString();
            }
            catch (Exception e) { }
        }

        [HttpGet]
        public JsonResult GetNameIfClientView(string contactIdQuery)
        {
            Thread.Sleep(100);
            int? contactId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? (int?)null : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(contactIdQuery));
            return Json(_contactRepository.GetFullNameByContactId(contactId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRole(int? contactId = null)
        {
            var c = _userService.GetAllUserRoles((int)contactId);
            return Json(c.FirstOrDefault().ToString(), JsonRequestBehavior.AllowGet);
        }



        public static string Decrypt(string cipherText, string passPhrase)
        {
            passPhrase = "b87df5f7-94fe-4b86-97d6-6d9c024b67cb";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(passPhrase, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private void InitializeCookies()
        {
            HttpCookie myCookie = new HttpCookie("MainContactId");
            DateTime now = DateTime.Now;
            myCookie.Value = "";
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("RememberLogin");
            now = DateTime.Now;
            myCookie.Value = "";
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("contactId");
            now = DateTime.Now;
            myCookie.Value = "";
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("UserRole");
            now = DateTime.Now;
            myCookie.Value = "";
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);

            myCookie = new HttpCookie("IsFirstLogin");
            now = DateTime.Now;
            myCookie.Value = "";
            myCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(myCookie);
        }

        public static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

    }


    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
      Justification = "This attribute is AllowMultiple = true and users might want to override behavior.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LoginAntiforgeryHandleErrorAttribute : FilterAttribute, IExceptionFilter
    {
        #region Implemented Interfaces

        #region IExceptionFilter

        /// <summary>
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.IsChildAction)
            {
                return;
            }

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            Exception exception = filterContext.Exception;

            // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
            // ignore it.
            if (new HttpException(null, exception).GetHttpCode() != 500)
            {
                return;
            }

            // check if antiforgery
            if (!(exception is HttpAntiForgeryException))
            {
                return;
            }

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "action", "Login" },
                });

            filterContext.ExceptionHandled = true;
        }

        #endregion

        #endregion
    }
}