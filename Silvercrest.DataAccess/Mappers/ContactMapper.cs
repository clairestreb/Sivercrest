using Silvercrest.DataAccess.Model;
using Silvercrest.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class ContactMapper
    {
        public static List<Silvercrest.Entities.ContactComplete> MapContactsList(IList<p_Web_Contact_List_Result> list)
        {
            var mappedContactList = new List<Silvercrest.Entities.ContactComplete>();
            foreach (var contact in list)
            {
                var mappedContact = new Silvercrest.Entities.ContactComplete();
                mappedContact.Id = contact.contact_id;
                mappedContact.DisplayName = contact.display_name;
                mappedContactList.Add(mappedContact);
            }
            return mappedContactList;
        }

        public static List<Silvercrest.Entities.ManagerContactComplete> MapContactsList(IList<p_Web_Relationship_Contacts_Result> list)
        {
            var mappedContactList = new List<Silvercrest.Entities.ManagerContactComplete>();
            foreach (var contact in list)
            {
                var mappedContact = new Silvercrest.Entities.ManagerContactComplete();
                mappedContact.ContactId = contact.contact_id;
                mappedContact.DisplayName = contact.display_name;
                mappedContact.Email = contact.email_address;
                mappedContact.IsWebUser = contact.is_web_user;
                mappedContact.Relationship = contact.relationship;
                mappedContactList.Add(mappedContact);
            }
            return mappedContactList;
        }

        public static UserRole MapUserRole(f_Web_User_Roles_Result userRole)
        {
            if(userRole.web_role == "Client")
            {
                return UserRole.Client;
            }
            if (userRole.web_role == "Super User")
            {
                return UserRole.SuperUser;
            }
            if (userRole.web_role == "Administrator")
            {
                return UserRole.Administrator;
            }
            if (userRole.web_role == "Team Member")
            {
                return UserRole.TeamMember;
            }
            if (userRole.web_role == "Secondary Client")
            {
                return UserRole.SecondaryClient;
            }
            return UserRole.None;
        }
    }
}
