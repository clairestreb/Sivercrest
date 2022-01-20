using Silvercrest.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.TeamMember
{
    public class TeamMemberFamiliesViewModel:GenericViewModel
    {
        public List<List<ManagerComplete>> Managers { get; set; }
//        public Hashtable HashData { get; set; }
    }
}
