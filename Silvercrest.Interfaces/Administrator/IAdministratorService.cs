using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using Silvercrest.ViewModels.Administrator;
using System.Collections.Generic;

namespace Silvercrest.Interfaces
{
    public interface IAdministratorService
    {
        int GetTwoFactorAuthById(string Id);
        void SetTwoFactorAuth(string Id, TwoFactorAuth twoFactorAuth);
        bool CreateUser(string contactEmail, int contactId);
        List<SecretQuestion> GetQuestionList();
        List<ContactComplete> GetContacts();
        List<ContactComplete> GetContactsByEmailPrefix();
        ContactComplete GetContactForNewUser(int contactId);
        List<UserComplete> GetUsers();
        UserRole GetUserRole(int contactId);
        string GetFullName(string userIdentityName);
        List<UserRole> GetAllUserRoles(int contactId);
        ContactSettingsInfo FillInInfo(int contactId);
        void UpdateEmail(int? contactId, string email);
        void UpdateStatus(int? contactId, bool isActive);
    }
}
