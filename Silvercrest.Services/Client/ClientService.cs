
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services
{
    public class ClientService : IClientService
    {
        private AccountRepository _accountRepository;

        public ClientService(SLVR_DEVEntities context)
        {
            _accountRepository = new AccountRepository(context);
        }

        public List<ClientAccount> GetAccounts(int? contactId)
        {
            var list = _accountRepository.GetAccountAndGroupsList(contactId).ToList();
            list.RemoveAll(x => x.Name == "Total");
            return list;
        }

        public List<ClientAccount> GetGroups(int? contactId)
        {
            var list = _accountRepository.GetAccountAndGroupsList(contactId).Where(x => x.IsClientGroup == true || x.IsGroup == true).ToList();
            list.RemoveAll(x => x.Name == "Total");
            return list;
        }

        public List<PieChart> GetCharts(int? contactId)
        {
            return _accountRepository.GetChartAssetClass(contactId, null,null,null).Where(x=>x.Percent > 0).ToList();
        }

        public void FillInfo(IndexInitViewModel model, UserInfo info)
        {
            var viewInfo = _accountRepository.GetInfo(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
            if (viewInfo == null)
            {
                model.Date = DateConverter.ConvertDateToString(DateTime.Now);
                model.CustodianAccount = "";
                model.Name = "Name";
            }
            else
            {
                model.Date = DateConverter.ConvertDateToString(viewInfo.Date);
                model.CustodianAccount = viewInfo.CustodianAccount == null ? "" : "custodian account: " + viewInfo.CustodianAccount;
                model.Name = viewInfo.Name ?? "Name";
            }
        }

        public int? GetContactId(string name)
        {
            return _accountRepository.GetContactId(name);
        }

        public List<AccountNickname> GetNicknames(int? contactId)
        {
            return _accountRepository.GetNicknames(contactId);
        }

        public void AddNickname(int contactId, int accountId, string mainId, string nickname)
        {
            _accountRepository.AddNickname(contactId, accountId, mainId, nickname);
        }

        public void DeleleNickname(int contactId, int accountId)
        {
            _accountRepository.DeleteNickname(contactId, accountId);
        }

        public void AcceptTerms(string value)
        {
            _accountRepository.AcceptTerms(int.Parse(value));
        }
    }
}
