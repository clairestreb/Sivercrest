using Silvercrest.ViewModels.Client.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface IGroupService
    {
        GroupsViewModel GetGroupsList(int? contactId);
        void UpdateGroup(int? contactId, string groupName, string accountIds, string changerName, int? accountGroupId);
        void DeleteGroup(int? contactId, int? accountGroupId);
    }
}
