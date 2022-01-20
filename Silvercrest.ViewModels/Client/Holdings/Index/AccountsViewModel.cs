using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Silvercrest.Entities;

namespace Silvercrest.ViewModels.Client.Holdings
{
    public class AccountViewModel
    {
        public string Account { get; set; }                
        public double MarketValue { get; set; }
        public double PercentOfTotal { get; set; }
        public int? ClientId { get; set; }
        public string AccountType { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsClientGroup { get; set; }
        public int? EntityId { get; set; }
    }
}
