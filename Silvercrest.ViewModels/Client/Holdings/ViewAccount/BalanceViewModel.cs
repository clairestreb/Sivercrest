using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;
namespace Silvercrest.ViewModels.Client.Holdings
{    
    public class BalanceViewModel:GenericViewModel
    {
        public string  Operation { get; set; }
        public double? CashMoneyFunds { get; set; }
        public double? FixedIncome { get; set; }
        public double? Equities { get; set; }
        public double? OtherAssets { get; set; }
        public double? MarketValue { get; set; }
        public double PercentOfTotal { get; set; }
        public double PercentOfAssetClass { get; set; }
        public double? AnnualIncome { get; set; }
        public string GroupName { get; set; }
        public string AssetClass { get; set; }
        public string Strategy { get; set; }
        public string Url { get; set; }
        public double? CurrentYield { get; set; }
    }
   
}
