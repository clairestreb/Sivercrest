using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Access
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string ShortName { get; set; }
        public string Manager { get; set; }
        public bool IsHaveAccess { get; set; }
    }
}
