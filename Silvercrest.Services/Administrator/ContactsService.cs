using Silvercrest.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Silvercrest.Entities;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.DataAccess.Model;

namespace Silvercrest.Services
{
    public class ContactsService : IContactsService
    {
        private ContactRepository _contactRepository;
        private QuestionRepository _questionRepository;

        public ContactsService(SLVR_DEVEntities context)
        {
            _contactRepository = new ContactRepository(context);
            _questionRepository = new QuestionRepository(context);
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

        public List<Entities.ContactComplete> GetContactsByEmail()
        {
            var contacts = GetContacts();
            return contacts.ToList();
        }

        public string[] GetContactsEmails(string email)
        {
            return GetContacts().Select(x => x.DisplayName).ToArray();
        }

        public List<SecretQuestion> GetQuestionList()
        {
            return _questionRepository.GetQuestionsList();
        }
    }
}
