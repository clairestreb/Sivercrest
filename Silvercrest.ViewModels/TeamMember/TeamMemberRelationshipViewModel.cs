using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silvercrest.ViewModels.TeamMember
{
    public class TeamMemberRelationshipViewModel:GenericViewModel
    {
        public int? FamilyId { get; set; }
        public int? FirmUserGroupId { get; set; }
        public JsonResult TableData { get; set; }
        public List<ManagerContactComplete> ContactsByFamily { get; set; }

    }
}
