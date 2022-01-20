using Silvercrest.Entities;
using Silvercrest.ViewModels.Client.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface ITransactionsAdoService
    {

        void FillPurchasesView(TransactionsPurchasesViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Purchase> mappedPurchasesList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate);
        void FillTransactionsView(TransactionsViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel);
        void FillSalesView(TransactionsSalesViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Sale> mappedSaleList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate);
        void FillContributionsView(TransactionsContributionsViewModel model, UserInfo info, List<Account> accounts, List<Account> groups, List<Account> accountsWithinGroups, List<Contribution> mappedContributionList, Silvercrest.ViewModels.Client.Holdings.Index.TopSectionViewModel topSectionViewModel, string startDate, string endDate);
    }
}
