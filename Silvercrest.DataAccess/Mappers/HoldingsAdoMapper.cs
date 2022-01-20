using Silvercrest.DataAccess.Model;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class HoldingsAdoMapper
    {
        public static void MapAccountsAndGroups(DataRowCollection rows, List<Silvercrest.Entities.Account> accounts, List<Silvercrest.Entities.Account> groups)
        {
            List<Silvercrest.Entities.Account> accountsData = new List<Silvercrest.Entities.Account>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var mappedAccount = new Silvercrest.Entities.Account();
                    mappedAccount.ContactId = (int?)r["contact_id"];
                    mappedAccount.Name = r["display_name"].ToString();
                    mappedAccount.MarketValue = !r.IsNull("total_value") ? (double)r["total_value"] : 0;
                    mappedAccount.PercentOfTotal = !r.IsNull("pct") ? (double)r["pct"] : 0;
                    mappedAccount.IsClientGroup = (bool?)r["is_client_group"];
                    mappedAccount.IsGroup = (bool?)r["is_group"];
                    mappedAccount.EntityId = (int?)r["entity_id"];
                    accountsData.Add(mappedAccount);
                }
            }
            accounts.AddRange(accountsData.Where(x => x.IsGroup == false && x.Name != "Total").ToList());
            groups.AddRange(accountsData.Where(x => x.IsGroup == true).ToList());
        }

        public static void MapAccountsWithinGroups(DataRowCollection rows, List<Silvercrest.Entities.Account> accountsWithinGroups)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var mappedAccount = new Silvercrest.Entities.Account();
                    mappedAccount.ContactId = (int?)r["contact_id"];
                    mappedAccount.Name = r["display_name"].ToString();
                    mappedAccount.IsClientGroup = (int)r["is_client_group"] == 1 ? true : false;
                    mappedAccount.IsGroup = (int)r["is_group"] == 1 ? true : false;
                    mappedAccount.EntityId = (int?)r["entity_id"];
                    accountsWithinGroups.Add(mappedAccount);
                }
            }
        }
        public static void MapHoldingType(DataRowCollection rows, HoldingsViewModel view)
        {
            if (rows.Count > 0)
            {
                var row1 = rows[0];
                view.PageData.HoldingType = (bool)row1["Display_Strategy_Allocation"];
            }
        }

        public static void MapPageData(DataRowCollection rows, TopSectionViewModel model)
        {
            if (rows.Count > 0)
            {
                var row = rows[0];
                model.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                model.CustodianAccount = !String.IsNullOrEmpty(row["custodian_account"].ToString()) ? "custodian account: " + row["custodian_account"].ToString() : "";
                model.Name = row["display_name"].ToString();
                model.DateString = "DATA AS OF " + Silvercrest.Utilities.DateConverter.ConvertDateToString(model.Date).ToLower();
                model.TotalMarketValue = (double?)row["total_market_value"] ?? 0;
                model.MarketValue = (double?)row["market_value"] ?? 0;
                model.UnrealizedGL = (double?)row["unrealized_gl"] ?? 0;
            }
        }

        public static void MapCharts(DataRowCollection rows, List<Silvercrest.Entities.PieChart> charts)
        {
            List<Silvercrest.Entities.PieChart> chartsData = new List<Entities.PieChart>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var chart = new Silvercrest.Entities.PieChart();
                    chart.MarketValue = (double?)r["market_value"];
                    chart.Percent = (double?)r["pct"];
                    chart.LegendName = r["asset_class"].ToString();
                    chartsData.Add(chart);
                }
                chartsData = chartsData.Where(x => x.Percent > 0).OrderByDescending(x => x.Percent).ToList();
            }
            charts.AddRange(chartsData);
        }

        public static void MapShortBalances(DataRowCollection rows, List<BalanceViewModel> shortBalances)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var balance = new BalanceViewModel();
                    balance.MarketValue = (double?)r["market_value"] ?? 0;
                    balance.PercentOfTotal = (double?)r["pct"] ?? 0;
                    balance.AnnualIncome = !r.IsNull("est_annual_income") ? (double?)r["est_annual_income"] : 0;
                    balance.CurrentYield = !r.IsNull("current_yield") ? (double?)r["current_yield"] : 0;
                    balance.AssetClass = r["asset_class"].ToString() ?? "";
                    shortBalances.Add(balance);
                }
            }
        }

        public static void MapStrategyChart(DataRowCollection rows, List<Silvercrest.Entities.PieChart> chartsStrategy)
        {
            List<Silvercrest.Entities.PieChart> chartsData = new List<Entities.PieChart>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var chart = new Silvercrest.Entities.PieChart();
                    chart.MarketValue = (double?)r["market_value"];
                    chart.Percent = (double?)r["pct"];
                    chart.LegendName = r["strategy"].ToString();
                    chartsData.Add(chart);
                }
            }
            chartsData = chartsData.OrderByDescending(x => x.Percent).ToList();
            chartsStrategy.AddRange(chartsData);
            if (chartsStrategy.Count == 0)
            {
                chartsStrategy.Add(new Silvercrest.Entities.PieChart
                {
                    LegendName = "Strategy chart: data's undefined!",
                    Percent = 1
                });
            }
        }

        public static void MapFullBalances(DataRowCollection rows, List<BalanceViewModel> fullBalances)
        {
            List<Silvercrest.Entities.Balance> fullBalancesData = new List<Silvercrest.Entities.Balance>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var fullBalance = new Silvercrest.Entities.Balance();
                    fullBalance.Operation = r["entity_name"].ToString();
                    fullBalance.CashMoneyFunds = (double?)r["Cash___Money_Funds"];
                    fullBalance.FixedIncome = (double?)r["Fixed_Income"];
                    fullBalance.Equities = (double?)r["Equities"];
                    fullBalance.OtherAssets = (double?)r["Other_Assets"];
                    fullBalance.MarketValue = (double)r["ROW_TOTAL"];
                    fullBalance.AnnualIncome = (double?)r["ANNUAL_INCOME"];
                    fullBalance.CurrentYield = (double?)r["CURR_YLD"];
                    fullBalance.GroupName = r["group_name"].ToString();
                    fullBalance.AssetClass = r["outer_asset_class"].ToString();
                    fullBalance.Strategy = r["strategy"].ToString();
                    fullBalance.OuterSortOrder = (int)r["outer_sort_order"];
                    fullBalancesData.Add(fullBalance);
                }
            }
            fullBalancesData = fullBalancesData.OrderBy(x => x.OuterSortOrder).ToList();
            foreach (var balance in fullBalancesData)
            {
                balance.PercentOfTotal = balance.MarketValue / fullBalancesData.Sum(x => x.MarketValue) * 100;
                var fullBalance = new BalanceViewModel();
                fullBalance.Operation = balance.Operation;
                fullBalance.CashMoneyFunds = balance.CashMoneyFunds;
                fullBalance.FixedIncome = balance.FixedIncome;
                fullBalance.Equities = balance.Equities;
                fullBalance.OtherAssets = balance.OtherAssets;
                fullBalance.MarketValue = balance.MarketValue;
                fullBalance.PercentOfTotal = balance.MarketValue / fullBalancesData.Where(y => y.AssetClass == balance.AssetClass).Sum(z => z.MarketValue) ?? 0;
                fullBalance.PercentOfAssetClass = balance.MarketValue / fullBalancesData.Sum(y => y.MarketValue) ?? 0;
                fullBalance.AnnualIncome = balance.AnnualIncome;
                fullBalance.CurrentYield = balance.CurrentYield;
                fullBalance.GroupName = balance.GroupName;
                fullBalance.AssetClass = balance.AssetClass;
                fullBalance.Strategy = balance.Strategy;
                fullBalance.Url = "";
                fullBalances.Add(fullBalance);
            }
        }

        public static void MapCashes(DataRowCollection rows, List<CashViewModel> cashList)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var mappedCash = new CashViewModel();
                    mappedCash.Holding = r["security_name"].ToString();
                    mappedCash.Quantity = (double?)r["quantity"];
                    mappedCash.MarketValueUnits = (double?)r["base_price"];
                    mappedCash.MarketValueTotal = (double?)r["base_total_value"];
//                    mappedCash.PercentOfAssets = (double?)r["pct_of_total_value"];
                    mappedCash.PercentOfAssets = (double?)r["pct_of_asset_class"];
                    mappedCash.AnnualIncome = (double?)r["base_annual_income"];
                    mappedCash.CurrentYield = (double?)r["yield"];
                    mappedCash.Category = r["grp1"].ToString();
                    cashList.Add(mappedCash);
                }
            }
        }

        public static void MapIncomes(DataRowCollection rows, List<IncomeViewModel> income)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var incomeViewModel = new IncomeViewModel();
                    incomeViewModel.Holdings = r["security_name"].ToString();
                    if (!String.IsNullOrEmpty(r["bond_description"].ToString()))
                    {
                        incomeViewModel.Holdings += "\n" + r["bond_description"].ToString();
                    }
                    incomeViewModel.Symbol = r["symbol"].ToString();
                    incomeViewModel.Quantity = (double?)r["quantity"] ?? 0;
                    var adjustedCostDate = (DateTime?)r["acq_date"];
                    incomeViewModel.AdjustedCostDate = adjustedCostDate.Value.ToString("MM'/'dd'/'yyyy");
                    incomeViewModel.AdjustedCostUnit = (double?)r["base_unit_cost"] ?? 0;
                    incomeViewModel.AdjustedCostTotal = (double?)r["base_cost_basis"] ?? 0;
                    incomeViewModel.MarketValueUnits = (double?)r["base_price"] ?? 0;
                    incomeViewModel.MarketValueTotal = (double?)r["base_total_value"] ?? 0;
                    incomeViewModel.MarketValuePercentOfAssets = (double?)r["pct_of_asset_class"] ?? 0;
//                    incomeViewModel.MarketValuePercentOfAssets = (double?)r["pct_of_total_value"] ?? 0;
                    incomeViewModel.AccuredInterest = (double?)r["base_accrued_interest"] ?? 0;
                    incomeViewModel.AnnualIncome = (double?)r["base_annual_income"] ?? 0;
                    incomeViewModel.CurrentYield = (double?)r["yield"] ?? 0;
                    incomeViewModel.Category = r["grp1"].ToString();
                    incomeViewModel.SubCategory = r["grp2"].ToString();
                    incomeViewModel.CategorySort = (int)r["grp1_sort"];
                    incomeViewModel.SubCategorySort = (int)r["grp2_sort"];
                    incomeViewModel.SecurityId = (int)r["security_id"];
                    incomeViewModel.NumberLots = (int)r["num_lots"];
                    income.Add(incomeViewModel);
                }
            }
        }

        public static void MapLinkTable(DataRowCollection rows, List<IncomeViewModel> income)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var incomeViewModel = new IncomeViewModel();
                    incomeViewModel.SecurityName = r["security_name"].ToString();
                    if (!String.IsNullOrEmpty(r["bond_description"].ToString()))
                    {
                        incomeViewModel.SecurityName += "\n" + r["bond_description"].ToString();
                    }
                    incomeViewModel.Symbol = r["symbol"].ToString();
                    incomeViewModel.Quantity = (double?)r["quantity"] ?? 0;
                    var adjustedCostDate = (DateTime?)r["acq_date"];
                    incomeViewModel.AdjustedCostDate = adjustedCostDate.Value.ToString("MM'/'dd'/'yyyy");
                    incomeViewModel.AdjustedCostUnit = (double?)r["base_unit_cost"] ?? 0;
                    incomeViewModel.AdjustedCostTotal = (double?)r["base_cost_basis"] ?? 0;
                    incomeViewModel.MarketValueUnits = (double?)r["base_price"] ?? 0;
                    incomeViewModel.MarketValueTotal = (double?)r["base_total_value"] ?? 0;
                    incomeViewModel.AccuredInterest = (double?)r["base_accrued_interest"] ?? 0;
                    incomeViewModel.AnnualIncome = (double?)r["base_annual_income"] ?? 0;
                    incomeViewModel.CurrentYield = (double?)r["yield"] ?? 0;
                    incomeViewModel.Holdings = r["display_name"].ToString();
                    income.Add(incomeViewModel);
                }
            }
        }


        public static void MapTypeAndMunicCharts(DataRowCollection rows, List<Silvercrest.Entities.PieChart> chartsType, List<Silvercrest.Entities.PieChart> chartsMunic)
        {
            List<IncomeDataViewModel> chartsData = new List<IncomeDataViewModel>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var chartData = new IncomeDataViewModel();
                    chartData.SecType = r["SecType"].ToString();
                    chartData.SecType_Percent = (double?)r["SecType_Percent"];
                    chartData.Market_Value = (double?)r["Market_Value"];
                    chartData.largest_sectype = r["largest_sectype"].ToString();
                    chartData.Sector_Percent = (double?)r["Sector_Percent"];
                    chartData.Sector = r["Sector"].ToString();
                    chartsData.Add(chartData);
                }
            }
            chartsType.AddRange(chartsData
               .GroupBy(p => p.SecType)
               .Select(g => new
               {
                   list = g.Select(p => p)
               })
                .Select(x => new Silvercrest.Entities.PieChart
                {
                    LegendName = x.list.FirstOrDefault().SecType,
                    Percent = x.list.Sum(s => s.SecType_Percent),
                    MarketValue = x.list.Sum(s => s.Market_Value)
                })
                .OrderByDescending(x => x.Percent)
               .ToList());

            if (!(chartsType.Count > 0))
            {
                chartsType.Add(new Silvercrest.Entities.PieChart
                {
                    LegendName = "Type chart: data's undefined!",
                    MarketValue = 0,
                    Percent = 1
                });
            }

            chartsMunic.AddRange(chartsData
                .Where(x => x.SecType == x.largest_sectype)
                .OrderByDescending(x => x.Sector_Percent)
                .Select(x =>
                    new Silvercrest.Entities.PieChart
                    {
                        MarketValue = x.Market_Value,
                        Percent = x.Sector_Percent,
                        LegendName = x.Sector,
                        Title = x.largest_sectype
                    }).ToList());

            if (!(chartsMunic.Count > 0))
            {
                chartsMunic.Add(new Silvercrest.Entities.PieChart
                {
                    LegendName = "Municipal Bound chart: data's undefined!",
                    MarketValue = 0,
                    Percent = 1
                });
            }
        }
    }
}
