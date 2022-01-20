using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class AccountNickname
    {
        public int? ContactId { get; set; }
        public int? AccountId { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
