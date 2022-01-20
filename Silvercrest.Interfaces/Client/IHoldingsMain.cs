using Silvercrest.Entities;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface IHoldingsMainService
    {
        void FillInAccounts(List<AccountViewModel> accounts, int? contactId);
        void FillInfoView(TopSectionViewModel topSectionViewModel, UserInfo info, int? type = null);
        //void FillInBalances(List<BalanceViewModel> balances, UserInfo info);
        //List<PieChart> GetChartStrategy(UserInfo info);
        List<PieChart> GetChartAssetClass(UserInfo info);
        void FillInBalancesShort(List<BalanceViewModel> balances, UserInfo info);
        int? GetContactId(string name);
        void FillInCashes(List<CashViewModel> cash, UserInfo info);
        void FillInOtherAssets(List<CashViewModel> assets, UserInfo info);
        void FillInEquities(List<IncomeViewModel> equities, UserInfo info);
        void FillInIncomes(List<IncomeViewModel> income, UserInfo info);
       /* List<PieChart> GetChartType(UserInfo info);
        List<PieChart> GetChartMunicBound(UserInfo info);
        List<PieChart> GetEquitiesChartType(UserInfo info);
        List<PieChart> GetEquitiesChartMunicBound(UserInfo info);*/
        UserInfo GetUserInfo(int? contactId);
        bool GetHoldingsType(int? contactId);
        void FillMultiData(UserInfo info);
    }
}
