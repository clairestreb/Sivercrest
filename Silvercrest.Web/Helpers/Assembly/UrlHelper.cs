using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Helpers.Assembly
{
    public static class UrlHelperExtensions
    {
        public static string _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ".000001";
        public static string GetVersion(this UrlHelper helper)
        {
            return _version;
        }
    }
}