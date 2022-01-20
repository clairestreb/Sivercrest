using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Common
{
    public class Account
    {
        public static Dictionary<int, Account> Account_Master = new Dictionary<int, Account>();

        public int account_id { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }

        public Account(int account_id, string account_code, string account_name)
        {
            this.account_id = account_id;
            this.account_code = account_code;
            this.account_name = account_name;

            Account_Master[account_id] = this;
        }

    }
}
