using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Silvercrest.Entities.Enums;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Mappers;

namespace Silvercrest.DataAccess
{
    public class ApplicationUserRepository
    {
        private SLVR_DEVEntities _context;
        public ApplicationUserRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public List<UserComplete> GetUsers()
        {
            return UserMapper.MapUsersList(_context.p_Web_Users().ToList());
        }

        public bool CreateUser(string password, string contactEmail, int contactId)
        {
            //var contact = Context.Contacts.Where(x => x.id == contactId).FirstOrDefault();
            //if (contact == null)
            //{
            //    return false;
            //}

            //var user = new Model.AspNetUser();
            //user.is_active = false;
            //user.needs_password_generate = true;
            //user.Email = contactEmail;
            //user.UserName = contactEmail;
            //user.contact_id = contact.id;

            //var store = new UserStore<Model.AspNetUser>(Context);
            //var manager = new UserManager<Model.AspNetUser>(store);
            //try
            //{
            //    manager.Create(user, password);
            //}
            //catch (Exception ex)
            //{
            //    var t = ex.Data.Count;
            //}
            //manager.AddToRole(user.Id, UserRole.Client.ToString());
            //Context.SaveChanges();
            return true;
        }
    }
}