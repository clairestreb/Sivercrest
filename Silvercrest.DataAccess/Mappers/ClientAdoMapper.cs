using Silvercrest.Entities;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class ClientAdoMapper
    {
        public static void MapAccounts(DataRowCollection rows, List<ClientAccount> mappedAccountList)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    var mappedAccount = new Silvercrest.Entities.ClientAccount();
                    mappedAccount.Name = row["display_name"].ToString();
                    mappedAccount.TotalValue = !row.IsNull("total_value") ? (double)row["total_value"] : 0;
                    mappedAccount.PercentOfTotal = !row.IsNull("pct") ? (double)row["pct"] : 0;
                    mappedAccount.ContactId = (int?)row["contact_id"];
                    mappedAccount.IsGroup = (bool?)row["is_group"];
                    mappedAccount.IsClientGroup = (bool?)row["is_client_group"];
                    mappedAccount.SortOrder = (int?)row["sort_order"];
                    mappedAccount.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                    mappedAccount.IsDefault = (bool?)row["is_default"] ?? false;
                    mappedAccount.EntityId = (int?)row["entity_id"];
                    mappedAccount.AccountType = ((bool)row["is_client_group"] == true || (bool)row["is_group"] == true) ? "Groups" : "Accounts";
                    mappedAccountList.Add(mappedAccount);
                }
            }
        }

        public static void MapGroupAccounts(DataRowCollection rows, List<ClientGroupAccount> mappedAccountList)
        {
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    var mappedAccount = new Silvercrest.Entities.ClientGroupAccount();
                    mappedAccount.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
                    mappedAccount.ContactId = (int?)row["contact_id"];
                    mappedAccount.GroupEntityId = (int?)row["group_entity_id"];
                    mappedAccount.GroupIsGroup = (bool?)row["group_is_group"];
                    mappedAccount.GroupIsClientGroup = (bool?)row["group_is_client_group"];
                    mappedAccount.GroupName = row["group_display_name"].ToString();
                    mappedAccount.GroupTotalValue = !row.IsNull("group_total_value") ? (double)row["group_total_value"] : 0;
                    mappedAccount.AccountEntityId = !row.IsNull("account_entity_id") ? (int?)row["account_entity_id"] : null;
                    mappedAccount.AccountIsGroup = !row.IsNull("account_is_group") ? (bool?)row["account_is_group"] : null;
                    mappedAccount.AccountIsClientGroup = !row.IsNull("account_is_client_group") ? (bool?)row["account_is_client_group"]: null;
                    mappedAccount.AccountName = row["account_display_name"].ToString();
                    mappedAccount.AccountTotalValue = !row.IsNull("account_total_value") ? (double)row["account_total_value"] : 0;
                    mappedAccount.PercentOfGroup = !row.IsNull("pct_of_group") ? (double)row["pct_of_group"] : 0;
                    mappedAccount.sortOrder= (int?)row["sort_order"];
                    mappedAccountList.Add(mappedAccount);
                }
            }
        }


        public static void MapCharts(DataRowCollection rows, List<Silvercrest.Entities.PieChart> charts)
        {
            List<Silvercrest.Entities.PieChart> chartsData = new List<Entities.PieChart>();
            if (rows.Count > 0)
            {
                foreach (DataRow r in rows)
                {
                    var chart = new Silvercrest.Entities.PieChart();
                    chart.MarketValue = (double?)r["market_value"];
                    chart.Percent = (double?)r["pct"];
                    chart.LegendName = r["asset_class"].ToString();
                    chartsData.Add(chart);
                }
                chartsData = chartsData.Where(x => x.Percent > 0).OrderByDescending(x => x.Percent).ToList();
            }
            charts.AddRange(chartsData);
        }

        public static void MapViewInfo(DataRowCollection rows, IndexInitViewModel model)
        {
            Info viewInfo = new Info();
            if (rows.Count > 0)
            {
                var row = rows[0];
               /* viewInfo.Date = (DateTime?)row["as_of_date"];
                viewInfo.CustodianAccount = row["custodian_account"].ToString();
                viewInfo.Name = row["display_name"].ToString();
                viewInfo.TotalMarketValue = (double?)row["total_market_value"] ?? 0;
                viewInfo.MarketValue = (double?)row["market_value"] ?? 0;
                viewInfo.UnrealizedGL = (double?)row["unrealized_gl"];*/
                model.Name = row["display_name"].ToString(); 
                model.CustodianAccount =string.IsNullOrEmpty(row["custodian_account"].ToString()) ? "" : "custodian account: " + row["custodian_account"].ToString();
                //model.Date = (row["as_of_date"]).ToString();
                model.Date =  Silvercrest.Utilities.DateConverter.ConvertDateToString((DateTime?)row["as_of_date"]).ToLower();
            }
            else
            {
                model.Date = DateConverter.ConvertDateToString(DateTime.Now);
                model.CustodianAccount = "";
                model.Name = "Name";
            }
        }
    }
}
