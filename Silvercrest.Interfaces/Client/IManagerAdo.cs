using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;
using Silvercrest.ViewModels.TeamMember;

namespace Silvercrest.Interfaces
{
    public interface IManagerAdo
    {
        void GetContactList(int? contactId, List<ManagerContactComplete> contacts);
        void GetAccountsByFamily(TeamMemberRelationshipViewModel model, int? familyId, int? firmUserGroupId, List<Account> accounts);
    }
}
