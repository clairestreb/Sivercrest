using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Info
    {
        public DateTime? Date { get; set; }
        public string CustodianAccount { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }
        public double TotalMarketValue { get; set; }
        public double MarketValue { get; set; }
        public double? UnrealizedGL { get; set; }
    }
}
