using Silvercrest.Entities;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client.Holdings;
using Silvercrest.ViewModels.Client.Holdings.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class HoldingsRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();

        public DataSet FillHoldingsView(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Allocation_By_AssetClass", contactId, entity_id, is_group, is_client_group);
            return data;
        }

        public DataSet FillCashView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Cash", contactId, entityId, isGroup, isClientGroup);
            return data;
        }

        public DataSet FillAssetClassView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup, string assetClass)
        {
            //            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_FixedIncome", contactId, entityId, isGroup, isClientGroup);
            DataSet data = _helper.ExecuteRollupStoredProcedure(connectionString, "p_WebMulti_Position_Rollups", contactId, entityId, isGroup, isClientGroup, assetClass);
            return data;
        }


        public DataSet FillIncomeView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup, string assetClass)
        {
//            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_FixedIncome", contactId, entityId, isGroup, isClientGroup);
            DataSet data = _helper.ExecuteRollupStoredProcedure(connectionString, "p_WebMulti_Position_Rollups", contactId, entityId, isGroup, isClientGroup, assetClass);
            return data;
        }

        public DataSet FillOtherAssetsView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_OtherAssets", contactId, entityId, isGroup, isClientGroup);
            return data;
        }
        public DataSet FillEquitiesView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup, string assetClass)
        {
            //            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Equities", contactId, entityId, isGroup, isClientGroup);
            DataSet data = _helper.ExecuteRollupStoredProcedure(connectionString, "p_WebMulti_Position_Rollups", contactId, entityId, isGroup, isClientGroup, assetClass);
            return data;
        }
        public DataSet FillModalView(int? contactId, int? entityId, bool? isGroup, bool? isClientGroup, int? securityId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Tax_Lots", contactId, entityId, isGroup, isClientGroup, securityId);
            return data;
        }
    }
}
