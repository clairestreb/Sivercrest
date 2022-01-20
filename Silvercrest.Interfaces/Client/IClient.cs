using Silvercrest.Entities;
using Silvercrest.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface IClientService
    {
        List<ClientAccount> GetAccounts(int? contactId);
        List<ClientAccount> GetGroups(int? contactId);
        List<PieChart> GetCharts(int? contactId);
        void FillInfo(IndexInitViewModel model, UserInfo info);
        int? GetContactId(string name);
        List<AccountNickname> GetNicknames(int? contactId);
        void DeleleNickname(int contactId, int accountId);
        void AddNickname(int contactId, int accountId, string mainId, string nickname);
        void AcceptTerms(string value);
    }
}
