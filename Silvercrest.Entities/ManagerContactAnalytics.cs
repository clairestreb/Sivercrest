using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ManagerContactAnalytics
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public double? LoginFrequency { get; set; }
        public int? LoggedInTime { get; set; }
        public int? ContactId { get; set; }
        public int? FamilyId { get; set; }
        public int? FirmUserGroupId { get; set; }
        public string ContactIdQuery { get; set; }
        public string FamilyIdQuery { get; set; }
        public string FirmUserGroupIdQuery { get; set; }
        public string IsFromQuery { get; set; }
    }
}
