using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.TeamMember
{
    public class ClientAnalyticsViewModel:GenericViewModel
    {
        public JsonResult TableData { get; set; }
    }
}
