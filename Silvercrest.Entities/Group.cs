using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Group
    {
//        public int Id { get; set; }
        public int ContactId { get; set; }
        public int AccountGroupId { get; set; }
        public string GroupName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public bool? IsGroupMember { get; set; }
    }
}
