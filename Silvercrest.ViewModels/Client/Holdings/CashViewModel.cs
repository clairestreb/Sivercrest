using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Holdings
{
    public class CashViewModel:GenericViewModel
    {
        public string Holding { get; set; }
        public double? Quantity { get; set; }
        public double? Total { get; set; }
        public double? MarketValueUnits { get; set; }
        public double? MarketValueTotal { get; set; }
        public double? PercentOfAssets { get; set; }
        public double? AnnualIncome { get; set; }
        public double? CurrentYield { get; set; }
        public string Category { get; set; }
    }
}
