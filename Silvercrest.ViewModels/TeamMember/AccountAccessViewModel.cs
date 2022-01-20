using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.TeamMember
{
    public class AccountAccessViewModel
    {
        public string AccountName { get; set; }
        public string ShortName { get; set; }
        public string ManagerCode { get; set; }
        public bool AccessType { get; set; }
        public int AccountId { get; set; }
    }
}
