using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ManagerComplete
    {
        public int Id { get; set; }
        public int RelationshipId { get; set; }
        public string Manager { get; set; }
        public string Relationship { get; set; }
        public double? RelationshipValue { get; set; }
        public int? FirmUserGroupId { get; set; }
    }
}
