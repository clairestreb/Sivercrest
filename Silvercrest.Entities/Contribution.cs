using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Contribution
    {
        public string SecurityName { get; set; }
        public string TradeDate { get; set; }
        public double UsdAmount { get; set; }
        public string Comment { get; set; }
        public string TransactionType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
