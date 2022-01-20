using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ClientGroupAccount
    {
        public DateTime Date { get; set; }
        public int? ContactId { get; set; }
        public int? GroupEntityId { get; set; }
        public bool? GroupIsGroup { get; set; }
        public bool? GroupIsClientGroup { get; set; }
        public string GroupName { get; set; }
        public double? GroupTotalValue { get; set; }
        public int? AccountEntityId { get; set; }
        public bool? AccountIsGroup { get; set; }
        public bool? AccountIsClientGroup { get; set; }
        public string AccountName { get; set; }
        public double? AccountTotalValue { get; set; }
        public double? PercentOfGroup { get; set; }
        public int? sortOrder { get; set; }
    }
}
