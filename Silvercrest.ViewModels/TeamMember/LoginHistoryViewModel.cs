using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.TeamMember
{
    public class LoginHistoryViewModel:GenericViewModel
    {
        public string ContactFullName { get; set; }
        public string WebUserId { get; set; }
        public int ContactId { get; set; }
        public int FamilyId { get; set; }
    }
}
