using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ClientAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ContactId { get; set; }
        public int? EntityId { get; set; }
        public double? TotalValue { get; set; }
        public double? PercentOfTotal { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsClientGroup { get; set; }
        public DateTime Date { get; set; }
        public bool IsDefault { get; set; }
        public int? SortOrder { get; set; }
        public string AccountType { get; set; }

        public string CompositeGroupId
        {
            get
            {
                return EntityId + "_" + IsClientGroup;
            }
        }
    }
}
