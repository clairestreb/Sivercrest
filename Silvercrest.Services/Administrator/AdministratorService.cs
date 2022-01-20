using Silvercrest.DataAccess;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities.Enums;
using Silvercrest.ViewModels.Administrator;

namespace Silvercrest.Services
{
    public class AdministratorService : IAdministratorService
    {
        private ApplicationUserRepository _userRepository;
        private QuestionRepository _questionRepository;
        private ContactRepository _contactRepository;
        private ManagerRepository _managerRepository;

        public AdministratorService(SLVR_DEVEntities context)
        {
            _userRepository = new ApplicationUserRepository(context);
            _questionRepository = new QuestionRepository(context);
            _contactRepository = new ContactRepository(context);
            _managerRepository = new ManagerRepository(context);
        }

        public int GetTwoFactorAuthById(string Id)
        {
            return _contactRepository.GetTwoFactorAuthById(Id);
        }

        public void SetTwoFactorAuth(string Id, TwoFactorAuth twoFactorAuth)
        {
            _contactRepository.SetTwoFactorAuth(Id, twoFactorAuth);
        }

        public ContactComplete GetContactForNewUser(int contactId)
        {
            var contact = _contactRepository.GetContactList().Where(x => x.Id == contactId).FirstOrDefault();
            return contact;
        }

        public bool CreateUser(string contactEmail, int contactId)
        {
            var password = PasswordGenerator.GetPass();
            var successCreating = _userRepository.CreateUser(password, contactEmail, contactId);
            if (!successCreating)
            {
                return false;
            }
            return true;
        }

        public List<SecretQuestion> GetQuestionList()
        {
            return _questionRepository.GetQuestionsList();
        }

        public List<Entities.ContactComplete> GetContacts()
        {
            return _contactRepository.GetContactList();
        }

        public List<Entities.ContactComplete> GetContactsByEmailPrefix()
        {
            var contacts = _contactRepository.GetContactList();
            return contacts;
        }

        public UserRole GetUserRole(int contactId)
        {
            var role = _contactRepository.GetUserRole(contactId);
            return role;
        }

        public List<UserRole> GetAllUserRoles(int contactId)
        {
            var roles = _contactRepository.GetAllUserRoles(contactId);
            return roles;
        }

        public List<UserComplete> GetUsers()
        {
            var userList = _userRepository.GetUsers();
            return userList;
        }

        public string GetFullName(string userIdentityName)
        {
            return _contactRepository.GetFullNameByEmail(userIdentityName);
        }

        public ContactSettingsInfo FillInInfo(int contactId)
        {
            return new ContactSettingsInfo
            {
                ContactId = contactId,
                Email = _managerRepository.GetEmail(contactId),
                FullName = _contactRepository.GetFullNameByContactId(contactId),
                IsActive = _managerRepository.GetStatus(contactId),
                ContactCode = _managerRepository.GetContactCode(contactId)
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
    }
}