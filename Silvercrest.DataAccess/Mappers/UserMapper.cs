using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class UserMapper
    {
        public static List<Silvercrest.Entities.UserComplete> MapUsersList(IList<p_Web_Users_Result> list)
        {
            var mappedUsersList = new List<Silvercrest.Entities.UserComplete>();
            foreach (var account in list)
            {
                var mappedUser = new Silvercrest.Entities.UserComplete();
                mappedUser.Id = account.Id;
                mappedUser.ContactId = account.contact_id;
                mappedUser.ContactCode = account.contact_code;
                mappedUser.Email = account.Email;
                mappedUser.UserName = account.UserName;
                mappedUser.IsActive = account.is_active;
                mappedUser.FullName = account.display_name;
                mappedUsersList.Add(mappedUser);
            }
            return mappedUsersList;
        }
    }
}
