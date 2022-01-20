using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Purchase
    {
        public string SecurityName { get; set; }
        public string Symbol { get; set; }
        public string TradeDate { get; set; }
        public double? Quantity { get; set; }
        public double? UsdAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string GroupingOne { get; set; }
        public string GroupingSecond { get; set; }
        public int SortingOne { get; set; }
        public int SortingSecond { get; set; }
    }
}
