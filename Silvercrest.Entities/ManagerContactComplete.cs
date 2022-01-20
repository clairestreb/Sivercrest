using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ManagerContactComplete
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public bool? IsWebUser { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
        public int? FamilyId { get; set; }
        public int? FirmUserGroupId { get; set; }
        public int? ContactId { get; set; }
        public string FamilyIdQuery { get; set; }
        public string FirmUserGroupIdQuery { get; set; }
        public string ContactIdQuery { get; set; }
    }
}
