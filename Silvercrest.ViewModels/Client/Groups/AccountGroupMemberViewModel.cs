using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Groups
{
    public class AccountGroupMemberViewModel
    {
        public string AccountName { get; set; }
        public bool? IsGroupMember { get; set; }
        public int AccountId { get; set; }
        public int AccountGroupId { get; set; }
    }
}
