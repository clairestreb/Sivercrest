﻿using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.Client.Holdings
{
    public class HoldingsIncomeViewModel:GenericViewModel
    {
        public TopSectionViewModel PageData { get; set; }
        public JsonResult AccountsData { get; set; }
        public JsonResult GroupsData { get; set; }
        public JsonResult AccountsWithinGroupsData { get; set; }
        public JsonResult ChartTypeData { get; set; }
        public JsonResult ChartMunicData { get; set; }
        public JsonResult IncomeData { get; set; }
        //public JsonResult FullBalancesData { get; set; }


        public HoldingsIncomeViewModel()
        {
            PageData = new TopSectionViewModel();
        }
    }
}