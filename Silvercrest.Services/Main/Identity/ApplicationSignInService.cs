//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Microsoft.Owin.Security;
//using Silvercrest.DataAccess.Model;
//using Silvercrest.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace Silvercrest.Services.Main.Identity
//{
//    // Configure the application sign-in manager which is used in this application.
//    public class ApplicationSignInService : SignInManager<ApplicationUser, string>
//    {
//        public ApplicationSignInService(ApplicationUserService userService, IAuthenticationManager authenticationManager)
//            : base(userService, authenticationManager)
//        {
//        }

//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
//        {
//            return user.GenerateUserIdentityAsync((ApplicationUserService)UserManager);
//        }

//        public static ApplicationSignInService Create(IdentityFactoryOptions<ApplicationSignInService> options, IOwinContext context)
//        {
//            return new ApplicationSignInService(context.GetUserManager<ApplicationUserService>(), context.Authentication);
//        }
//    }
//}
