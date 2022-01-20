using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;
namespace Silvercrest.ViewModels.Client.Holdings
{
    public class HoldingsInitViewModel:GenericViewModel
    {
        public List<Account> Accounts { get; set; }
    }
}
