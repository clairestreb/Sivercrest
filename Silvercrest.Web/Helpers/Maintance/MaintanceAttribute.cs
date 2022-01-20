using Silvercrest.Interfaces;
using System.Web.Mvc;
using System.Web.Routing;

namespace Silvercrest.Web.Helpers.Maintance
{
    public class MaintanceAttribute : FilterAttribute { }
    public class MaintanceFilter : ActionFilterAttribute
    {
        private readonly IRedirectHelperService _redirectHelperService;
        public MaintanceFilter(IRedirectHelperService redirectHelperService)
        {
            _redirectHelperService = redirectHelperService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isRedirected = _redirectHelperService.IsRedirect();
            if (isRedirected)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary {{ "Controller", "Maintance" },
                                      { "Action", "Index" } ,{"Area", "" } });
            
            }
     
            base.OnActionExecuting(filterContext);
        }
    }
}