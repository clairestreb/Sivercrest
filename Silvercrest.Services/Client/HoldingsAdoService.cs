using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces.Client;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services.Client
{
    public class HoldingsAdoService: IHoldingsAdoService
    {
        private HoldingsRepository _repository;

        public HoldingsAdoService()
        {
            _repository = new HoldingsRepository();
        }

        public void FillHoldingsView(UserInfo info, HoldingsViewModel view, List<PieChart> charts, List<BalanceViewModel> shortBalances, List<BalanceViewModel> fullBalances, List<PieChart> chartsStrategy, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups)
        {
            FillPageInfo(info, view.PageData);
            DataSet data = _repository.FillHoldingsView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);

            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            HoldingsAdoMapper.MapHoldingType(data.Tables[2].Rows, view);
            HoldingsAdoMapper.MapPageData(data.Tables[3].Rows, view.PageData);
            HoldingsAdoMapper.MapCharts(data.Tables[3].Rows, charts);
            HoldingsAdoMapper.MapShortBalances(data.Tables[3].Rows, shortBalances);
            HoldingsAdoMapper.MapStrategyChart(data.Tables[4].Rows, chartsStrategy);
            HoldingsAdoMapper.MapFullBalances(data.Tables[5].Rows, fullBalances);
        }

        public void FillCashView(UserInfo info, HoldingsCashViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<CashViewModel> cashList)
        {
            FillPageInfo(info, view.PageData);
            //            DataSet data = _repository.FillCashView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);

            DataSet data = _repository.FillAssetClassView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "Cash & Money Funds");

            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            HoldingsAdoMapper.MapPageData(data.Tables[2].Rows, view.PageData);
            HoldingsAdoMapper.MapCashes(data.Tables[4].Rows, cashList);
        }

        public void FillIncomeView(UserInfo info, HoldingsIncomeViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<PieChart> chartsType, List<PieChart> chartsMunic, List<IncomeViewModel> income)
        {
            FillPageInfo(info, view.PageData);
            DataSet data = _repository.FillAssetClassView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "Fixed Income");

            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            HoldingsAdoMapper.MapPageData(data.Tables[2].Rows, view.PageData);
            HoldingsAdoMapper.MapTypeAndMunicCharts(data.Tables[3].Rows, chartsType, chartsMunic);
            HoldingsAdoMapper.MapIncomes(data.Tables[4].Rows, income);
        }
        public void FillEquitiesView(UserInfo info, HoldingsIncomeViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<PieChart> chartsType, List<PieChart> chartsMunic, List<IncomeViewModel> income)
        {
            FillPageInfo(info, view.PageData);
            DataSet data = _repository.FillAssetClassView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "Equities");

            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            HoldingsAdoMapper.MapPageData(data.Tables[2].Rows, view.PageData);
            HoldingsAdoMapper.MapTypeAndMunicCharts(data.Tables[3].Rows, chartsType, chartsMunic);
            HoldingsAdoMapper.MapIncomes(data.Tables[4].Rows, income);
        }

        public void FillOtherAssetsView(UserInfo info, HoldingsCashViewModel view, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<CashViewModel> assetsList)
        {
            FillPageInfo(info, view.PageData);
            DataSet data = _repository.FillAssetClassView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "Other Assets");

            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            HoldingsAdoMapper.MapPageData(data.Tables[2].Rows, view.PageData);
            HoldingsAdoMapper.MapCashes(data.Tables[4].Rows, assetsList);
        }

        private void FillPageInfo(UserInfo info, TopSectionViewModel model)
        {
            model.IsGroup = info.IsGroup;
            model.IsClientGroup = info.IsClientGroup;
            model.EntityId = info.EntityId;
            model.ClientId = info.ContactId;
        }

        public void FillModalView(UserInfo info, int? securityId, List<IncomeViewModel> income)
        {
            DataSet data = _repository.FillModalView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, securityId);

            HoldingsAdoMapper.MapLinkTable(data.Tables[0].Rows, income);
        }
    }
}
