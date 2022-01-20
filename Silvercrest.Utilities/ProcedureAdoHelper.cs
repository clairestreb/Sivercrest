using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Utilities
{
    public class ProcedureAdoHelper
    {
        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");
                    AddIntParameter(cmd, entity_id, "entity_id_inp");
                    AddBoolParameter(cmd, is_group, "is_group_inp");
                    AddBoolParameter(cmd, is_client_group, "is_client_group_inp");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, int? entity_id, bool? is_group, bool? is_client_group, int? securityId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");
                    AddIntParameter(cmd, entity_id, "entity_id_inp");
                    AddBoolParameter(cmd, is_group, "is_group_inp");
                    AddBoolParameter(cmd, is_client_group, "is_client_group_inp");
                    AddIntParameter(cmd, securityId, "security_id_inp");

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


        public DataSet ExecuteRollupStoredProcedure(string connectionString, string procedureName, int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string assetClass)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");
                    AddIntParameter(cmd, entity_id, "entity_id_inp");
                    AddBoolParameter(cmd, is_group, "is_group_inp");
                    AddBoolParameter(cmd, is_client_group, "is_client_group_inp");
                    AddStringParameter(cmd, assetClass, "asset_class_inp");

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


        public DataSet ExecuteHomeStoredProcedure(string connectionString, string procedureName, int? contactId, int? entity_id, bool? is_client_group, int?  sub_entity_id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");
                    AddIntParameter(cmd, entity_id, "group_entity_id_inp");
                    AddBoolParameter(cmd, is_client_group, "is_client_group_inp");
                    AddIntParameter(cmd, sub_entity_id, "entity_id_inp");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? familyGroupId, int? firmUserGroupId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, familyGroupId, "family_group_id_inp");
                    AddIntParameter(cmd, firmUserGroupId, "firm_user_group_id_inp");

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

        public DataSet ExecuteStoredProcedureGetTeamSettings(string connectionString, string procedureName, int? firmUserGroupId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    AddIntParameter(cmd, firmUserGroupId, "firm_user_group_id_inp");

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
        
        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");
                    AddIntParameter(cmd, entity_id, "entity_id_inp");
                    AddBoolParameter(cmd, is_group, "is_group_inp");
                    AddBoolParameter(cmd, is_client_group, "is_client_group_inp");
                    AddStringParameter(cmd, startDate, "startDate_str");
                    AddStringParameter(cmd, endDate, "endDate_str");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "contact_id_inp");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, string fileId, bool? isFavorite, string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id");
                    AddStringParameter(cmd, fileId, "@file_id");
                    AddBoolParameter(cmd, isFavorite, "@is_favorite");
                    AddStringParameter(cmd, userName, "@username");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, string fileId, string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddStringParameter(cmd, fileId, "@file_id");
                    AddStringParameter(cmd, userName, "@username");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id_inp");
                    AddStringParameter(cmd, email, "@username");

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

        public DataSet ExecuteStoredProcedure(string connectionString, string procedureName, int? contactId, bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id_inp");
                    AddBoolParameter(cmd, isActive, "@is_active");

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

        public DataSet ExecuteNotifiactionsStoredProcedure(string connectionString, string procedureName, int? contactId, bool sendNotifications)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id_inp");
                    AddBoolParameter(cmd, sendNotifications, "@send_notifications");

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

        public DataSet ExecutePostStoredProcedure(string connectionString, string procedureName, int? contactId, bool post)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id_inp");
                    AddBoolParameter(cmd, post, "@post");

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

        public DataSet ExecuteUPUCFTSStoredProcedure(string connectionString, string procedureName, int? contactId, int firmUserGroupId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, contactId, "@contact_id_inp");
                    AddIntParameter(cmd, firmUserGroupId, "@firm_user_group_id_inp");

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
        
        private void AddIntParameter(SqlCommand cmd, int? parameter, string parameterName)
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

        private void AddBoolParameter(SqlCommand cmd, bool? parameter, string parameterName)
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

        private void AddStringParameter(SqlCommand cmd, string parameter, string parameterName)
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

        public DataSet ExecuteUpdateTSStoredProcedure(string connectionString, string procedureName, int? firmUserGroupId, int updateParameterValue, string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddIntParameter(cmd, firmUserGroupId, "@firm_user_group_id_inp");
                    AddIntParameter(cmd, updateParameterValue, "@update_parameter_value");
                    AddStringParameter(cmd, userName, "@username");

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
    }
}
