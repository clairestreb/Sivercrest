using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.Client
{
    public class IndexInitViewModel:GenericViewModel
    {
        public int ContactId { get; set; }
        public string Date { get; set; }
        public string CustodianAccount { get; set; }
        public string Name { get; set; }
        public JsonResult ChartData { get; set; }
        public JsonResult TableData { get; set; }

        public IndexInitViewModel()
        {
            ChartData = new JsonResult();
        }
    }
}
