using System;
namespace Silvercrest.Entities
{
    public  class Income
    {
        public string Holdings { get; set; }
        public string Symbol { get; set; }
        public double Quantity { get; set; }
        public DateTime? AdjustedCostDate { get; set; }
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
    }
}