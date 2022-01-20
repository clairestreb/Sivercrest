using Silvercrest.DataAccess;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.ViewModels.TeamMember;
using System.Collections;
using Silvercrest.Entities.Enums;

namespace Silvercrest.Services
{
    public class ManagerService : IManagerService
    {
        private ManagerRepository _managerRepository;
        private AccountRepository _accountRepository;
        private ContactRepository _contactRepository;

        public ManagerService(SLVR_DEVEntities context)
        {
            _contactRepository = new ContactRepository(context);
            _accountRepository = new AccountRepository(context);
            _managerRepository = new ManagerRepository(context);
        }

        public void Fill(Hashtable hs)
        {

        }

        public List<ManagerComplete> GetFamilies(string code)
        {
            return _managerRepository.GetManagerList(code);
        }

        public List<ManagerContactComplete> GetContactsByFamily(int? id)
        {
            return _contactRepository.GetContactList(id);
        }

        public List<Entities.Account> GetAccountsByFamily(int? id, int? firmUserGroupId)
        {
            var list = _accountRepository.GetRelationshipAccountList(id, firmUserGroupId);
            list.RemoveAll(x => x.Name == "Total" || x.Name == "All Accounts");
            return list;
        }

        public string GetCode(string name)
        {
            return _accountRepository.GetUserCode(name);
        }

        public List<List<ManagerComplete>> GetManagers(string code)
        {
            return _managerRepository.GetManagerList(code).GroupBy(x => x.Manager).Select(x => x.ToList()).ToList();
        }

        public List<Silvercrest.Entities.TeamSettings> GetTeamSettings(int? firmUserGroupId)
        {
            return _managerRepository.GetTeamSettings(firmUserGroupId);
        }

        public void UpdateTeamSettings(int? firmUserGroupId, bool onHold, string userName)
        {
            _managerRepository.UpdateTeamSettings(firmUserGroupId, onHold, userName);
        }

        public List<AccountAccessViewModel> GetAccountAccess(int? contactId)
        {
            var accountAccess = _managerRepository.GetAccountAccess(contactId).ToList();
            var accountAccessList = new List<AccountAccessViewModel>();
            foreach (var account in accountAccess)
            {
                var accountAccessViewModel = new AccountAccessViewModel();
                accountAccessViewModel.AccessType = account.AccessType == -1 ? true : false;
                accountAccessViewModel.AccountName = account.AccountName;
                accountAccessViewModel.ManagerCode = account.ManagerCode;
                accountAccessViewModel.ShortName = account.ShortName;
                accountAccessViewModel.AccountId = account.AccountId;
                accountAccessList.Add(accountAccessViewModel);
            }
            return accountAccessList;

        }
        public List<AccountAccessViewModel> GetNonAccountAccess(int? managerContactId, int? contactId)
        {
            var accountNonAccess = _managerRepository.GetNonAccountAccess(managerContactId, contactId).ToList();
            var accountNonAccessList = new List<AccountAccessViewModel>();
            foreach(var account in accountNonAccess)
            {
                var accountAccessViewModel = new AccountAccessViewModel();
                accountAccessViewModel.AccessType = account.AccessType == -1 ? true : false;
                accountAccessViewModel.AccountName = account.AccountName;
                accountAccessViewModel.ManagerCode = account.ManagerCode;
                accountAccessViewModel.ShortName = account.ShortName;
                accountAccessViewModel.AccountId = account.AccountId;
                accountNonAccessList.Add(accountAccessViewModel);
            }
            return accountNonAccessList;
        }

        public int RemoveAccess(int? contactId, string accountIds, string fullname)
        {
            return _managerRepository.RemoveAccess(contactId, accountIds, fullname);
        }
        public int GrantAccess(int? contactId, string accountIds, string fullname)
        {
            return _managerRepository.GrantAccess(contactId, accountIds, fullname);
        }

        public int? GetContactId(string name)
        {
            return _accountRepository.GetContactId(name);
        }

        public string GetContactName(int? contactId)
        {
            return _accountRepository.GetUserName(contactId);
        }


        public AccountAccessInfo FillInInfo(int? contactId)
        {
            return new AccountAccessInfo
            {
                ContactId = contactId,
                Email = _managerRepository.GetEmail(contactId),
                FullName = _contactRepository.GetFullNameByContactId(contactId),
                IsActive = _managerRepository.GetStatus(contactId),
                SendNotifications = _managerRepository.GetSendNotificationsFlag(contactId),
                Post = _managerRepository.GetPostEqtyWriteUpsFlag(contactId),
                ContactCode = _managerRepository.GetContactCode(contactId),
                EconomicCommentary = _managerRepository.GetPostEconomicCommentary(contactId)
            };
        }

        public void UpdateEmail(int? contactId, string email)
        {
            _managerRepository.UpdateEmail(contactId, email); 
        }

        public void UpdateStatus(int? contactId, bool isActive)
        {
            _managerRepository.UpdateStatus(contactId, isActive);
        }

        public void SendEmailNotifications(int? contactId, bool sendNotifications)
        {
            _managerRepository.SendEmailNotifications(contactId, sendNotifications);
        }

        public void PostEqtyWriteUps(int? contactId, bool post)
        {
            _managerRepository.PostEqtyWriteUps(contactId, post);
        }

        public void PostEconomicCommentary(int? contactId, bool economicCommentary)
        {
            _managerRepository.PostEconomicCommentary(contactId, economicCommentary);
        }

        public void UpdateTSEmailNotification(int? firmUserGroupId, bool emailNotification, string userName)
        {
            _managerRepository.UpdateTSEmailNotification(firmUserGroupId, emailNotification, userName);
        }

        public void UpdateTSEquityWriteUps(int? firmUserGroupId, bool equityWriteUps, string userName)
        {
            _managerRepository.UpdateTSEquityWriteUps(firmUserGroupId, equityWriteUps, userName);
        }

        public void UpdateTSEconomicCommentary(int? firmUserGroupId, bool economicCommentary, string userName)
        {
            _managerRepository.UpdateTSEconomicCommentary(firmUserGroupId, economicCommentary, userName);
        }

        public void ChangeTwoFactorAuth(int? contactId, TwoFactorAuth twoFactAuth)
        {
            if (!contactId.HasValue)
            {
                return;
            }
            _contactRepository.SetTwoFactorAuthByConactId(contactId.Value, twoFactAuth);
        }
    }
}
