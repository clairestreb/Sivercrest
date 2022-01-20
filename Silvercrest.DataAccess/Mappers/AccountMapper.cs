using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
namespace Silvercrest.DataAccess.Mappers
{
    public static class AccountMapper
    {
        public static List<Silvercrest.Entities.Account> MapAccountsList(IList<p_Web_Contact_Entities_Result> list)
        {
            var mappedAccountList = new List<Silvercrest.Entities.Account>();
            foreach (var account in list)
            {
                var mappedAccount = new Silvercrest.Entities.Account();
                mappedAccount.ContactId = account.contact_id;
                mappedAccount.Name = account.display_name;
                mappedAccount.MarketValue = account.total_value ?? 0;
                mappedAccount.PercentOfTotal = account.pct ?? 0;
                mappedAccount.IsClientGroup = account.is_client_group;
                mappedAccount.IsGroup = account.is_group;
                mappedAccount.EntityId = account.entity_id;
                mappedAccountList.Add(mappedAccount);
            }
            return mappedAccountList;
        }

        internal static List<Entities.Account> MapAccountsWithinGroup(List<f_Web_Accounts_Within_Group_Result> accountList)
        {
            var mappedAccountList = new List<Entities.Account>();
            foreach(var account in accountList)
            {
                var mappedAccount = new Silvercrest.Entities.Account();
                mappedAccount.ContactId = account.contact_id;
                mappedAccount.Name = account.display_name;
                mappedAccount.IsClientGroup = account.is_client_group == 1 ? true : false;
                mappedAccount.IsGroup = account.is_group == 1? true :false;
                mappedAccount.EntityId = account.entity_id;
                mappedAccountList.Add(mappedAccount);
            }

            return mappedAccountList;
        }

        public static List<Silvercrest.Entities.Account> MapRelationshipAccountsList(IList<p_Web_Relationship_Accounts_Result> list)
        {
            var mappedAccountList = new List<Silvercrest.Entities.Account>();
            foreach (var account in list)
            {
                var mappedAccount = new Silvercrest.Entities.Account();
                mappedAccount.Name = account.display_name;
                mappedAccount.MarketValue = account.market_value ?? 0;
                mappedAccount.PercentOfTotal = account.pct ?? 0;
                mappedAccountList.Add(mappedAccount);
            }
            return mappedAccountList;
        }

        public static List<Silvercrest.Entities.Contribution> MapContributionList(IList<p_Web_Transactions_Result> list)
        {
            var mappedContributionList = new List<Silvercrest.Entities.Contribution>();
            foreach (var contribution in list)
            {
                var mappedContribution = new Silvercrest.Entities.Contribution();
                mappedContribution.SecurityName = contribution.security_name;
                mappedContribution.TradeDate = contribution.trade_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedContribution.UsdAmount = contribution.usd_trade_amount ?? 0;
                mappedContribution.Comment = contribution.comment;
                mappedContribution.TransactionType = contribution.transaction_type;
                mappedContribution.StartDate = contribution.start_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedContribution.EndDate = contribution.end_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedContributionList.Add(mappedContribution);
            }
            return mappedContributionList;
        }

        public static List<AccountNickname> MapNicknames(IList<p_Web_Account_Nicknames_Result> result)
        {
            var mappedNicknameList = new List<Silvercrest.Entities.AccountNickname>();
            foreach (var nickname in result)
            {
                var mappedContribution = new Silvercrest.Entities.AccountNickname();
                mappedContribution.ContactId = nickname.contact_id;
                mappedContribution.AccountId = nickname.account_id;
                mappedContribution.Name = nickname.account_name;
                mappedContribution.Nickname = nickname.nickname;
                //mappedContribution.InsertBy = nickname.comment;
                //mappedContribution.InsertDate = nickname.transaction_type;
                //mappedContribution.UpdateBy = nickname.start_date.Value.ToString("MM'/'dd'/'yyyy");
                //mappedContribution.UpdateDate = nickname.end_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedNicknameList.Add(mappedContribution);
            }
            return mappedNicknameList;

        }

        public static List<Silvercrest.Entities.Sale> MapSalesList(IList<p_Web_RealizedGL_Result> list)
        {
            var mappedSalesList = new List<Silvercrest.Entities.Sale>();
            foreach (var sale in list)
            {
                var mappedSale = new Silvercrest.Entities.Sale();
                mappedSale.Security = sale.security_name;
                mappedSale.Quantity = sale.quantity;
                mappedSale.OpenDate = sale.open_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedSale.OriginalTotal = sale.cost_basis;
                mappedSale.AdjustedUnit = sale.adj_cost_basis / sale.quantity;
                mappedSale.AdjustedTotal = sale.adj_cost_basis;
                mappedSale.CloseDate = sale.close_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedSale.ProceedsUnit = sale.proceeds / sale.quantity;
                mappedSale.ProceedsTotal = sale.proceeds;
                mappedSale.ShortTerm = sale.realized_gl_st;
                mappedSale.LongTerm = sale.realized_gl_lt + sale.realized_gl_5y;
                mappedSale.GroupingOne = sale.grouping1_name;
                mappedSale.GroupingSecond = sale.grouping2_name;
                mappedSale.StartDate = sale.start_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedSale.EndDate = sale.end_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedSalesList.Add(mappedSale);
            }
            return mappedSalesList;
        }

        public static List<Silvercrest.Entities.Purchase> MapPurchasesList(IList<p_Web_Transactions_Result> list)
        {
            var mappedPurchasesList = new List<Silvercrest.Entities.Purchase>();
            foreach (var purchase in list)
            {
                var mappedPurchase = new Silvercrest.Entities.Purchase();

                mappedPurchase.SecurityName = purchase.security_name;
                mappedPurchase.TradeDate = purchase.trade_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedPurchase.UsdAmount = purchase.usd_trade_amount ?? 0;
                mappedPurchase.Quantity = purchase.quantity;
                mappedPurchase.Symbol = purchase.symbol;
                mappedPurchase.StartDate = purchase.start_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedPurchase.EndDate = purchase.end_date.Value.ToString("MM'/'dd'/'yyyy");
                mappedPurchasesList.Add(mappedPurchase);
            }
            return mappedPurchasesList;
        }


        public static UserInfo MapUserInfo(IList<p_Web_Contact_Entities_Result> list)
        {
            var mappedInfo = new Silvercrest.Entities.UserInfo();
            var info = list.Where(x => x.is_default == true).FirstOrDefault();
            mappedInfo.ContactId = info.contact_id;
            mappedInfo.EntityId = info.entity_id;
            mappedInfo.IsGroup = info.is_group;
            mappedInfo.IsClientGroup = info.is_client_group;
            mappedInfo.FullName = info.display_name;
            return mappedInfo;
        }
        
        public static Info MapInfoItem(IList<p_Web_Allocation_By_Asset_Class_Result> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            var infoItem = new Silvercrest.Entities.Info();
            infoItem.Date = list[0].as_of_date;
            infoItem.CustodianAccount = list[0].custodian_account;
            infoItem.Name = list[0].display_name;
            infoItem.TotalMarketValue = list[0].total_market_value ?? 0;
            infoItem.MarketValue = list[0].market_value ?? 0;
            infoItem.UnrealizedGL = list[0].unrealized_gl;
            return infoItem;
        }
        #region-------------------------------------------charts
        //public static List<PieChart> MapChartsItem(IList<p_Web_Top8_Strategies_Result> charts)
        //{
        //    return charts.OrderByDescending(x => x.pct).Select(x => new PieChart
        //    {
        //        MarketValue = x.market_value,
        //        Percent = x.pct,
        //        LegendName = x.strategy
        //    }).ToList();
        //}

        public static List<PieChart> MapChartsItem(IList<p_Web_Allocation_By_Asset_Class_Result> list)
        {
            return list.OrderByDescending(x => x.pct).Select(x => new PieChart
            {
                MarketValue = x.market_value,
                Percent = x.pct,
                LegendName = x.asset_class
            }).ToList();
        }

        /*public static List<PieChart> MapTypeCharts(IList<p_Web_FI_Allocation_Result> charts)
        {
            return charts
                .GroupBy(p => p.SecType)
                .Select(g => new
                {
                    list = g.Select(p => p)
                })
                 .Select(x => new PieChart
                 {
                     LegendName = x.list.FirstOrDefault().SecType,
                     Percent = x.list.Sum(s => s.SecType_Percent),
                     MarketValue = x.list.Sum(s => s.Market_Value)
                 })
                 .OrderByDescending(x=>x.Percent)
                .ToList();

        }*/
        /*public static List<PieChart> MapMunicBoundCharts(IList<p_Web_FI_Allocation_Result> charts)
        {
            return charts
                .Where(x => x.SecType == x.largest_sectype)
                .OrderByDescending(x => x.Sector_Percent)
                .Select(x =>
                    new PieChart
                    {
                        MarketValue = x.Market_Value,
                        Percent = x.Sector_Percent,
                        LegendName = x.Sector,
                        Title = x.largest_sectype
                    }).ToList();
        }*/

       /* public static List<PieChart> MapTypeCharts(IList<p_Web_EQ_Allocation_Result> charts)
        {
            return charts
               
                .GroupBy(p => p.SecType)
                .Select(g => new
                {
                    list = g.Select(p => p)
                })
                 .Select(x => new PieChart
                 {
                     LegendName = x.list.FirstOrDefault().SecType,
                     Percent = x.list.Sum(s => s.SecType_Percent),
                     MarketValue = x.list.Sum(s => s.Market_Value)
                 })
                 .OrderByDescending(x=>x.Percent)
                .ToList();
        }*/

        /*public static List<PieChart> MapMunicBoundCharts(IList<p_Web_EQ_Allocation_Result> charts)
        {
            return charts
                .Where(x => x.SecType == x.largest_sectype)
                .OrderByDescending(x=>x.SecType_Percent)
                .Select(x =>
                    new PieChart
                    {
                        MarketValue = x.Market_Value,
                        Percent = x.Sector_Percent,
                        LegendName = x.Sector
                    }).ToList();
        }*/

        #endregion
        public static List<Balance> MapBalanceShortItem(IList<p_Web_Allocation_By_Asset_Class_Result> balances)
        {
            var mappedBalanceList = new List<Silvercrest.Entities.Balance>();
            foreach (var balance in balances)
            {
                var mappedBalance = new Silvercrest.Entities.Balance();
                mappedBalance.MarketValue = balance.market_value;
                mappedBalance.PercentOfTotal = balance.pct;
                mappedBalance.AnnualIncome = balance.est_annual_income;
                mappedBalance.CurrentYield = balance.current_yield;
                mappedBalance.AssetClass = balance.asset_class;
                mappedBalanceList.Add(mappedBalance);
            }
            return mappedBalanceList;
        }

        //public static List<Balance> MapBalanceItem(IList<p_Web_Allocation_By_Strategy_Result> balances)
        //{
        //    balances = balances.OrderBy(x => x.outer_sort_order).ToList();
        //    var mappedBalanceList = new List<Silvercrest.Entities.Balance>();
        //    foreach (var balance in balances)
        //    {
        //        var mappedBalance = new Silvercrest.Entities.Balance();
        //        mappedBalance.Operation = balance.entity_name;
        //        mappedBalance.CashMoneyFunds = balance.Cash___Money_Funds;
        //        mappedBalance.FixedIncome = balance.Fixed_Income;
        //        mappedBalance.Equities = balance.Equities;
        //        mappedBalance.OtherAssets = balance.Other_Assets;
        //        mappedBalance.MarketValue = balance.ROW_TOTAL;
        //        mappedBalance.PercentOfTotal = balance.ROW_TOTAL / balances.Sum(x => x.ROW_TOTAL) * 100;
        //        mappedBalance.AnnualIncome = balance.ANNUAL_INCOME;
        //        mappedBalance.CurrentYield = balance.CURR_YLD;
        //        mappedBalance.GroupName = balance.group_name;
        //        mappedBalance.AssetClass = balance.outer_asset_class;
        //        mappedBalance.Strategy = balance.strategy;
        //        mappedBalanceList.Add(mappedBalance);
        //    }
        //    return mappedBalanceList;
        //}

        public static List<Silvercrest.Entities.ClientAccount> MapAccountsAndGroupsList(IList<p_Web_Contact_Entities_Result> list)
        {
            var mappedAccountList = new List<Silvercrest.Entities.ClientAccount>();
            foreach (var account in list)
            {
                var mappedAccount = new Silvercrest.Entities.ClientAccount();
                mappedAccount.Name = account.display_name;
                mappedAccount.TotalValue = account.total_value ?? 0;
                mappedAccount.PercentOfTotal = account.pct ?? 0;
                mappedAccount.ContactId = account.contact_id;
                mappedAccount.IsGroup = account.is_group;
                mappedAccount.IsClientGroup = account.is_client_group;
                mappedAccount.SortOrder = account.sort_order;
                mappedAccount.Date = account.as_of_date ?? DateTime.Now;
                mappedAccount.IsDefault = account.is_default ?? false;
                mappedAccount.EntityId = account.entity_id;
                mappedAccount.AccountType = (account.is_client_group == true || account.is_group == true) ? "Groups" : "Accounts";
                mappedAccountList.Add(mappedAccount);
            }
            return mappedAccountList;
        }

        public static List<Cash> MapCashItem(IList<p_Web_Positions_By_Asset_Class_Result> list)
        {
            var mappedCashList = new List<Silvercrest.Entities.Cash>();
            foreach (var cash in list)
            {
                var mappedCash = new Silvercrest.Entities.Cash();
                mappedCash.Holding = cash.security_name;
                mappedCash.Quantity = cash.quantity;
                mappedCash.MarketValueUnits = cash.base_price;
                mappedCash.MarketValueTotal = cash.base_total_value;
                mappedCash.PercentOfAssets = cash.pct_of_total_value;
                mappedCash.AnnualIncome = cash.base_annual_income;
                mappedCash.CurrentYield = cash.yield;
                mappedCash.Category = cash.grp1;
                mappedCashList.Add(mappedCash);
            }
            return mappedCashList;
        }

        public static List<Income> MapIncomeItem(IList<p_Web_Positions_By_Asset_Class_Result> list)
        {
            var mappedIncomeList = new List<Silvercrest.Entities.Income>();
            foreach (var income in list)
            {
                var mappedIncome = new Silvercrest.Entities.Income();
                mappedIncome.Holdings = income.security_name;
                if (income.bond_description != "")
                {
                    mappedIncome.Holdings += "\n" + income.bond_description;
                }
                mappedIncome.Symbol = income.symbol;
                mappedIncome.Quantity = income.quantity ?? 0;
                mappedIncome.AdjustedCostDate = income.acq_date;
                mappedIncome.AdjustedCostUnit = income.base_unit_cost ?? 0;
                mappedIncome.AdjustedCostTotal = income.base_cost_basis ?? 0;
                mappedIncome.MarketValueUnits = income.base_price ?? 0;
                mappedIncome.MarketValueTotal = income.base_total_value ?? 0;
                mappedIncome.MarketValuePercentOfAssets = income.pct_of_total_value ?? 0;
                mappedIncome.AccuredInterest = income.base_accrued_interest ?? 0;
                mappedIncome.AnnualIncome = income.base_annual_income ?? 0;
                mappedIncome.CurrentYield = income.yield ?? 0;
                mappedIncome.Category = income.grp1;
                mappedIncome.SubCategory = income.grp2;
                mappedIncomeList.Add(mappedIncome);
            }
            return mappedIncomeList;
        }

        
    }
}
