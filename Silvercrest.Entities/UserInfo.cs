using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class UserInfo
    {
        public int? ContactId { get; set; }
        public int? EntityId { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsClientGroup { get; set; }
        public string FullName { get; set; }
        public int? SubEntityId { get; set; }

        public UserInfo() { }
        public UserInfo(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            ContactId = contactId;
            EntityId = entity_id;
            IsGroup = is_group;
            IsClientGroup = is_client_group;
        }
    }
}
