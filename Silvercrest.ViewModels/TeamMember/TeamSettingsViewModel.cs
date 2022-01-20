using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.TeamMember
{
    public class TeamSettingsViewModel: GenericViewModel
    {
        public int? contactId { get; set; }
        //public int? FamilyId { get; set; }
        public int FirmUserGroupId { get; set; }
        public string Code { get; set; }
        public string ManagerName { get; set; }
        public bool? StatementUploadOnHold { get; set; }
        public bool? EmailNotification { get; set; }
        public bool? EquityWriteUps { get; set; }
        public bool? EconomicCommentary { get; set; }
    }
}
