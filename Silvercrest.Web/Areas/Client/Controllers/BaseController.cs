using Silvercrest.Web.Helpers.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Analytics]
    public class BaseController : Controller
    {
        public int? ContactId { get; set; }
    }
}