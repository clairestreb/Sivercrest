using Silvercrest.Entities;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class ClientRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();

        public DataSet FillHomeView(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, int? sub_entity_id)
        {
            DataSet data = _helper.ExecuteHomeStoredProcedure(connectionString, "p_WebMulti_New_Client_Home", contactId, entity_id, is_client_group, sub_entity_id);
            return data;
        }
        public DataSet ClientGroups(int? contactId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "dbo.p_WebMulti_Client_Groups", contactId);
            return data;
        }

        public DataSet ContactEntities(int? contactId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "dbo.p_Web_contact_entities", contactId);
            return data;
        }



        //public async Task<Info> GetInfo(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, int? type = null)
        //{
        //        Stopwatch stop = new Stopwatch();
        //        stop.Start();
        //        string typeStr = "";
        //        if (type == 1) typeStr = "ca";
        //        if (type == 2) typeStr = "fi";
        //        if (type == 3) typeStr = "eq";
        //        if (type == 0 && type == null) typeStr = null;
        //        var mappedItem = new Info();
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("p_Web_Allocation_By_Asset_Class", connection))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                if (contactId.HasValue)
        //                {
        //                    cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = contactId;
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = DBNull.Value;
        //                }
        //                if (entity_id.HasValue)
        //                {
        //                    cmd.Parameters.Add("entity_id_inp", SqlDbType.Int).Value = entity_id;
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("entity_id_inp", SqlDbType.Int).Value = DBNull.Value;
        //                }
        //                if (is_group.HasValue)
        //                {
        //                    cmd.Parameters.Add("is_group_inp", SqlDbType.Bit).Value = is_group;
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("is_group_inp", SqlDbType.Bit).Value = DBNull.Value;
        //                }
        //                if (is_client_group.HasValue)
        //                {
        //                    cmd.Parameters.Add("is_client_group_inp", SqlDbType.Bit).Value = is_client_group;
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("is_client_group_inp", SqlDbType.Bit).Value = DBNull.Value;
        //                }
        //                cmd.Parameters.Add("asset_class_inp", SqlDbType.NVarChar).Value = typeStr;
        //                await connection.OpenAsync();
        //                Stopwatch timer = new Stopwatch();
        //                timer.Start();

        //                SqlDataReader info = await cmd.ExecuteReaderAsync();

        //                DataTable data = new DataTable();
        //                data.Load(info);
        //                connection.Close();
        //                connection.Dispose();
        //                timer.Stop();
        //                var tme = timer.Elapsed;
        //                var row = data.Rows[0];
        //                mappedItem.Date = (DateTime?)row["as_of_date"];
        //                mappedItem.CustodianAccount = row["custodian_account"].ToString();
        //                mappedItem.Name = row["display_name"].ToString();
        //                mappedItem.TotalMarketValue = (double?)row["total_market_value"] ?? 0;
        //                mappedItem.MarketValue = (double?)row["market_value"] ?? 0;
        //                mappedItem.UnrealizedGL = (double?)row["unrealized_gl"];
        //            }
        //        }
        //        stop.Stop();
        //        var time = stop.Elapsed;
        //        return mappedItem;
        //}

        //public async Task<List<ClientAccount>> GetAccountAndGroupsList(int? contactId)
        //{
        //        var mappedAccountList = new List<Silvercrest.Entities.ClientAccount>();
        //        using (SqlConnection connection = new SqlConnection(connectionString))// "Server=v3iinihz9y.database.windows.net;Database=SLV_UAT;User ID=scguser1;Password=User2017;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("p_Web_Contact_Entities", connection))
        //            {

        //                cmd.CommandType = CommandType.StoredProcedure;
        //                if (contactId.HasValue)
        //                {
        //                    cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = contactId;
        //                }
        //                else
        //                {
        //                    cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = DBNull.Value;
        //                }
        //            Stopwatch stop = new Stopwatch();
        //            stop.Start();
        //            await connection.OpenAsync();
        //            stop.Stop();
        //            var time = stop.Elapsed;

        //            SqlDataReader info = await cmd.ExecuteReaderAsync();

        //                DataTable data = new DataTable();
        //                data.Load(info);
        //                connection.Close();
        //                connection.Dispose();

        //                foreach (DataRow row in data.Rows)
        //                {
        //                    var mappedAccount = new Silvercrest.Entities.ClientAccount();
        //                    mappedAccount.Name = row["display_name"].ToString();
        //                    mappedAccount.TotalValue = (double?)row["total_value"] ?? 0;
        //                    mappedAccount.PercentOfTotal = (double?)row["pct"] ?? 0;
        //                    mappedAccount.ContactId = (int?)row["contact_id"];
        //                    mappedAccount.IsGroup = (bool?)row["is_group"];
        //                    mappedAccount.IsClientGroup = (bool?)row["is_client_group"];
        //                    mappedAccount.SortOrder = (int?)row["sort_order"];
        //                    mappedAccount.Date = (DateTime?)row["as_of_date"] ?? DateTime.Now;
        //                    mappedAccount.IsDefault = (bool?)row["is_default"] ?? false;
        //                    mappedAccount.EntityId = (int?)row["entity_id"];
        //                    mappedAccount.AccountType = ((bool)row["is_client_group"] == true || (bool)row["is_group"] == true) ? "Groups" : "Accounts";
        //                    mappedAccountList.Add(mappedAccount);
        //                }

        //            }
        //        }
        //        return mappedAccountList;

        //}

        //public List<PieChart> GetChartAssetClass(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        //{
        //    Stopwatch stop = new Stopwatch();
        //    stop.Start();
        //    var mappedItems = new List<PieChart>();
        //    using (SqlConnection connection = new SqlConnection(connectionString))// "Server=v3iinihz9y.database.windows.net;Database=SLV_UAT;User ID=scguser2;Password=User2017;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("p_Web_Allocation_By_Asset_Class", connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            if (contactId.HasValue)
        //            {
        //                cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = contactId;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("contact_id_inp", SqlDbType.Int).Value = DBNull.Value;
        //            }
        //            if (entity_id.HasValue)
        //            {
        //                cmd.Parameters.Add("entity_id_inp", SqlDbType.Int).Value = entity_id;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("entity_id_inp", SqlDbType.Int).Value = DBNull.Value;
        //            }
        //            if (is_group.HasValue)
        //            {
        //                cmd.Parameters.Add("is_group_inp", SqlDbType.Bit).Value = is_group;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("is_group_inp", SqlDbType.Bit).Value = DBNull.Value;
        //            }
        //            if (is_client_group.HasValue)
        //            {
        //                cmd.Parameters.Add("is_client_group_inp", SqlDbType.Bit).Value = is_client_group;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("is_client_group_inp", SqlDbType.Bit).Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add("asset_class_inp", SqlDbType.NVarChar).Value = DBNull.Value;
        //            connection.Open();
        //            Stopwatch timer = new Stopwatch();
        //            timer.Start();

        //            SqlDataReader info = cmd.ExecuteReader();

        //            DataTable data = new DataTable();
        //            data.Load(info);
        //            connection.Close();
        //            connection.Dispose();
        //            timer.Stop();
        //            var tme = timer.Elapsed;
        //            foreach (DataRow row in data.Rows)
        //            {
        //                var chart = new PieChart();
        //                chart.MarketValue = (double?)row["market_value"];
        //                chart.Percent = (double?)row["pct"];
        //                chart.LegendName = row["asset_class"].ToString();
        //                mappedItems.Add(chart);
        //            }
        //        }
        //    }
        //    stop.Stop();
        //    var time = stop.Elapsed;
        //    return mappedItems.OrderByDescending(x=>x.Percent).ToList();
        //}




    }
}
