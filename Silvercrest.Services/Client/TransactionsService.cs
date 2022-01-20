using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Interfaces;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.DataAccess.Model;
using Silvercrest.ViewModels.Client;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System.Globalization;
using Silvercrest.Entities;
using Silvercrest.Utilities;

namespace Silvercrest.Services
{
    public class TransactionsService : ITransactions
    {
        private AccountRepository _accountRepository;

        public TransactionsService(SLVR_DEVEntities context)
        {
            _accountRepository = new AccountRepository(context);
        }

        public void FillInfoView(TopSectionViewModel topSectionViewModel, UserInfo info)
        {
            var viewInfo = _accountRepository.GetInfo(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup);
            topSectionViewModel.IsGroup = info.IsGroup;

            if (viewInfo == null)
            {
                topSectionViewModel.Date = DateTime.Now;
                topSectionViewModel.DateString = DateConverter.ConvertDateToString(DateTime.Now);
                topSectionViewModel.CustodianAccount = "";
                topSectionViewModel.Name = "Name";
                return;
            }
            topSectionViewModel.Date = (viewInfo.Date ?? DateTime.Now);
            topSectionViewModel.DateString = DateConverter.ConvertDateToString(viewInfo.Date);
            topSectionViewModel.CustodianAccount = viewInfo.CustodianAccount == null ? "" : "custodian account: " + viewInfo.CustodianAccount;
            topSectionViewModel.Name = viewInfo.Name ?? "Name";
        }

        public UserInfo GetUserInfo(int? contactId)
        {
            return _accountRepository.GetUserInfo(contactId);
        }

        public int? GetContactId(string name)
        {
            return _accountRepository.GetContactId(name);
        }

        public List<Contribution> GetContributions(UserInfo info, string startDate, string endDate)
        {
            return _accountRepository.GetContributions(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate).ToList();
        }

        public List<Purchase> GetPurchases(UserInfo info, string startDate, string endDate)
        {
            return _accountRepository.GetPurchases(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate);
        }

        public Purchase GetFirstPurchase(UserInfo info)
        {
            return _accountRepository.GetPurchases(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, null, null).FirstOrDefault();
        }

        public Contribution GetFirstContribution(UserInfo info)
        {
            return _accountRepository.GetContributions(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, null, null).FirstOrDefault();
        }

        public Sale GetFirstSale(UserInfo info)
        {
            return _accountRepository.GetSales(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, null, null).FirstOrDefault();
        }

        public List<Sale> GetSales(UserInfo info, string startDate, string endDate)
        {
            return _accountRepository.GetSales(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, startDate, endDate);
        }

    }
}
