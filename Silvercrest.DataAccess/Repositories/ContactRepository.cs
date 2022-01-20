using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities.Enums;
using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Silvercrest.DataAccess.Repositories
{
    public class ContactRepository
    {
        private SLVR_DEVEntities _context;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public ContactRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public int GetTwoFactorAuthById(string Id)
        {
            var aspNetUser = _context.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();

            if (aspNetUser == null)
            {
                throw new ArgumentNullException("user");
            }

            return aspNetUser.TwoFactorAuth;
        }

        public void SetTwoFactorAuthByConactId(int contactId, TwoFactorAuth twoFactorAuth)
        {
            var aspNetUser = _context.AspNetUsers.Where(x => x.contact_id == contactId).FirstOrDefault();

            if (aspNetUser == null)
            {
                throw new ArgumentNullException("user");
            }

            aspNetUser.TwoFactorAuth = (int)twoFactorAuth;
            _context.SaveChanges();
        }

        public void SetTwoFactorAuth(string Id, TwoFactorAuth twoFactorAuth)
        {
            var aspNetUser = _context.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();

            if (aspNetUser == null)
            {
                throw new ArgumentNullException("user");
            }

            aspNetUser.TwoFactorAuth = (int)twoFactorAuth;
            _context.SaveChanges();
        }

        public List<Silvercrest.Entities.ContactComplete> GetContactList()
        {
            try
            {
                IList<p_Web_Contact_List_Result> contactList = _context.p_Web_Contact_List().ToList<p_Web_Contact_List_Result>();
                var mappedList = ContactMapper.MapContactsList(contactList);
                return mappedList;
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
            }
            return null;
        }

        public List<Silvercrest.Entities.ManagerContactComplete> GetContactList(int? familyId)
        {
            IList<p_Web_Relationship_Contacts_Result> contactList = _context.p_Web_Relationship_Contacts(familyId).ToList<p_Web_Relationship_Contacts_Result>();
            var mappedList = ContactMapper.MapContactsList(contactList);
            return mappedList;
        }

        public UserRole GetUserRole(int contactId)
        {
            try
            {
                IList<f_Web_User_Roles_Result> roleList = _context.f_Web_User_Roles().ToList<f_Web_User_Roles_Result>();
                var role = roleList.Where(x => x.contact_id == contactId).FirstOrDefault();
                var mappedRole = ContactMapper.MapUserRole(role);
                return mappedRole;
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
            }
            return UserRole.None;
        }

        public List<UserRole> GetAllUserRoles(int contactId)
        {
            try
            {
                IList<f_Web_User_Roles_Result> roleList = _context.f_Web_User_Roles().Where(x => x.contact_id == contactId).OrderBy(s => s.web_role_id).ToList<f_Web_User_Roles_Result>();
                var mappedRoles = new List<UserRole>();

                foreach (var role in roleList)
                {
                    var mappedRole = ContactMapper.MapUserRole(role);
                    mappedRoles.Add(mappedRole);
                }

                //INSTEAD OF SENDING BACK ALL ROLES JUST SEND MOST RELEVANT
                //                var mappedRole = ContactMapper.MapUserRole(roleList[0]);
                //                mappedRoles.Add(mappedRole);

                return mappedRoles;
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
            }
            return new List<UserRole> { UserRole.None };
        }

        public string GetFullNameByContactId(int? contactId)
        {
            string fullName = "";
            try
            {
                fullName = _context.Contacts.Where(x => x.id == contactId).Select(x => x.first_name + " " + x.last_name).FirstOrDefault();
                return fullName;
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
            }
            return fullName;
        }

        public string GetFullNameByEmail(string userIdentityName)
        {
            var contactId = _context.AspNetUsers.Where(y => y.Email == userIdentityName).Select(y => y.contact_id).FirstOrDefault();
            var fullName = GetFullNameByContactId(contactId);
            return fullName;
        }
    }
}
