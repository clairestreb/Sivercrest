using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Common
{
   public class GetFullName
    {
        private SLVR_DEVEntities1 _context;
        public string GetFullNameByEmail(string userIdentityName)
        {
            var contactId = _context.AspNetUsers.Where(y => y.Email == userIdentityName).Select(y => y.contact_id).FirstOrDefault();
            var fullName = GetFullNameByContactId(contactId);
            return fullName;
        }

        public string GetFullNameByContactId(int? contactId)
        {
            return _context.Contacts
                 .Where(x => x.id == contactId)
                 .Select(x => x.first_name + " " + x.last_name)
                 .FirstOrDefault();
        }
    }
}
