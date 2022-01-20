using Silvercrest.Web.Helpers.Maintance;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
