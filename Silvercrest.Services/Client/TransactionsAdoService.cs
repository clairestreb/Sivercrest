using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.Client.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services.Client
{
    public class TransactionsAdoService : ITransactionsAdoService
    {
        private TransactionsRepository _repository;

        public TransactionsAdoService()
        {
            _repository = new TransactionsRepository();
        }

        public void FillTransactionsView(TransactionsViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel)
        {
            DataSet data = _repository.TransactionsInit(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);

            if (data.Tables[2].Rows.Count > 0)
            {
                var row = data.Tables[2].Rows[0];
                topSectionViewModel.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                topSectionViewModel.CustodianAccount = !String.IsNullOrEmpty(row["custodian_account"].ToString()) ? "custodian account: " + row["custodian_account"].ToString() : "";
                topSectionViewModel.Name = row["display_name"].ToString();
                topSectionViewModel.DateString = "data as of " + Silvercrest.Utilities.DateConverter.ConvertDateToString(topSectionViewModel.Date).ToLower();
            }
            topSectionViewModel.IsGroup = info.IsGroup;

        }

        public void FillPurchasesView(TransactionsPurchasesViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Purchase> mappedPurchasesList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate)
        {
            DataSet data = _repository.TransactionsPurchases(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate);
            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            string sDate = startDate;
            string eDate = endDate;
            if (data.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[2].Rows)
                {
                    var mappedPurchase = new Silvercrest.Entities.Purchase();
                    mappedPurchase.StartDate = ((DateTime?)row["start_date"]).Value.ToString("MM'/'dd'/'yyyy") ;
                    mappedPurchase.EndDate = ((DateTime?)row["end_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    sDate = mappedPurchase.StartDate;
                    eDate = mappedPurchase.EndDate;
                    mappedPurchase.Quantity = Convert.IsDBNull(row["quantity"]) ? null : (double?)row["quantity"];
                    mappedPurchase.SecurityName = row["security_name"].ToString();
                    mappedPurchase.Symbol = row["symbol"].ToString();
                    mappedPurchase.TradeDate = ((DateTime?)row["trade_date"]).Value.ToString("MM'/'dd'/'yyyy"); ;
                    mappedPurchase.UsdAmount = (double?)row["usd_trade_amount"];
                    mappedPurchase.GroupingOne = row["grouping1_name"].ToString();
                    mappedPurchase.GroupingSecond = row["grouping2_name"].ToString();
                    mappedPurchase.SortingOne = (int)row["SRT"];
                    mappedPurchase.SortingSecond = (int)row["CSRT"];
                    mappedPurchasesList.Add(mappedPurchase);
                }
            }

            if (data.Tables[3].Rows.Count > 0)
            {
                var row = data.Tables[3].Rows[0];
                topSectionViewModel.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                topSectionViewModel.CustodianAccount = !String.IsNullOrEmpty(row["custodian_account"].ToString()) ? "custodian account: " + row["custodian_account"].ToString() : "";
                topSectionViewModel.Name = row["display_name"].ToString();
                topSectionViewModel.DateString = "data as of " + Silvercrest.Utilities.DateConverter.ConvertDateToString(topSectionViewModel.Date).ToLower();
            }

            if((sDate == null)||(eDate == null))
            {
                eDate = topSectionViewModel.Date.ToString("MM'/'dd'/'yyyy");
                sDate = topSectionViewModel.Date.AddMonths(-3).ToString("MM'/'dd'/'yyyy");
            }
            topSectionViewModel.StartDate = sDate;
            topSectionViewModel.EndDate = eDate;
            topSectionViewModel.IsGroup = info.IsGroup;

        }
        public void FillContributionsView(TransactionsContributionsViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Contribution> mappedContributionList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate)
        {
            DataSet data = _repository.TransactionsContributions(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate);
            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            string sDate = startDate;
            string eDate = endDate;
            if (data.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[2].Rows)
                {
                    var mappedContribution = new Silvercrest.Entities.Contribution();
                    mappedContribution.StartDate = ((DateTime?)row["start_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    mappedContribution.EndDate = ((DateTime?)row["end_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    sDate = mappedContribution.StartDate;
                    eDate = mappedContribution.EndDate;
                    mappedContribution.TransactionType = row["transaction_type"].ToString();
                    mappedContribution.SecurityName = row["security_name"].ToString();
                    mappedContribution.Comment = String.IsNullOrEmpty(row["custom_comment"].ToString()) ? row["comment"].ToString() : row["custom_comment"].ToString();
                    mappedContribution.TradeDate = ((DateTime?)row["trade_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    mappedContribution.UsdAmount = (double)row["usd_trade_amount"];
                    mappedContributionList.Add(mappedContribution);
                }
            }

            if (data.Tables[3].Rows.Count > 0)
            {
                var row = data.Tables[3].Rows[0];
                topSectionViewModel.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                topSectionViewModel.CustodianAccount = !String.IsNullOrEmpty(row["custodian_account"].ToString()) ? "custodian account: " + row["custodian_account"].ToString() : "";
                topSectionViewModel.Name = row["display_name"].ToString();
                topSectionViewModel.DateString = "data as of " + Silvercrest.Utilities.DateConverter.ConvertDateToString(topSectionViewModel.Date).ToLower();
            }

            if ((sDate == null) || (eDate == null))
            {
                eDate = topSectionViewModel.Date.ToString("MM'/'dd'/'yyyy");
                sDate = topSectionViewModel.Date.AddMonths(-3).ToString("MM'/'dd'/'yyyy");
            }
            topSectionViewModel.StartDate = sDate;
            topSectionViewModel.EndDate = eDate;
            topSectionViewModel.IsGroup = info.IsGroup;

        }
        public void FillSalesView(TransactionsSalesViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Sale> mappedSaleList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate)
        {
            DataSet data = _repository.TransactionsSales(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate);
            HoldingsAdoMapper.MapAccountsAndGroups(data.Tables[0].Rows, accounts, groups);
            HoldingsAdoMapper.MapAccountsWithinGroups(data.Tables[1].Rows, accountsWithinGroups);
            string sDate = startDate;
            string eDate = endDate;
            if (data.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[2].Rows)
                {
                    var mappedSale = new Silvercrest.Entities.Sale();
                    mappedSale.StartDate = ((DateTime?)row["start_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    mappedSale.EndDate = ((DateTime?)row["end_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    sDate = mappedSale.StartDate;
                    eDate = mappedSale.EndDate;
                    mappedSale.Security = row["security_name"].ToString();
                    mappedSale.Quantity = Convert.IsDBNull(row["quantity"])? null : (double?)row["quantity"];
                    mappedSale.OpenDate = Convert.IsDBNull(row["open_date"]) ? null :((DateTime?)row["open_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    mappedSale.OriginalTotal = Convert.IsDBNull(row["cost_basis"]) ? null : (double?)row["cost_basis"];
                    mappedSale.AdjustedUnit = Convert.IsDBNull(row["quantity"]) ? null : (double?)row["adj_cost_basis"] / (double?)row["quantity"];
                    mappedSale.AdjustedTotal = Convert.IsDBNull(row["adj_cost_basis"]) ? null : (double?)row["adj_cost_basis"];
                    mappedSale.CloseDate = ((DateTime?)row["close_date"]).Value.ToString("MM'/'dd'/'yyyy");
                    mappedSale.ProceedsUnit = Convert.IsDBNull(row["quantity"]) ? null : (double?)row["proceeds"] / (double?)row["quantity"];
                    mappedSale.ProceedsTotal = Convert.IsDBNull(row["proceeds"]) ? null : (double?)row["proceeds"];
                    mappedSale.ShortTerm = Convert.IsDBNull(row["realized_gl_st"]) ? null : (double?)row["realized_gl_st"];
                    mappedSale.LongTerm = (double?)row["realized_gl_lt"] + (double?)row["realized_gl_5y"];
                    mappedSale.GroupingOne = row["grouping1_name"].ToString();
                    mappedSale.GroupingSecond = row["grouping2_name"].ToString();
                    mappedSale.SortingOne = (int)row["SRT"];
                    mappedSale.SortingSecond = (int)row["CSRT"];
                    mappedSaleList.Add(mappedSale);
                }
            }

            if (data.Tables[3].Rows.Count > 0)
            {
                var row = data.Tables[3].Rows[0];
                topSectionViewModel.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                topSectionViewModel.CustodianAccount = !String.IsNullOrEmpty(row["custodian_account"].ToString()) ? "custodian account: " + row["custodian_account"].ToString() : "";
                topSectionViewModel.Name = row["display_name"].ToString();
                topSectionViewModel.DateString = "data as of " + Silvercrest.Utilities.DateConverter.ConvertDateToString(topSectionViewModel.Date).ToLower();
            }

            if ((sDate == null) || (eDate == null))
            {
                eDate = topSectionViewModel.Date.ToString("MM'/'dd'/'yyyy");
                sDate = topSectionViewModel.Date.AddMonths(-3).ToString("MM'/'dd'/'yyyy");
            }
            topSectionViewModel.StartDate = sDate;
            topSectionViewModel.EndDate = eDate;
            topSectionViewModel.IsGroup = info.IsGroup;
        }

    }
}
