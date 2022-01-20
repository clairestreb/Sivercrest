using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.Client.Transactions
{
    public class TransactionsPurchasesViewModel:GenericViewModel
    {
        public TopSectionViewModel PageData { get; set; }
        public JsonResult AccountsData { get; set; }
        public JsonResult GroupsData { get; set; }
        public JsonResult AccountsWithinGroupsData { get; set; }
        public JsonResult TableData { get; set; }
        public string Error { get; set; }
    }
}
