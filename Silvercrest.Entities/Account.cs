using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silvercrest.Entities
{
    public class Account
    {
        public bool? IsClientGroup { get; set; }
        public bool? IsGroup { get; set; }
        public int? ContactId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "Money")]
        public double MarketValue { get; set; }
        public double PercentOfTotal { get; set; }
        public int? EntityId { get; set; }

        public string EntityIdEnc { get; set; }
        public string IsGroupEnc { get; set; }
        public string IsClientGroupEnc { get; set; }
        public string ContactIdEnc { get; set; }
    }
}
