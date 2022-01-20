using Silvercrest.Entities;
using Silvercrest.ViewModels.Client;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silvercrest.Interfaces
{
    public interface IClientAdoService
    {
        //Task<IndexInitViewModel> FillInfo(UserInfo info);
        //Task<List<ClientAccount>> GetAccounts(int? contactId);
        //List<PieChart> GetCharts(int? contactId);
        void FillHomeView(IndexInitViewModel model, List<PieChart> charts, UserInfo info, List<ClientGroupAccount> mappedAccountList);
        void ContactEntities(List<ClientAccount> entities, int contactId);
    }
}
