using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using Silvercrest.DataAccess.IdentityStore;
using Silvercrest.DataAccess.Model;

[assembly: OwinStartupAttribute(typeof(test.Startup))]
namespace test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<SLVR_DEVEntities2>(() => new SLVR_DEVEntities2());
            app.CreatePerOwinContext<UserManager<AspNetUser, string>>(
                (IdentityFactoryOptions<UserManager<AspNetUser, string>> options, IOwinContext context) =>
                    new UserManager<AspNetUser, string>(new UserStore(context.Get<SLVR_DEVEntities2>())));

            ConfigureAuth(app);

        }
    }
}
