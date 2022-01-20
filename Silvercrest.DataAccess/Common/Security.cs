using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Common
{
    public class Security
    {
        public static Dictionary<int, Security> Security_Master = new Dictionary<int, Security>();

        public int security_id { get; set; }
        public string symbol { get; set; }
        public string ticker { get; set; }
        public string security_name { get; set; }
        public string bond_description { get; set; }
        public string asset_class { get; set; }
        public string security_type { get; set; }
        public string sector { get; set; }
        public int security_type_sort { get; set; }
        public int sector_sort { get; set; }

        public Security(int security_id, string symbol, string ticker, string security_name, string bond_description, string asset_class, string security_type, string sector, int security_type_sort, int sector_sort)
        {
            this.security_id = security_id;
            this.symbol = symbol;
            this.ticker = ticker;
            this.security_name = security_name;
            this.bond_description = bond_description;
            this.asset_class = asset_class;
            this.security_type = security_type;
            this.sector = sector;
            this.security_type_sort = security_type_sort;
            this.sector_sort = sector_sort;

            Security_Master[security_id] = this;
        }
    }
}
