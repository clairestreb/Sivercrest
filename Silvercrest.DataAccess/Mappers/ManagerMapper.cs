using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class ManagerMapper
    {
        public static List<Silvercrest.Entities.ManagerComplete> MapManagersList(IList<p_Web_Relationships_Result> list)
        {
            var mappedManagerList = new List<Silvercrest.Entities.ManagerComplete>();
            foreach (var manager in list)
            {
                var mappedManager = new Silvercrest.Entities.ManagerComplete();
                mappedManager.Manager = manager.manager;
                mappedManager.Relationship = manager.relationship;
                mappedManager.RelationshipValue = manager.relationship_value ?? 0;
                mappedManager.RelationshipId = manager.id;
                mappedManager.FirmUserGroupId = manager.firm_user_group_id;
                mappedManagerList.Add(mappedManager);
            }
            return mappedManagerList;
        }

        public static List<Silvercrest.Entities.AccountAccess> MapAccountAccessList(IList<p_Web_Current_Account_Access_Result> accountList)
        {
            var mappedAccountAccessList = new List<Silvercrest.Entities.AccountAccess>();

            foreach (var account in accountList)
            {
                var mappedAccount = new Silvercrest.Entities.AccountAccess();
                mappedAccount.AccountName = account.account_name;
                mappedAccount.ShortName = account.account_code;
                mappedAccount.ManagerCode = account.manager_code;
                mappedAccount.AccessType = account.access_type;
                mappedAccount.AccountId = account.account_id;
                mappedAccountAccessList.Add(mappedAccount);
            }
            return mappedAccountAccessList;

        }

        public static List<Silvercrest.Entities.AccountAccess> MapNonAccountAccessList(IList<p_Web_Non_Access_Accounts_Result> accountList)
        {
            var mappedAccountList = new List<Silvercrest.Entities.AccountAccess>();
            var accessTypeUnckecked = 0;
            foreach (var account in accountList)
            {
                var mappedAccount = new Silvercrest.Entities.AccountAccess();
                mappedAccount.AccountName = account.account_name;
                mappedAccount.ShortName = account.account_code;
                mappedAccount.ManagerCode = account.manager_code;
                mappedAccount.AccessType = accessTypeUnckecked;
                mappedAccount.AccountId = account.account_id;
                mappedAccountList.Add(mappedAccount);
            }
            return mappedAccountList;
        }

        public static List<Silvercrest.Entities.TeamSettings> MapFirmUserGroupAttributesList(IList<f_Web_Firm_User_Group_Attributes_Result> list)
        {
            var mappedTeamSettingsList = new List<Silvercrest.Entities.TeamSettings>();

            foreach (var item in list)
            {
                var mappedAttribute = new Silvercrest.Entities.TeamSettings();
                mappedAttribute.Code = item.code;
                mappedAttribute.ManagerName = item.manager_name;
                mappedAttribute.StatementUploadOnHold = item.statements_upload_held.Value;
                mappedTeamSettingsList.Add(mappedAttribute);
            }
            return mappedTeamSettingsList;
        }
    }
}
