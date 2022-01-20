using Silvercrest.DataAccess.Model;
using Silvercrest.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silvercrest.Web.Helpers.Authorize
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        private string[] _roles;
        private SLVR_DEVEntities _context;

        public AuthorizeRoles(params UserRole[] roles)
        {
            _context = new SLVR_DEVEntities();
            _roles = roles.Select(r => r.ToString()).ToArray(); 
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            AspNetUser user = null;
            //while(((IObjectContextAdapter)_context).ObjectContext.Connection.State != System.Data.ConnectionState.Closed)
            //{
            //    if(((IObjectContextAdapter)_context).ObjectContext.Connection != null)
            //    ((IObjectContextAdapter)_context).ObjectContext.Connection.Dispose();
            //    _context = new SLVR_DEVEntities();
            //}
            user = _context.AspNetUsers
                .FirstOrDefault(x => x.UserName == httpContext.User.Identity.Name);
            if(user == null)
            {
                return false;
            }
            //Order by Clause Added by Rohan Otherwise Error Where client and Super user was not forwarding to the Super User page
            var role = _context.f_Web_User_Roles()
                .Where(z => z.contact_id == user.contact_id).OrderBy(s=>s.web_role_id)
                .FirstOrDefault();
            var webRole = role.web_role.Replace(" ", String.Empty);
            if (!_roles.Contains(webRole))
            {
                return false;
            }
            return true;
        }
    }
}