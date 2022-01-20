using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class GroupRepository
    {
        private SLVR_DEVEntities _context;

        public GroupRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public List<Group> GetClientGroupsList(int? contactId)
        {
            IList<f_Web_Client_Groups_Result> groupList = _context.f_Web_Client_Groups(contactId).ToList();
            var mappedList = GroupMapper.MapGroupsList(groupList);
            return mappedList;
        }

        public void UpdateClientGroup(int? contactId, string groupName, string accountIds, string changerName, int? accountGroupId)
        {
            _context.p_Web_Update_Client_Group(contactId, groupName, accountIds, changerName, accountGroupId);
        }

        public void DeleteClientGroup(int? contactId, int? accountGroupId)
        {
            _context.p_Web_Delete_Client_Group(contactId, accountGroupId);
        }

        public void GetClientAccountsList(int? contactId, Dictionary<int, string> clientAccounts)
        {
            IList<p_Web_Client_Group_Accounts_Result> accountList = _context.p_Web_Client_Group_Accounts(contactId).ToList();
            foreach (var account in accountList)
            {
                clientAccounts.Add(account.account_id.Value, account.account_name);
            }
        }

        public List<ClientTeam> GetClientTeam(int? contactId)
        {
            List< p_Web_Client_Team_Result> clientTeamsList = _context.p_Web_Client_Team(contactId).ToList();
            var mappedTeamsList = GroupMapper.MapTeamList(clientTeamsList);
            return mappedTeamsList;
        }
    }
}
