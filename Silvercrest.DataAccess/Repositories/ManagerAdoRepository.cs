using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class ManagerAdoRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();

        public DataSet ManagerInit(int? familyGroupId, int? firmUserGroupId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_WebMulti_Relationship_Details", familyGroupId, firmUserGroupId);
            return data;
        }

        public DataSet ContactListInit(int? contactId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Team_Contacts", contactId);
            return data;
        }
    }
}
