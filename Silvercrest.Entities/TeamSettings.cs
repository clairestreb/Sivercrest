using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class TeamSettings
    {
        public string Code { get; set; }
        public string ManagerName { get; set; }
        [DataNames("firm_user_group_id")]
        public int firm_user_group_id { get; set; }
        [DataNames("upload_on_hold")]
        public bool? StatementUploadOnHold { get; set; }
        [DataNames("email_notifications")]
        public bool? email_notifications { get; set; }
        [DataNames("receives_equity_writeups")]
        public bool? receives_equity_writeups { get; set; }
        [DataNames("receives_econ_commentary")]
        public bool? receives_econ_commentary { get; set; }
    }
}
