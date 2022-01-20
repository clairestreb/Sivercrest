using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Interfaces.CommonServices;
using Silvercrest.DataAccess.Model;

namespace Silvercrest.Services.CommonServices
{
    public class ClientCommonService : IClientCommon
    {       
        private SLVR_DEVEntities1 _context;

        public ClientCommonService(SLVR_DEVEntities1 context)
        {
            _context = context;            
        }

        public string GetFullName(string userIdentityName)
        {
            var contactId = _context.AspNetUsers
              .Where(y => y.Email == userIdentityName)
              .Select(y => y.contact_id)
              .FirstOrDefault();

            return _context.Contacts
                .Where(x => x.id == contactId)
                .Select(x => x.first_name + " " + x.last_name)
                .FirstOrDefault();
        }
    }
}
