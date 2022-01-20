using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Silvercrest.ViewModels.Client.Holdings.Index
{
    public class TopSectionViewModel
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string CustodianAccount { get; set; }
        public string Name { get; set; }
        public int? ClientId { get; set; }
        public bool? IsGroup { get; set; }
        public double TotalMarketValue { get; set; }
        public double MarketValue { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public bool HoldingType { get; set; }
        public int? EntityId { get; set; }
        public bool? IsClientGroup { get; set; }
        public double UnrealizedGL { get; set; }
    }
}
