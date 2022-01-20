using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;


namespace Silvercrest.DataAccess.Mappers
{
    public class GroupMapper
    {
        public static List<Silvercrest.Entities.Group> MapGroupsList(IList<f_Web_Client_Groups_Result> list)
        {
            var mappedGroupsList = new List<Silvercrest.Entities.Group>();
            foreach (var group in list)
            {
                var mappedGroup = new Silvercrest.Entities.Group();
                mappedGroup.ContactId = group.contact_id;
                mappedGroup.AccountGroupId = group.account_group_id;
                mappedGroup.GroupName = group.grp_name;
                mappedGroup.AccountId = group.account_id;
                mappedGroup.AccountName = group.account_name;
                mappedGroup.IsGroupMember = group.is_group_member;
                mappedGroupsList.Add(mappedGroup);
            }
            return mappedGroupsList;
        }

        public static List<ClientTeam> MapTeamList(IList<p_Web_Client_Team_Result> list)
        {
            list = list.OrderBy(x => x.sort_order).ToList();
            var mappedTeamList = new List<ClientTeam>();
            foreach(var team in list)
            {
                var clientTeam = new ClientTeam();
                clientTeam.Title = team.title;
                clientTeam.Name = team.display_name;
                clientTeam.Email = team.email_address_1;
                clientTeam.PhoneNumber = team.phone_number;
                clientTeam.Photo = team.photo;
                mappedTeamList.Add(clientTeam);
            }
            return mappedTeamList;
        }
    }
}
