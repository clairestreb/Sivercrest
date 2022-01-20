using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities.Enums;
using Silvercrest.Interfaces;
using System.Web;

namespace Silvercrest.Services.Client
{
    public class RedirectHelperService : IRedirectHelperService
    {
        private WebApplicationParametersRepository _webApplicationParametersRepository;
        public RedirectHelperService(SLVR_DEVEntities context)
        {
            _webApplicationParametersRepository = new WebApplicationParametersRepository(context);
        }
        public bool IsRedirect()
        {
            var currentUserRole = HttpContext.Current.Request.Cookies.Get("UserRole")?.Value;
            var webApplicationParameters = _webApplicationParametersRepository.GetApplicationParameters();
            if (currentUserRole == UserRole.Administrator.ToString() || currentUserRole == UserRole.SuperUser.ToString())
            {
                return false;
            }
            return webApplicationParameters.system_maintenance;
        }
    }
}
