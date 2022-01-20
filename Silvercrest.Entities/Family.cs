using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Contact> Users { get; set; }
    }
}
