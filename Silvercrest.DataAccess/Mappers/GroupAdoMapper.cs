using Silvercrest.ViewModels.Client.Groups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;

namespace Silvercrest.DataAccess.Mappers
{
    public class GroupAdoMapper
    {
        public static void MapGroups(DataRowCollection rows, List<Silvercrest.Entities.Group> accounts)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var mappedGroup = new Silvercrest.Entities.Group();
                    mappedGroup.ContactId = (int)r["contact_id"];
                    mappedGroup.AccountGroupId = (int)r["account_group_id"];
                    mappedGroup.GroupName = r["grp_name"].ToString();
                    mappedGroup.AccountId = (int)r["account_id"];
                    mappedGroup.AccountName = r["account_name"].ToString();
                    mappedGroup.IsGroupMember = (bool?)r["is_group_member"];
                    accounts.Add(mappedGroup);
                }
            }
        }
        public static void MapAccountGroups(DataRowCollection rows, Dictionary<int, string> clientAccounts)
        {
            foreach (DataRow account in rows)
            {
                clientAccounts.Add((int)account["account_id"], account["account_name"].ToString());
            }
        }
    }
}
