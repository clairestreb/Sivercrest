using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Sale
    {
        public string Security { get; set; }
        public double? Quantity { get; set; }
        public string OpenDate { get; set; }
        public double? OriginalTotal { get; set; }
        public double? AdjustedUnit { get; set; }
        public double? AdjustedTotal { get; set; }
        public string CloseDate { get; set; }
        public double? ProceedsUnit { get; set; }
        public double? ProceedsTotal { get; set; }
        public double? ShortTerm { get; set; }
        public double? LongTerm { get; set; }
        public string GroupingOne { get; set; }
        public string GroupingSecond { get; set; }
        public int SortingOne { get; set; }
        public int SortingSecond { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
