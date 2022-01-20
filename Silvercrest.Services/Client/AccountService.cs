using Silvercrest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.DataAccess.Model;

namespace Silvercrest.Services
{
    public class AccountService : IAccountService
    {
        private AccountRepository _accountRepository;

        public AccountService(SLVR_DEVEntities context)
        {
            _accountRepository = new AccountRepository(context);
        }

        public List<Entities.Account> GetAccounts(int? contactId)
        {
            return _accountRepository.GetAccountList(contactId).Where(x => x.IsGroup == false && x.Name != "Total").ToList();
        }

        public List<Entities.Account> GetGroups(int? contactId)
        {
            return _accountRepository.GetAccountList(contactId).Where(x=>x.IsGroup == true).ToList();
        }

        public List<Entities.Account> GetAccountsWithinGroup(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            return _accountRepository.GetAccountsWithinGroup(contactId, entity_id, is_group, is_client_group).ToList();
        }
    }
}
