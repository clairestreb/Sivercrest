using Microsoft.AspNet.Identity;
using Silvercrest.Interfaces.CommonServices;
using Silvercrest.Services.CommonServices;
using Silvercrest.ViewModels.Common.Analytics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Helpers.Analytics
{
    public class AnalyticsAttribute : FilterAttribute { }

    public class AnalyticsFilter : ActionFilterAttribute
    {
        private readonly IAnalyticsService _service;
        public AnalyticsFilter(IAnalyticsService service)
        {
            _service = service;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        
            bool hasIgnoreAnalyticsAttribute = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreAnalyticsAttribute), false).Any() ||
                filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IgnoreAnalyticsAttribute), false).Any();
            if (!hasIgnoreAnalyticsAttribute &&
                filterContext.Exception == null &&
                filterContext.Result is ViewResult)
            {
                var userId = filterContext.HttpContext.User.Identity.GetUserId();
                var rawUrl = filterContext.HttpContext.Request.RawUrl;
                var viewAccountRoutePath = @"/Client/Holdings/ViewAccount";
                if (!string.IsNullOrWhiteSpace(userId) && !rawUrl.Contains(viewAccountRoutePath))
                {
                    _service.RecordNonAccountPageVisit(userId, rawUrl);
                }
                if (!string.IsNullOrWhiteSpace(userId) && rawUrl.ToLower().Contains(viewAccountRoutePath.ToLower()))
                {
                    var model = new ViewAccountRouteParametersModel();
                    model.IsGroup = ParametersParser.GetBoolValue(filterContext.HttpContext.Request.QueryString["isGroup"]);
                    model.IsClientGroup = ParametersParser.GetBoolValue(filterContext.HttpContext.Request.QueryString["isClientGroup"]);
                    model.ContactId = ParametersParser.GetIntValue(filterContext.HttpContext.Request.QueryString["contactId"]);
                    model.EntityId = ParametersParser.GetIntValue(filterContext.HttpContext.Request.QueryString["entityId"]);
                    if (model.IsGroup.HasValue && model.IsClientGroup.HasValue && model.ContactId.HasValue && model.EntityId.HasValue)
                    {
                        _service.RecordAccountPageVisit(userId, rawUrl, model);
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }
        
    }
}