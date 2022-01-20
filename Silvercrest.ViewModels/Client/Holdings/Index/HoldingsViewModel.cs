using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.Client.Holdings.Index
{
    public class HoldingsViewModel:GenericViewModel
    {
        public TopSectionViewModel PageData { get; set; }
        public JsonResult ChartData { get; set; }
        public JsonResult ChartStrategyData { get; set; }
        public JsonResult TableData { get; set; }
        public JsonResult FullBalancesData { get; set; }
        public JsonResult AccountsData { get; set; }
        public JsonResult GroupsData { get; set; }
        public JsonResult AccountsWithinGroupsData { get; set; }

        public HoldingsViewModel()
        {
            PageData = new TopSectionViewModel();
        }
    }
}
