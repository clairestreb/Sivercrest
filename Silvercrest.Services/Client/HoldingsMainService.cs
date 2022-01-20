using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silvercrest.Services
{
    public class HoldingsMainService : IHoldingsMainService
    {
        private AccountRepository _accountRepository;

        public HoldingsMainService(SLVR_DEVEntities context)
        {
            _accountRepository = new AccountRepository(context);
        }

        public void FillInAccounts(List<AccountViewModel> accounts, int? contactId)
        {
            var accountsList = _accountRepository.GetAccountList(contactId);
            foreach(var account in accountsList)
            {
                var accountViewModel = new AccountViewModel();
                accountViewModel.Account = account.Name;
                accountViewModel.MarketValue = account.MarketValue;
                accountViewModel.PercentOfTotal = Math.Round(account.PercentOfTotal, 2);
                accountViewModel.ClientId = account.ContactId;
                accountViewModel.AccountType = account.IsGroup != null ?
                                !(bool)account.IsGroup ?
                                    "Accounts" :
                                    "Groups" :
                                "undefined";
                accountViewModel.IsGroup = account.IsGroup;
                accountViewModel.IsClientGroup = account.IsClientGroup;
                accountViewModel.EntityId = account.EntityId;
                accounts.Add(accountViewModel);
            };
            accounts.RemoveAll(x => x.Account == "Total");
        }

        public void FillInfoView(TopSectionViewModel topSectionViewModel, UserInfo info, int? type =null)
        {
            topSectionViewModel.IsGroup = info.IsGroup;
            topSectionViewModel.IsClientGroup = info.IsClientGroup;
            topSectionViewModel.EntityId = info.EntityId;
            topSectionViewModel.ClientId = info.ContactId;

            var viewInfo = _accountRepository.GetInfo(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, type);
            if (viewInfo == null)
            {
                return;
            }
            topSectionViewModel.Date = viewInfo.Date ?? DateTime.Now;
            topSectionViewModel.CustodianAccount = viewInfo.CustodianAccount!=null  ? "custodian account: " + viewInfo.CustodianAccount : "";
            topSectionViewModel.Name = viewInfo.Name;
            topSectionViewModel.DateString = "data as of " + DateConverter.ConvertDateToString(viewInfo.Date).ToLower();
            topSectionViewModel.TotalMarketValue = viewInfo.TotalMarketValue;
            topSectionViewModel.MarketValue = viewInfo.MarketValue;
            topSectionViewModel.UnrealizedGL = viewInfo.UnrealizedGL ?? 0;
        }

        //public void FillInBalances(List<BalanceViewModel> balances, UserInfo info)
        //{
        //    var balancesSet = _accountRepository.GetBalances(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
        //    foreach (var balance in balancesSet) {
        //        var balanceViewModel = new BalanceViewModel();
        //        balanceViewModel.Operation = balance.Operation;
        //        balanceViewModel.CashMoneyFunds = balance.CashMoneyFunds;
        //        balanceViewModel.FixedIncome = balance.FixedIncome;
        //        balanceViewModel.Equities = balance.Equities;
        //        balanceViewModel.OtherAssets = balance.OtherAssets;
        //        balanceViewModel.MarketValue = balance.MarketValue;
        //        balanceViewModel.PercentOfTotal = balance.MarketValue / balancesSet.Where(y => y.AssetClass == balance.AssetClass).Sum(z => z.MarketValue) ?? 0;
        //        balanceViewModel.PercentOfAssetClass = balance.MarketValue / balancesSet.Sum(y => y.MarketValue) ?? 0;
        //        balanceViewModel.AnnualIncome = balance.AnnualIncome;
        //        balanceViewModel.CurrentYield = balance.CurrentYield;
        //        balanceViewModel.GroupName = balance.GroupName;
        //        balanceViewModel.AssetClass = balance.AssetClass;
        //        balanceViewModel.Strategy = balance.Strategy;
        //        balanceViewModel.Url = "";
        //        balances.Add(balanceViewModel);
        //    }    
        //}

        public void FillInBalancesShort(List<BalanceViewModel> balances, UserInfo info)
        {
            var shortBalances = _accountRepository.GetBalancesShort(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
            foreach(var balance in shortBalances)
            {
                var balanceViewModel = new BalanceViewModel();
                balanceViewModel.MarketValue = balance.MarketValue ?? 0;
                balanceViewModel.PercentOfTotal = balance.PercentOfTotal ?? 0;
                balanceViewModel.AnnualIncome = balance.AnnualIncome ?? 0;
                balanceViewModel.CurrentYield = balance.CurrentYield ?? 0;
                balanceViewModel.AssetClass = balance.AssetClass ?? "";
                balances.Add(balanceViewModel);
            };
        }

        //public List<PieChart> GetChartStrategy(UserInfo info)
        //{
        //    var result = _accountRepository.GetChartStrategy(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).ToList();
        //    return result.Count > 0 ? result : GetUndefinedChart("Strategy chart");
        //}

        public List<PieChart> GetChartAssetClass(UserInfo info)
        {
            var result  = _accountRepository.GetChartAssetClass(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).Where(x => x.Percent > 0).ToList();
            return result.Count > 0 ? result : GetUndefinedChart("Asset Class chart");
        }

        public int? GetContactId(string name)
        {
            return _accountRepository.GetContactId(name);
        }

        private List<PieChart> GetUndefinedChart(string chartName)
        {
            var result = new List<PieChart>();
            result.Add(new PieChart
            {
                LegendName = chartName + ": data's undefined!",
                Percent = 1
            });
            return result;
        }

        public void FillInCashes(List<CashViewModel> cashes, UserInfo info)
        {
            var cashesList = _accountRepository.GetCashes(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "ca");
            foreach(var cash in cashesList)
            {
                var cashViewModel = new CashViewModel();
                cashViewModel.Holding = cash.Holding;
                cashViewModel.Quantity = cash.Quantity;
                cashViewModel.Total = cash.Total;
                cashViewModel.MarketValueUnits = cash.MarketValueUnits;
                cashViewModel.MarketValueTotal = cash.MarketValueTotal;
                cashViewModel.PercentOfAssets = cash.PercentOfAssets;
                cashViewModel.AnnualIncome = cash.AnnualIncome;
                cashViewModel.Category = cash.Category;
                cashViewModel.CurrentYield = cash.CurrentYield;
                cashes.Add(cashViewModel);
            };

        }

        public void FillInOtherAssets(List<CashViewModel> assets, UserInfo info)
        {
            var assetsList = _accountRepository.GetAssets(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "ot");
            foreach (var asset in assetsList)
            {
                var cashViewModel = new CashViewModel();
                cashViewModel.Holding = asset.Holding;
                cashViewModel.Quantity = asset.Quantity;
                cashViewModel.Total = asset.Total;
                cashViewModel.MarketValueUnits = asset.MarketValueUnits;
                cashViewModel.MarketValueTotal = asset.MarketValueTotal;
                cashViewModel.PercentOfAssets = asset.PercentOfAssets;
                cashViewModel.AnnualIncome = asset.AnnualIncome;
                cashViewModel.Category = asset.Category;
                cashViewModel.CurrentYield = asset.CurrentYield;
                assets.Add(cashViewModel);
            };

        }

        public void FillInIncomes(List<IncomeViewModel> incomes, UserInfo info)
        {
            var incomeList = _accountRepository.GetIncomes(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "fi");
            foreach(var income in incomeList)
            {
                var incomeViewModel = new IncomeViewModel();
                incomeViewModel.Holdings = income.Holdings;
                incomeViewModel.Symbol = income.Symbol;
                incomeViewModel.Quantity = income.Quantity;
                var adjustedCostDate = income.AdjustedCostDate;
                incomeViewModel.AdjustedCostDate = adjustedCostDate.Value.ToString("MM'/'dd'/'yyyy");
                incomeViewModel.AdjustedCostUnit = income.AdjustedCostUnit;
                incomeViewModel.AdjustedCostTotal = income.AdjustedCostTotal;
                incomeViewModel.MarketValueUnits = income.MarketValueUnits;
                incomeViewModel.MarketValueTotal = income.MarketValueTotal;
                incomeViewModel.MarketValuePercentOfAssets = income.MarketValuePercentOfAssets;
                incomeViewModel.AccuredInterest = income.AccuredInterest;
                incomeViewModel.AnnualIncome = income.AnnualIncome;
                incomeViewModel.CurrentYield = income.CurrentYield;
                incomeViewModel.Category = income.Category;
                incomeViewModel.SubCategory = income.SubCategory;
                incomes.Add(incomeViewModel);
            };
        }

        public void FillInEquities(List<IncomeViewModel> equities, UserInfo info)
        {
            var incomesList = _accountRepository.GetIncomes(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, "eq");
            foreach(var income in incomesList)
            {
                var incomeViewModel = new IncomeViewModel();
                incomeViewModel.Holdings = income.Holdings;
                incomeViewModel.Symbol = income.Symbol;
                incomeViewModel.Quantity = income.Quantity;
                var adjustedCostDate = income.AdjustedCostDate;
                incomeViewModel.AdjustedCostDate = adjustedCostDate.Value.ToString("MM'/'dd'/'yyyy");
                incomeViewModel.AdjustedCostUnit = income.AdjustedCostUnit;
                incomeViewModel.AdjustedCostTotal = income.AdjustedCostTotal;
                incomeViewModel.MarketValueUnits = income.MarketValueUnits;
                incomeViewModel.MarketValueTotal = income.MarketValueTotal;
                incomeViewModel.MarketValuePercentOfAssets = income.MarketValuePercentOfAssets;
                incomeViewModel.AccuredInterest = income.AccuredInterest;
                incomeViewModel.AnnualIncome = income.AnnualIncome;
                incomeViewModel.CurrentYield = income.CurrentYield;
                incomeViewModel.Category = income.Category;
                incomeViewModel.SubCategory = income.SubCategory;
                equities.Add(incomeViewModel);
            };
        }

        /*public List<PieChart> GetChartType(UserInfo info)
        {
            var result = _accountRepository.GetChartType(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).ToList();
            return result.Count > 0 ? result : GetUndefinedChart("Type chart");
        }

        public List<PieChart> GetChartMunicBound(UserInfo info)
        {
            var result = _accountRepository.GetChartMunicBound(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).ToList();
            return result.Count > 0 ? result : GetUndefinedChart("Municipal Bound chart");
        }

        public List<PieChart> GetEquitiesChartType(UserInfo info)
        {
            var result = _accountRepository.GetEquitiesChartType(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).ToList();
            return result.Count > 0 ? result : GetUndefinedChart("Type chart");
        }

        public List<PieChart> GetEquitiesChartMunicBound(UserInfo info)
        {
            var result = _accountRepository.GetEquitiesChartMunicBound(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup).ToList();
            return result.Count > 0 ? result : GetUndefinedChart("Municipal Bound chart");
        }*/

        public UserInfo GetUserInfo(int? contactId)
        {
            return _accountRepository.GetUserInfo(contactId);
        }

        public bool GetHoldingsType(int? contactId)
        {
            return _accountRepository.GetHoldingsType(contactId);
        }

        public void FillMultiData(UserInfo info)
        {
            _accountRepository.GetMultiData(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
        }
    }
}
