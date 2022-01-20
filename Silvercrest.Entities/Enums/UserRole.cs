using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities.Enums
{
    public enum UserRole
    {
        None = 0,
        SuperUser = 1,
        Administrator = 2,
        TeamMember = 3,
        SecondaryTeamMember = 4,
        Client = 5,
        SecondaryClient = 6
    }
}
