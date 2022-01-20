using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services
{
    public class ManagerAdoService : IManagerAdo
    {
        private ManagerAdoRepository _repository;

        public ManagerAdoService()
        {
            _repository = new ManagerAdoRepository();
        }

        public void GetAccountsByFamily(TeamMemberRelationshipViewModel model, int? familyId, int? firmUserGroupId, List<Account> accounts)
        {
            DataSet data = _repository.ManagerInit(familyId, firmUserGroupId);

            if (data.Tables[1].Rows.Count > 0)
            {
                var mappedContactList = new List<Silvercrest.Entities.ManagerContactComplete>();
                foreach (DataRow row in data.Tables[1].Rows)
                {
                    var mappedContact = new Silvercrest.Entities.ManagerContactComplete();
                    mappedContact.ContactId = (int?)row["contact_id"];
                    mappedContact.DisplayName = row["display_name"].ToString();
                    mappedContact.Email = row["email_address"].ToString();
                    mappedContact.IsWebUser = (bool?)row["is_web_user"];
                    mappedContact.Relationship = row["relationship"].ToString();
                    mappedContactList.Add(mappedContact);
                }
                model.ContactsByFamily = mappedContactList;
            }
            if (data.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var mappedAccount = new Silvercrest.Entities.Account();
                    mappedAccount.Name = row["display_name"].ToString();
                    mappedAccount.MarketValue = (double?)row["market_value"] ?? 0;
                    mappedAccount.PercentOfTotal = (double?)row["pct"] ?? 0;
                    accounts.Add(mappedAccount);
                }
//                accounts.RemoveAll(x => x.Name == "Total" || x.Name == "All Accounts");
            }
        }

        public void GetContactList(int? contactId, List<ManagerContactComplete> contacts)
        {
            DataSet data = _repository.ContactListInit(contactId);
            if (data.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var mappedAccount = new Silvercrest.Entities.ManagerContactComplete();
                    mappedAccount.DisplayName = row["display_name"].ToString();
                    mappedAccount.FamilyId =  (int?)row["family_group_id"];
                    mappedAccount.FirmUserGroupId = (int?)row["firm_user_group_id"];
                    mappedAccount.Email = row["email_address"].ToString();
                    mappedAccount.ContactId = (int?)row["contact_id"];
                    mappedAccount.IsWebUser = (int)row["is_web_user"] == 1;
                    contacts.Add(mappedAccount);
                }
                //                accounts.RemoveAll(x => x.Name == "Total" || x.Name == "All Accounts");
            }

        }
    }
}
