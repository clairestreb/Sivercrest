using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.Entities;
using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;
namespace Silvercrest.DataAccess.Repositories
{
    public class AccountRepository
    {
        private SLVR_DEVEntities _context;

        public AccountRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public List<Entities.Account> GetAccountsWithinGroup(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            List<f_Web_Accounts_Within_Group_Result> accountList = _context.f_Web_Accounts_Within_Group(contactId, entity_id, is_group == true ? 1 : 0, is_client_group == true ? 1 : 0).ToList();
            return AccountMapper.MapAccountsWithinGroup(accountList);
        }

        public List<ClientAccount> GetAccountAndGroupsList(int? contactId)
        {
            IList<p_Web_Contact_Entities_Result> accountList = _context.p_Web_Contact_Entities(contactId).ToList();
            var mappedList = AccountMapper.MapAccountsAndGroupsList(accountList);
            return mappedList;
        }

        public UserInfo GetUserInfo(int? contactId)
        {
            IList<p_Web_Contact_Entities_Result> accountList = _context.p_Web_Contact_Entities(contactId).ToList();
            var mappedInfo = AccountMapper.MapUserInfo(accountList);
            return mappedInfo;
        }

        public List<Entities.Account> GetAccountList(int? contactId)
        {
            IList<p_Web_Contact_Entities_Result> accountList = _context.p_Web_Contact_Entities(contactId).ToList();
            var mappedList = AccountMapper.MapAccountsList(accountList);
            return mappedList;
        }
        
        public List<Entities.Account> GetRelationshipAccountList(int? contactId, int? firmUserGroupID)
        {
            IList<p_Web_Relationship_Accounts_Result> accountList = _context.p_Web_Relationship_Accounts(contactId, firmUserGroupID).ToList();
            var mappedList = AccountMapper.MapRelationshipAccountsList(accountList);
            return mappedList;
        }

        public List<PieChart> GetChartAssetClass(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_Allocation_By_Asset_Class_Result> charts = _context.p_Web_Allocation_By_Asset_Class(contactId, entity_id, is_group, is_client_group, null).ToList();
            var mappedItem = AccountMapper.MapChartsItem(charts);
            return mappedItem;
        }

        public void AddNickname(int contactId, int accountId, string mainId, string nickname)
        {
            var elem = _context.Web_Account_Nickname.Where(x => x.contact_id == contactId && x.account_id == accountId).FirstOrDefault();
            if (elem == null)
            {
                elem = new Web_Account_Nickname();
                elem.account_id = accountId;
                elem.contact_id = contactId;
                elem.insert_by = mainId;
                elem.insert_date = DateTime.Now;
                elem.nickname = nickname;
                _context.Web_Account_Nickname.Add(elem);
            }
            else
            {
                elem.nickname = nickname;
                elem.update_by = mainId;
                elem.update_date = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public void AcceptTerms(int id)
        {
            _context.AspNetUsers.Where(x => x.contact_id == id).FirstOrDefault().accepted_terms = DateTime.UtcNow;
            _context.SaveChanges();
        }

        public void DeleteNickname(int contactId, int accountId)
        {
           var elem = _context.Web_Account_Nickname.Where(x => x.contact_id == contactId && x.account_id == accountId).FirstOrDefault();
            if(elem != null)
                _context.Web_Account_Nickname.Remove(elem);
            _context.SaveChanges();
        }

        public List<AccountNickname> GetNicknames(int? contactId)
        {
            IList<p_Web_Account_Nicknames_Result> result = _context.p_Web_Account_Nicknames(contactId).ToList();
            var mappedItem = AccountMapper.MapNicknames(result);
            return mappedItem;
        }

        //public List<PieChart> GetChartStrategy(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group)
        //{
        //    IList<p_Web_Top8_Strategies_Result> charts = _context.p_Web_Top8_Strategies(contact_id, entity_id, is_group, is_client_group).ToList();
        //    var mappedItem = AccountMapper.MapChartsItem(charts);
        //    return mappedItem;
        //}

        //public List<Balance> GetBalances(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group)
        //{
        //    IList<p_Web_Allocation_By_Strategy_Result> balance = _context.p_Web_Allocation_By_Strategy(contact_id, entity_id, is_group, is_client_group).ToList();
        //    var a = balance.Select(x => x.outer_asset_class == "Margin");
        //    var mappedItem = AccountMapper.MapBalanceItem(balance);
        //    return mappedItem;
        //}

        public List<Balance> GetBalancesShort(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_Allocation_By_Asset_Class_Result> balance = _context.p_Web_Allocation_By_Asset_Class(contact_id, entity_id, is_group, is_client_group, null).ToList();
            var mappedItem = AccountMapper.MapBalanceShortItem(balance);
            return mappedItem;
        }

        public List<Contribution> GetContributions(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            if (startDate == "")
            {
                startDate = null;
            }
            if (endDate == "")
            {
                endDate = null;
            }
            IList<p_Web_Transactions_Result> contributions = _context.p_Web_Transactions(contact_id, entity_id, is_group, is_client_group, startDate, endDate, "Contributions | Withdrawals").ToList();
            var mappedItem = AccountMapper.MapContributionList(contributions);
            return mappedItem;
        }

        public List<Purchase> GetPurchases(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            IList<p_Web_Transactions_Result> purchases = _context.p_Web_Transactions(contact_id, entity_id, is_group, is_client_group, startDate, endDate, "Purchases").ToList();
            var mappedItem = AccountMapper.MapPurchasesList(purchases);
            return mappedItem;
        }

        public List<Sale> GetSales(int? contact_id, int? entity_id, bool? is_group, bool? is_client_group, string startDate, string endDate)
        {
            IList<p_Web_RealizedGL_Result> sales = _context.p_Web_RealizedGL(contact_id, entity_id, is_group, is_client_group, startDate, endDate).ToList();
            var mappedItem = AccountMapper.MapSalesList(sales);
            return mappedItem;
        }

        public string GetUserName(int? contactId)
        {
            return _context.Contacts.Where(x => x.id == contactId).Select(x => x.first_name + " " + x.last_name).FirstOrDefault();
        }

        public string GetGroupNameOfNotClientGroup(int? entityId)
        {
            return _context.Account_Group.FirstOrDefault(ag => ag.id == entityId)?.name;
        }

        public string GetGroupNameOfClientGroup(int? entityId, int? contactId)
        {
            return _context.Web_Account_Group.FirstOrDefault(wag => wag.contact_id == contactId.Value && wag.account_group_id == entityId)?.name;
        }

        public Info GetInfo(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, int? type = null)
        {
            string typeStr = "";
            if (type == 1) typeStr = "ca";
            if (type == 2) typeStr = "fi";
            if (type == 3) typeStr = "eq";
            if (type == 4) typeStr = "ot";
            if (type == 0 && type == null) typeStr = null;
            
            IList<p_Web_Allocation_By_Asset_Class_Result> info = _context.p_Web_Allocation_By_Asset_Class(contactId, entity_id, is_group, is_client_group, typeStr).ToList();           
            var mappedItem = AccountMapper.MapInfoItem(info);
            return mappedItem;
        }
        
        public AspNetUserLogin GetStatsByUserId(string UserId)
        {
            var stats = _context.AspNetUserLogins.Include("AspNetUser").FirstOrDefault();
            return stats;
        }

        public string GetDefaultGroup(int? entityId)
        {
            return _context.Accounts.FirstOrDefault(a=>a.id==entityId)?.long_name;
        }

        public void UpdateStats(string UserId)
        {
            var user = _context.AspNetUserLogins.Include("AspNetUser").Where(x => x.UserId == UserId).FirstOrDefault();
            if (user == null)
            {
                return;
            }
            user.AspNetUser.last_visit = DateTime.Now;
        }

        public int? GetContactId(string name)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Email == name)?.contact_id;
        }

        public string GetUserCode(string name)
        {
            return _context.AspNetUsers.Where(x => x.UserName == name).Select(x => x.Contact.code).First();
        }

        public List<Silvercrest.Entities.Cash> GetCashes(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string category)
        {
            IList<p_Web_Positions_By_Asset_Class_Result> cashes = _context.p_Web_Positions_By_Asset_Class(contactId, entity_id, is_group, is_client_group, category).ToList();
            var mappedItem = AccountMapper.MapCashItem(cashes);
            return mappedItem;
        }

        public List<Silvercrest.Entities.Cash> GetAssets(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string category)
        {
            IList<p_Web_Positions_By_Asset_Class_Result> assets = _context.p_Web_Positions_By_Asset_Class(contactId, entity_id, is_group, is_client_group, category).ToList();
            var mappedItem = AccountMapper.MapCashItem(assets);
            return mappedItem;
        }

        public List<Silvercrest.Entities.Income> GetIncomes(int? contactId, int? entity_id, bool? is_group, bool? is_client_group, string category)
        {
            IList<p_Web_Positions_By_Asset_Class_Result> incomes = _context.p_Web_Positions_By_Asset_Class(contactId, entity_id, is_group, is_client_group, category).ToList();
            var mappedItem = AccountMapper.MapIncomeItem(incomes);
            return mappedItem;
        }
        
        /*public List<PieChart> GetChartType(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_FI_Allocation_Result> charts = _context.p_Web_FI_Allocation(contactId, entity_id, is_group, is_client_group).ToList();
            var mappedItem = AccountMapper.MapTypeCharts(charts);
            
            return mappedItem;
        }

        public List<PieChart> GetChartMunicBound(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_FI_Allocation_Result> charts = _context.p_Web_FI_Allocation(contactId, entity_id, is_group, is_client_group).ToList();
            var mappedItem = AccountMapper.MapMunicBoundCharts(charts);
            return mappedItem;
        }

        public List<PieChart> GetEquitiesChartType(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_EQ_Allocation_Result> charts = _context.p_Web_EQ_Allocation(contactId, entity_id, is_group, is_client_group).ToList();
            var mappedItem = AccountMapper.MapTypeCharts(charts);
            return mappedItem;
        }

        public List<PieChart> GetEquitiesChartMunicBound(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            IList<p_Web_EQ_Allocation_Result> charts = _context.p_Web_EQ_Allocation(contactId, entity_id, is_group, is_client_group).ToList();
            var mappedItem = AccountMapper.MapMunicBoundCharts(charts);
            return mappedItem;
        }*/
               
        public bool GetHoldingsType(int? contact_id)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var info = GetUserInfo(contact_id);
                SqlCommand command = new SqlCommand(string.Format("select dbo.f_Web_Display_Strategy_Allocation({0}, {1}, {2}, {3})", 
                    contact_id, info.EntityId, info.IsGroup == true? 1: 0, info.IsClientGroup == true ? 1 : 0), connection);
                return Convert.ToBoolean(command.ExecuteScalar());
            }                       
        }

        public void GetMultiData(int? contactId, int? entity_id, bool? is_group, bool? is_client_group)
        {
            var result = _context.p_WebMulti_Allocation_By_AssetClass(contactId, entity_id, is_group, is_client_group);
        }
       
    }
}
