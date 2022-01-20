using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using Silvercrest.ViewModels.TeamMember;
using System.Collections;
using System.Collections.Generic;

namespace Silvercrest.Interfaces
{
    public interface IManagerService
    {
        List<ManagerComplete> GetFamilies(string code);
        List<ManagerContactComplete> GetContactsByFamily(int? id);
        List<Account> GetAccountsByFamily(int? id, int? firmUserGroupId);
        string GetCode(string name);
        List<List<ManagerComplete>> GetManagers(string code);
        List<AccountAccessViewModel> GetAccountAccess(int? contactId);
        AccountAccessInfo FillInInfo(int? contactId);
        List<AccountAccessViewModel> GetNonAccountAccess(int? managerContactId,int? contactId);
        int? GetContactId(string name);
        int RemoveAccess(int? contactId, string accountIds, string fullname);
        int GrantAccess(int? contactId, string accountIds, string fullname);
        string GetContactName(int? contactId);
        void Fill(Hashtable hash);
        void UpdateEmail(int? contactId, string email);
        void UpdateStatus(int? contactId, bool isActive);
        void SendEmailNotifications(int? contactId, bool sendNotifications);
        void PostEqtyWriteUps(int? contactId, bool post);
        //Entities.TeamSettings GetTeamSettings(int? firmUserGroupId);
        List<Entities.TeamSettings> GetTeamSettings(int? firmUserGroupId);
        void UpdateTeamSettings(int? firmUserGroupId, bool onHold, string userName);
        void PostEconomicCommentary(int? contactId, bool economicCommentary);
        void UpdateTSEmailNotification(int? firmUserGroupId, bool emailNotification, string userName);
        void UpdateTSEquityWriteUps(int? firmUserGroupId, bool equityWriteUps, string userName);
        void UpdateTSEconomicCommentary(int? firmUserGroupId, bool economicCommentary, string userName);
        void ChangeTwoFactorAuth(int? contactId, TwoFactorAuth state);
    }
}
