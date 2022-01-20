using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Silvercrest.ViewModels.Client.Holdings.Index;
using Silvercrest.Entities;

namespace Silvercrest.Interfaces
{
    public interface ITransactions
    {
        void FillInfoView(TopSectionViewModel topSectionViewModel, UserInfo info);
        UserInfo GetUserInfo(int? contactId);
        int? GetContactId(string name);
        List<Contribution> GetContributions(UserInfo info, string startDate, string endDate);
        List<Purchase> GetPurchases(UserInfo info, string startDate, string endDate);
        List<Sale> GetSales(UserInfo info, string startDate, string endDate);
        Purchase GetFirstPurchase(UserInfo info);
        Contribution GetFirstContribution(UserInfo info);
        Sale GetFirstSale(UserInfo info);
    }
}
