using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class TransactionsRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();

        public DataSet TransactionsInit(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Transaction_Home", contactId, entity_id, is_group, is_client_group);
            return data;
        }

        public DataSet TransactionsPurchases(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Purchases", contactId, entity_id, is_group, is_client_group, startDate, endDate);
            return data;
        }

        public DataSet TransactionsSales(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Sales", contactId, entity_id, is_group, is_client_group, startDate, endDate);
            return data;
        }

        public DataSet TransactionsContributions(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Contributions", contactId, entity_id, is_group, is_client_group, startDate, endDate);
            return data;
        }

    }
}
