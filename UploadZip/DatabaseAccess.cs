using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadZip
{
    class DatabaseAccess
    {
        public static void UpdateTable(int alertId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Web_Email_Alerts SET is_active = 0, update_date = GETUTCDATE() WHERE id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@id", alertId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataSet GetNextEmailAlerts()
        {
            int? id = null;
            int? fileId = null;
            string recepient = null;
            string displayName = null;
            string documentDate = null;
            string documentType = null;
            string clientName = null;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("p_Web_Get_Next_Email_Alert", connection))
                {
                    cmd.CommandTimeout = 120;
                    AddIntParameter(cmd, id, "id");
                    AddStringParameter(cmd, recepient, "recipient");
                    AddIntParameter(cmd, fileId, "file_id");
                    AddStringParameter(cmd, displayName, "display_name");
                    AddStringParameter(cmd, documentDate, "document_date");
                    AddStringParameter(cmd, documentType, "document_type");
                    AddStringParameter(cmd, clientName, "client_name");
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet data = new DataSet();
                    adapter.Fill(data);
                    connection.Close();
                    connection.Dispose();
                    return data;
                }
            }
        }

        public static void GenerateEmailAlerts()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.p_Web_Generate_Email_Alerts", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;          
                    connection.Open();
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private static void AddIntParameter(SqlCommand cmd, int? parameter, string parameterName)
        {
            if (parameter.HasValue)
            {
                cmd.Parameters.Add(parameterName, SqlDbType.Int).Value = parameter;
            }
            if (!parameter.HasValue)
            {
                cmd.Parameters.Add(parameterName, SqlDbType.Int).Value = DBNull.Value;
            }
        }

        private static void AddBoolParameter(SqlCommand cmd, bool? parameter, string parameterName)
        {
            if (parameter.HasValue)
            {
                cmd.Parameters.Add(parameterName, SqlDbType.Bit).Value = parameter;
            }
            if (!parameter.HasValue)
            {
                cmd.Parameters.Add(parameterName, SqlDbType.Bit).Value = DBNull.Value;
            }
        }

        private static void AddStringParameter(SqlCommand cmd, string parameter, string parameterName)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                cmd.Parameters.Add(parameterName, SqlDbType.NVarChar).Value = parameter;
            }
            if (string.IsNullOrEmpty(parameter))
            {
                cmd.Parameters.Add(parameterName, SqlDbType.NVarChar).Value = DBNull.Value;
            }
        }
    }
}
