using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Silvercrest.DataAccess.Repositories
{
    public class ManagerRepository
    {
        private SLVR_DEVEntities _context;
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();
        public ManagerRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public List<Silvercrest.Entities.ManagerComplete> GetManagerList(string contactId)
        {
            IList<p_Web_Relationships_Result> managerList = _context.p_Web_Relationships(contactId).ToList<p_Web_Relationships_Result>();
            var mappedList = ManagerMapper.MapManagersList(managerList);
            return mappedList;
        }

        public List<Silvercrest.Entities.AccountAccess> GetAccountAccess(int? contactId)
        {
            IList<p_Web_Current_Account_Access_Result> accountList = _context.p_Web_Current_Account_Access(contactId).ToList();
            var mappedList = ManagerMapper.MapAccountAccessList(accountList);
            return mappedList;
        }

        public List<Silvercrest.Entities.TeamSettings> GetTeamSettings(int? firmUserGroupId)
        {
            var firmUserGroupAttributes = _context.f_Web_Firm_User_Group_Attributes(firmUserGroupId).ToList();
            var mappedObject = ManagerMapper.MapFirmUserGroupAttributesList(firmUserGroupAttributes);

            DataSet data = _helper.ExecuteStoredProcedureGetTeamSettings(connectionString, "p_Web_Get_Team_Settings", firmUserGroupId);
            DataNamesMapper<TeamSettings> mapper = new DataNamesMapper<TeamSettings>();
            List<TeamSettings> settings = mapper.Map(data.Tables[0]).ToList();

            if (mappedObject.Count > 0 && settings.Count > 0)
            {
                for (int i = 0; i < mappedObject.Count; i++)
                {
                    settings[i].Code = mappedObject[i].Code;
                    settings[i].ManagerName = mappedObject[i].ManagerName;
                }
                return settings;
            }

            return mappedObject;
        }

        public void UpdateTeamSettings(int? firmUserGroupId, bool onHold, string userName)
        {
            _context.p_Web_Update_FUG_Statement_Upload(firmUserGroupId, onHold ? 1 : 0, userName);
        }

        public List<Silvercrest.Entities.AccountAccess> GetNonAccountAccess(int? managerContactId, int? contactId)
        {
            var param1 = _context.Contacts.Where(x => x.id == managerContactId).Select(x => x.code).FirstOrDefault();
            var nonAccessAccountsList = _context.p_Web_Non_Access_Accounts(param1, contactId).ToList();
            var mappedList = ManagerMapper.MapNonAccountAccessList(nonAccessAccountsList);
            return mappedList;
        }

        public int RemoveAccess(int? contactId, string accountIds, string fullname)
        {
            return _context.p_Web_Remove_Account_Access(contactId, accountIds, fullname);
        }

        public int GrantAccess(int? contactId, string accountIds, string fullname)
        {
            return _context.p_Web_Grant_Account_Access(contactId, accountIds, fullname);
        }

        public string GetEmail(int? contactId)
        {
            return _context.AspNetUsers
                .Where(x => x.contact_id == contactId)
                .Select(x => x.Email)
                .FirstOrDefault();
        }

        public bool GetStatus(int? contactId)
        {
            return _context.AspNetUsers
                .Where(x => x.contact_id == contactId)
                .Select(x => x.is_active)
                .FirstOrDefault();
        }

        public bool GetSendNotificationsFlag(int? contactId)
        {
            return _context.AspNetUsers
                .Where(x => x.contact_id == contactId)
                .Select(x => x.gets_email_notifications)
                .FirstOrDefault();
        }

        public bool GetPostEqtyWriteUpsFlag(int? contactId)
        {
            return _context.AspNetUsers
                .Where(x => x.contact_id == contactId)
                .Select(x => x.receives_equity_writeups)
                .FirstOrDefault();
        }

        public bool GetPostEconomicCommentary(int? contactId)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Post_Get_EconomicCommentary", contactId);
            DataNamesMapper<UserViewModel> mapper = new DataNamesMapper<UserViewModel>();
            List<UserViewModel> persons = mapper.Map(data.Tables[0]).ToList();
            return persons.Count > 0 ? persons.FirstOrDefault().receives_econ_commentary : false;
        }

        public string GetContactCode(int? contactId)
        {
            return _context.Contacts
                .Where(x => x.id == contactId)
                .Select(x => x.code)
                .FirstOrDefault();
        }

        //public void UpdateEmail(int? contactId, string email)
        //{
        //    var existingAccount = _context.AspNetUsers
        //        .Where(x => x.contact_id == contactId).FirstOrDefault();
        //    existingAccount.Email = email;
        //    _context.SaveChanges();
        //}

        public DataSet UpdateEmail(int? contactId, string email)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Update_User_Username", contactId, email);
            return data;
        }

        //public void UpdateStatus(int? contactId, bool isActive)
        //{
        //    var existingAccount = _context.AspNetUsers
        //        .Where(x => x.contact_id == contactId).FirstOrDefault();
        //    existingAccount.is_active = isActive;
        //    _context.SaveChanges();
        //}

        public DataSet UpdateStatus(int? contactId, bool isActive)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Update_User_Status", contactId, isActive);
            return data;
        }

        //public void SendEmailNotifications(int? contactId, bool sendNotifications)
        //{
        //    var existingAccount = _context.AspNetUsers
        //        .Where(x => x.contact_id == contactId).FirstOrDefault();
        //    existingAccount.gets_email_notifications = sendNotifications;
        //    _context.SaveChanges();
        //}

        public DataSet SendEmailNotifications(int? contactId, bool sendNotifications)
        {
            DataSet data = _helper.ExecuteNotifiactionsStoredProcedure(connectionString, "p_Web_Update_User_Notifications", contactId, sendNotifications);
            return data;
        }

        //public void PostEqtyWriteUps(int? contactId, bool post)
        //{
        //    var existingAccount = _context.AspNetUsers
        //        .Where(x => x.contact_id == contactId).FirstOrDefault();
        //    existingAccount.receives_equity_writeups = post;
        //    _context.SaveChanges();
        //}

        public DataSet PostEqtyWriteUps(int? contactId, bool post)
        {
            DataSet data = _helper.ExecutePostStoredProcedure(connectionString, "p_Web_Update_User_EqtyWriteUps", contactId, post);
            return data;
        }

        public DataSet PostEconomicCommentary(int? contactId, bool post)
        {
            DataSet data = _helper.ExecutePostStoredProcedure(connectionString, "p_Web_Update_User_Economic_Commentary", contactId, post);
            return data;
        }

        public DataSet UpdateTSEmailNotification(int? firmUserGroupId, bool emailNotification, string userName)
        {
            DataSet data = _helper.ExecuteUpdateTSStoredProcedure(connectionString, "p_Web_Update_FUG_Email_Notification", firmUserGroupId, emailNotification ? 1 : 0, userName);
            return data;
        }

        public DataSet UpdateTSEquityWriteUps(int? firmUserGroupId, bool equityWriteUps, string userName)
        {
            DataSet data = _helper.ExecuteUpdateTSStoredProcedure(connectionString, "p_Web_Update_FUG_EquityWriteUps", firmUserGroupId, equityWriteUps ? 1 : 0, userName);
            return data;
        }

        public DataSet UpdateTSEconomicCommentary(int? firmUserGroupId, bool economicCommentary, string userName)
        {
            DataSet data = _helper.ExecuteUpdateTSStoredProcedure(connectionString, "p_Web_Update_FUG_Economic_Commentary", firmUserGroupId, economicCommentary ? 1 : 0, userName);
            return data;
        }
    }
}
