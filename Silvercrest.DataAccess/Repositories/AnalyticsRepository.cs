using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using System.Linq;
using System.Collections.Generic;
using Silvercrest.DataAccess.Mappers;
using Silvercrest.Entities.Enums;
using System;
using Silvercrest.ViewModels.Common.Analytics;

namespace Silvercrest.DataAccess.Repositories
{
    public class AnalyticsRepository
    {
        private readonly SLVR_DEVEntities _context;
        public AnalyticsRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Model.Contact GetContactById(int contactId)
        {
            return _context.Contacts.FirstOrDefault(c => c.id == contactId);
        }

        public void AddPageVisitRecord(Web_Page_Visit_Record webRecord)
        {
            var test = _context.Database.CurrentTransaction;
            _context.Web_Page_Visit_Record.Add(webRecord);
            _context.SaveChanges();
        }

        public AspNetUser GetUserById(string userId)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Id == userId);
        }

        public void UpdatePageVisitRecord(Web_Page_Visit_Record webRecord)
        {
            _context.Web_Page_Visit_Record.Attach(webRecord);
            var entry = _context.Entry(webRecord);
            entry.Property(e => e.visit_count).IsModified = true;
            _context.SaveChanges();
        }

        public AspNetUser GetUserByContactId(int contactId)
        {
            return _context.AspNetUsers.FirstOrDefault(anu => anu.contact_id == contactId);
        }

        public Web_Page_Visit_Record CheckIfNonAccoutPageExists(string userId, string parameterlessRoute)
        {
            return _context.Web_Page_Visit_Record.SingleOrDefault(vpr => vpr.web_user_id == userId && vpr.route.ToLower() == parameterlessRoute.ToLower() && vpr.page_type == (int)PageType.None);
        }

        public Web_Page_Visit_Record CheckIfAccoutPageExists(string userId, PageType pageType, ViewAccountRouteParametersModel route)
        {
            return _context.Web_Page_Visit_Record.SingleOrDefault(vpr => vpr.web_user_id == userId &&
            vpr.page_type == (int)pageType &&
            vpr.route.ToLower().Contains((route.ContactIdString + route.ContactId.Value.ToString()).ToLower()) &&
            vpr.route.ToLower().Contains((route.EntityIdString + route.EntityId.Value.ToString()).ToLower()) &&
            vpr.route.ToLower().Contains((route.IsClientGroupString + route.IsClientGroup.Value.ToString()).ToLower()) &&
            vpr.route.ToLower().Contains((route.IsGroupString + route.IsGroup.Value.ToString()).ToLower()));
        }

        public int AddLoginSessionRecord(Web_Login_Session_Record oldRecord, Web_Login_Session_Record newRecord)
        {
            if (oldRecord != null)
            {
                _context.Web_Login_Session_Record.Attach(oldRecord);
                var entry = _context.Entry(oldRecord);
                entry.Property(e => e.logout_date).IsModified = true;
            }
            _context.Web_Login_Session_Record.Add(newRecord);
            _context.SaveChanges();
            return newRecord.id;
        }

        public void UpdateLoginSessionRecordTime(Web_Login_Session_Record webRecord)
        {
            _context.Web_Login_Session_Record.Attach(webRecord);
            var entry = _context.Entry(webRecord);
            entry.Property(e => e.minutes_online).IsModified = true;
            entry.Property(e => e.update_date).IsModified = true;
            _context.SaveChanges();
        }

        public int GetLoginCount(string userId)
        {
            return _context.Web_Login_Session_Record.Where(lsr => lsr.web_user_id == userId).Count();
        }

        public void UpdateLoginSessionRecordOnLogout(Web_Login_Session_Record webRecord)
        {
            _context.Web_Login_Session_Record.Attach(webRecord);
            var entry = _context.Entry(webRecord);
            entry.Property(e => e.logout_date).IsModified = true;
            _context.SaveChanges();
        }
        
        public Web_Login_Session_Record GetLoginRecord(int sessionRecordId)
        {
            return _context.Web_Login_Session_Record.FirstOrDefault(lsr => lsr.id == sessionRecordId && !lsr.logout_date.HasValue);
        }

        public List<LoginSessionRecord> GetUserLoginRecords(string userId)
        {
            var webRecordList = _context.Web_Login_Session_Record.Where(lsr => lsr.web_user_id == userId).ToList();
            var recordList = new List<LoginSessionRecord>();
            foreach (var webRecord in webRecordList)
            {
                var record = AnalyticsMapper.MapLoginSessionRecord(webRecord);
                recordList.Add(record);
            }
            return recordList;
        }

        public List<ManagerContactAnalytics> GetAllUserLoginRecords(int? contactId)
        {
            var webRecordList = _context.p_Web_Team_Contact_Analytics(contactId).ToList();
            var recordList = new List<ManagerContactAnalytics>();
            recordList = AnalyticsMapper.MapContactAnalyticsRecord(webRecordList);
            return recordList;
        }

        public List<Web_Page_Visit_Record> GetMostVisitedRecords(string userId)
        {
            return _context.Web_Page_Visit_Record
                 .Where(pvr => pvr.web_user_id == userId)
                 .GroupBy(pvr => pvr.page_type)
                 .Select(group => group.OrderByDescending(pvr => pvr.visit_count)
                 .FirstOrDefault())
                 .ToList();
        }

        public List<LoginSessionRecord> GetLoginHistory(string userId)
        {
            var webRecordList = _context.Web_Login_Session_Record.Where(lsr => lsr.web_user_id == userId).OrderByDescending(lsr => lsr.login_date).ToList();
            var recordList = new List<LoginSessionRecord>();
            foreach (var webRecord in webRecordList)
            {
                var record = AnalyticsMapper.MapLoginSessionRecord(webRecord);
                recordList.Add(record);
            }
            return recordList;
        }

        public Model.Contact GetContactByWebUserId(string userId)
        {
            var contactId = _context.AspNetUsers.Where(u => u.Id == userId).Select(u => u.contact_id).FirstOrDefault();
            return _context.Contacts.FirstOrDefault(c => c.id == contactId);
        }
    }
}
