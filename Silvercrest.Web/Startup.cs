using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using Hangfire;
using System.Web.Routing;
using Silvercrest.Web.App_Start;
using Silvercrest.Entities;
using Silvercrest.DataAccess;
using Silvercrest.DataAccess.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Silvercrest.DataAccess.IdentityStore;
using Microsoft.Owin.Security.OAuth;
using Silvercrest.Web.Common;
using System.Data.Entity.Validation;
using Silvercrest.Services.CommonServices;
using System.Configuration;
using Hangfire.SqlServer;
using Silvercrest.Services.Client;
using Silvercrest.Interfaces.Client;

[assembly: OwinStartupAttribute(typeof(Silvercrest.Web.Startup))]
namespace Silvercrest.Web
{
    
    public partial class Startup
    {
        public Startup()
        {
            
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //CreateSecretQuestions();
            //var a = Common.Hash.hash;
            //var options = new SqlServerStorageOptions
            //{
            //    PrepareSchemaIfNecessary = true
            //};
            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("Database", options);
            //using (var server = new BackgroundJobServer())
            //{
            //    //RecurringJob.AddOrUpdate(() => _documentsService.ProceedZips(), Cron.Minutely);
            //    RecurringJob.AddOrUpdate(() => ScheduleService.ProceedZips(), Cron.Minutely);
            //}

            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
        }
        
        private void CreateSecretQuestions()
        {
            int i = 0;
            var context = new SLVR_DEVEntities();
            var questions = context.Web_Security_Question.Any();
            if (questions)
            {
                return;
            }
            List<string> questionsList = new List<string>{
                    "What was your childhood nickname?",
                    "In what city did you meet your spouse / significant other?",
                    "What is the name of your favorite childhood friend?",
                    "What street did you live on in third grade?",
                    "What is your oldest sibling’s birthday month and year? (e.g., January 1900)",
                    "What is the middle name of your oldest child?",
                    "What is your oldest sibling's middle name?",
                    "What school did you attend for sixth grade?",
                    "What was your childhood phone number including area code? (e.g., 000 - 000 - 0000)",
                    "What is your oldest cousin's first and last name?",
                    "What was the name of your first stuffed animal?",
                    "In what city or town did your mother and father meet?",
                    "Where were you when you had your first kiss?",
                    "What is the first name of the boy or girl that you first kissed?",
                    "What was the last name of your third grade teacher?",
                    "In what city does your nearest sibling live?",
                    "What is your oldest brother’s birthday month and year? (e.g., January 1900)",
                    "What is your maternal grandmother's maiden name?",
                    "In what city or town was your first job?",
                    "What is the name of the place your wedding reception was held?",
                    "What is the name of a college you applied to but didn't attend?",
                    "Where were you when you first heard about 9/11?"};

            foreach (var question in questionsList)
            {
                var secretQuestion =  new Web_Security_Question();
                //secretQuestion.id = i;
                i++;
                secretQuestion.question = question;
                secretQuestion.insert_by = "CPU";
                secretQuestion.insert_date = DateTime.UtcNow;
                secretQuestion.update_by = null;
                secretQuestion.update_date = null;
                secretQuestion.display_order = i;
                context.Web_Security_Question.Add(secretQuestion);
            }
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string s, g;
                foreach (var eve in e.EntityValidationErrors)
                {
                    s = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        g = String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            app.CreatePerOwinContext<SLVR_DEVEntities>(() =>new SLVR_DEVEntities());
            app.CreatePerOwinContext<UserManager<AspNetUser, string>>(
                (IdentityFactoryOptions<UserManager<AspNetUser, string>> options, IOwinContext context) =>
                    new UserManager<AspNetUser, string>(new UserStore(context.Get<SLVR_DEVEntities>())));


            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(SLVR_DEVEntities.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
                //ExpireTimeSpan = DateTime.Now.Subtract(DateTime.UtcNow).Add(TimeSpan.FromDays(30))
        });
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AspNetUser>(
            //        //    validateInterval: TimeSpan.FromMinutes(30),
            //        //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});
           // app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
