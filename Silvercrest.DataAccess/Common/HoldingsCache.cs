using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Common
{
    public sealed class HoldingsCache
    {
        private static readonly HoldingsCache instance = new HoldingsCache();
        private static DateTime as_of_date = new DateTime(1970, 1, 1);
        private static Dictionary<int, List<Tax_Lot>> Tax_Lots = new Dictionary<int, List<Tax_Lot>>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HoldingsCache()
        {
        }

        private HoldingsCache()
        {
        }

        public static HoldingsCache Instance
        {
            get
            {
                return instance;
            }
        }

        public static void setAsOfDate(DateTime as_of_date)
        {
            as_of_date = DateTime.SpecifyKind(as_of_date, DateTimeKind.Utc);
        }

        public static DateTime getAsOfDate()
        {
            return as_of_date;
        }

        private static void AddTaxLot(Tax_Lot lot)
        {
            List<Tax_Lot> lots;
            if (Tax_Lots.ContainsKey(lot.account_id))
            {
                lots = Tax_Lots[lot.account_id];
            }
            else
            {
                lots = new List<Tax_Lot>();
            }

            lots.Add(lot);
        }






    }
}


