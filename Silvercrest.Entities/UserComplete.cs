using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class UserComplete
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? ContactId { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string ContactCode { get; set; }
    }
}
