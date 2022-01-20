//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Silvercrest.DataAccess;
//using Silvercrest.DataAccess.Model;
//using Silvercrest.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Silvercrest.Services.Main.Identity
//{
//    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
//    public class ApplicationUserService : UserManager<Entities.ApplicationUser>
//    {
//        public ApplicationUserService(IUserStore<Entities.ApplicationUser> store)
//            : base(store)
//        {
//        }

//        public static ApplicationUserService Create(Microsoft.AspNet.Identity.Owin.IdentityFactoryOptions<ApplicationUserService> options, IOwinContext context)
//        {
//            var manager = new ApplicationUserService(new UserStore<Entities.ApplicationUser>(context.Get<SLVR_DEVEntities1>()));
//            // Configure validation logic for usernames
//            manager.UserValidator = new UserValidator<Entities.ApplicationUser>(manager)
//            {
//                AllowOnlyAlphanumericUserNames = false,
//                RequireUniqueEmail = true
//            };

//            // Configure validation logic for passwords
//            manager.PasswordValidator = new PasswordValidator
//            {
//                RequiredLength = 6,
//                RequireNonLetterOrDigit = true,
//                RequireDigit = true,
//                RequireLowercase = true,
//                RequireUppercase = true,
//            };

//            // Configure user lockout defaults
//            manager.UserLockoutEnabledByDefault = true;
//            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
//            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

//            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
//            // You can write your own provider and plug it in here.
//            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<Entities.ApplicationUser>
//            {
//                MessageFormat = "Your security code is {0}"
//            });
//            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Entities.ApplicationUser>
//            {
//                Subject = "Security Code",
//                BodyFormat = "Your security code is {0}"
//            });
//            var dataProtectionProvider = options.DataProtectionProvider;
//            if (dataProtectionProvider != null)
//            {
//                manager.UserTokenProvider =
//                    new DataProtectorTokenProvider<Entities.ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
//            }
//            return manager;
//        }
//    }
//}
