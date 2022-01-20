using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface IAccountService
    {
        List<Entities.Account> GetAccounts(int? contactId);
        List<Entities.Account> GetGroups(int? contactId);
        List<Entities.Account> GetAccountsWithinGroup(int? contactId, int? entity_id, bool? is_group, bool? is_client_group);
    }
}
