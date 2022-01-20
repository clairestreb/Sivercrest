[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Silvercrest.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Silvercrest.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Silvercrest.Web.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Interfaces;
    using DataAccess;
    using Entities;
    using Services;
    using DataAccess.Model;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using Helpers.Analytics;
    using Interfaces.TeamMember;
    using Services.TeamMember;
    using Interfaces.CommonServices;
    using Services.CommonServices;
    using Services.Client;
    using Interfaces.Client;
    using Silvercrest.Web.Helpers.Maintance;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //SLVR_DEVEntities applicationContext = new SLVR_DEVEntities();
            //kernel.Bind(typeof(SLVR_DEVEntities)).ToConstant(applicationContext);
            kernel.Bind<SLVR_DEVEntities>().ToSelf().InRequestScope();

            //IApplicationContext applicationContext = new Silvercrest.DataAccess.Model.SLVR_DEVEntities();
            //kernel.Bind(typeof(IApplicationContext)).ToConstant(applicationContext);
            //kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>()
            //    .WithConstructorArgument("context", context => kernel.Get<IApplicationContext>());

            //kernel.Bind<UserManager<AspNetUser>>().ToSelf();

            //kernel.Bind<IRoleStore<IdentityRole, string>>().To<RoleStore<IdentityRole>>()
            //  .WithConstructorArgument("context", context => kernel.Get<>());

            //kernel.Bind<RoleManager<IdentityRole>>().ToSelf()
            //    .WithConstructorArgument("store", store => kernel.Get<IRoleStore<IdentityRole, string>>());

            //#System  

            //#Administrator  
            kernel.Bind<IContactsService>().To<ContactsService>().InRequestScope();
            kernel.Bind<IAdministratorService>().To<AdministratorService>().InRequestScope();

            //#Manager
            kernel.Bind<IManagerService>().To<ManagerService>().InRequestScope();

            //#Client
            kernel.Bind<IHoldingsMainService>().To<HoldingsMainService>().InRequestScope();
            kernel.Bind<IClientService>().To<ClientService>().InRequestScope();
            kernel.Bind<ITransactions>().To<TransactionsService>().InRequestScope();
            kernel.Bind<ITransactionsAdoService>().To<TransactionsAdoService>().InRequestScope();
            kernel.Bind<IManagerAdo>().To<ManagerAdoService>().InRequestScope();
            kernel.Bind<IGroupService>().To<GroupService>().InRequestScope();
            kernel.Bind<IRedirectHelperService>().To<RedirectHelperService>().InRequestScope();
            kernel.Bind<ITeam>().To<TeamService>().InRequestScope();
            kernel.Bind<IDocuments>().To<DocumentsService>().InRequestScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();

            //#Analytics       
            kernel.Bind<IUserAnalyticsService>().To<UserAnalyticsService>().InRequestScope();
            kernel.Bind<IAnalyticsService>().To<AnalyticsService>().InRequestScope();
            kernel.Bind<IClientAdoService>().To<ClientAdoService>().InRequestScope();
            kernel.Bind<IHoldingsAdoService>().To<HoldingsAdoService>().InRequestScope();

            kernel.BindFilter<AnalyticsFilter>(System.Web.Mvc.FilterScope.Controller, 0).WhenControllerHas<AnalyticsAttribute>().InRequestScope();
            kernel.BindFilter<MaintanceFilter>(System.Web.Mvc.FilterScope.Controller, 0).WhenControllerHas<MaintanceAttribute>();
            //kernel.BindFilter<MaintanceFilter>(System.Web.Mvc.FilterScope.Controller, 0).WhenControllerHas<MaintanceAttribute>().WithConstructorArgumentFromActionAttribute<MaintanceFilter>("area", o => o._area); ;

        }
    }
}
