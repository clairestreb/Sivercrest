using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Holdings
{
    public class IncomeViewModel:GenericViewModel
    {
        public string Holdings { get; set; }
        public string Symbol { get; set; }
        public double Quantity { get; set; }
        public string AdjustedCostDate { get; set; }
        public double AdjustedCostUnit { get; set; }
        public double AdjustedCostTotal { get; set; }        
        public double MarketValueUnits { get; set; }
        public double MarketValueTotal { get; set; }
        public double MarketValuePercentOfAssets { get; set; }
        public double AccuredInterest { get; set; }
        public double AnnualIncome { get; set; }
        public double CurrentYield { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int CategorySort { get; set; }
        public int SubCategorySort { get; set; }
        public int SecurityId { get; set; }
        public string SecurityName { get; set; }
        public int NumberLots { get; set; }
    }
}
