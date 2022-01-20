using Silvercrest.Entities;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces.Client
{
    public interface IHoldingsAdoService
    {
        void FillHoldingsView(UserInfo info, HoldingsViewModel view, List<PieChart> charts, List<BalanceViewModel> shortBalances, List<BalanceViewModel> fullBalances, List<PieChart> chartsStrategy, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups);
        void FillCashView(UserInfo info, HoldingsCashViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<CashViewModel> cashList);
        void FillIncomeView(UserInfo info, HoldingsIncomeViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<PieChart> chartsType, List<PieChart> chartsMunic, List<IncomeViewModel> income);
        void FillEquitiesView(UserInfo info, HoldingsIncomeViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<PieChart> chartsType, List<PieChart> chartsMunic, List<IncomeViewModel> income);
        void FillOtherAssetsView(UserInfo info, HoldingsCashViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<CashViewModel> assetsList);
        void FillModalView(UserInfo info, int? securityId, List<IncomeViewModel> income);
    }
}
