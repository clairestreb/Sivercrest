using System;
using System.Linq;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.ViewModels.TeamMember;
using Silvercrest.Interfaces.TeamMember;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Silvercrest.Services.CommonServices;

namespace Silvercrest.Services.TeamMember
{
    public class UserAnalyticsService : IUserAnalyticsService
    {
        private AccountRepository _accountRepository;
        private readonly AnalyticsRepository _analyticsRepository;

        public UserAnalyticsService(SLVR_DEVEntities context)
        {
            _accountRepository = new AccountRepository(context);
            _analyticsRepository = new AnalyticsRepository(context);
        }

        private string GetLoginFrequency(DateTime creationDate, int loginCount)
        {
            if (loginCount == 0)
            {
                return "0 days";
            }
            double daysFromCreation = (DateTime.UtcNow - creationDate).TotalHours/24.0;
            double dblAvg = daysFromCreation / (1.0 * loginCount);
            if(dblAvg >= 1.0)
            {
                int days = (int)Math.Round(daysFromCreation / (1.0 * loginCount));
                return $"{days} day(s)";
            }

            int hours = (int)Math.Round(daysFromCreation * 24.0/ (1.0 * loginCount));
            return $"{hours} hour(s)";

        }

        public UserAnalyticsViewModel GetAnalytics(int contactId, int familyId)
        {
            var user = _analyticsRepository.GetUserByContactId(contactId);
            var userAnalytics = new UserAnalyticsViewModel();
            userAnalytics.FamilyId = familyId;
            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                return userAnalytics;
            }
            var userId = user.Id;
            userAnalytics.WebUserId = userId;
            var userLoginRecords = _analyticsRepository.GetUserLoginRecords(userId);
            userAnalytics.Email = user.Email;
            userAnalytics.UserName = GetNameByContactId(user.contact_id);
            if (userLoginRecords.Count() == 0)
            {
                return userAnalytics;
            }
            var lastLoginRecord = userLoginRecords.OrderByDescending(lsr => lsr.LoginDate).First();
            DateTime lastLoginDate = new DateTime(lastLoginRecord.LoginDate.Ticks, DateTimeKind.Utc);
            lastLoginDate = lastLoginDate.ToLocalTime();
            var culture = new System.Globalization.CultureInfo("en-US");
            var usaDateFormatInfo = culture.DateTimeFormat;
            userAnalytics.LastLogin = lastLoginDate.ToString("M/d/yyyy HH:mm", usaDateFormatInfo);
            userAnalytics.LoggedInTime = GetAverageTimeLoggedIn(userLoginRecords);
            int loginCount = _analyticsRepository.GetLoginCount(userId);
            userAnalytics.LoginFrequency = GetLoginFrequency(user.insert_date, loginCount).ToString();
            var mostVisitedPages = _analyticsRepository.GetMostVisitedRecords(userId);
            var mostVisitedAccount = mostVisitedPages.FirstOrDefault(p => p.page_type == (int)PageType.Account);
            var mostVisitedGroup = mostVisitedPages.FirstOrDefault(p => p.page_type == (int)PageType.Group);
            var mostVisitedPage = mostVisitedPages.FirstOrDefault(p => p.page_type == (int)PageType.None);
            if (mostVisitedAccount != null)
            {
                userAnalytics.ViewedAccount = GetGroup(mostVisitedAccount.route);
            }
            if (mostVisitedGroup != null)
            {
                userAnalytics.ViewedGroup = GetGroup(mostVisitedGroup.route);
            }
            if (mostVisitedPage != null)
            {
                userAnalytics.ViewedPage = GetMostVisitedPage(mostVisitedPage.route);
            }
            return userAnalytics;
        }

        public List<ManagerContactAnalytics> GetAllAnalytics(int? contactId)
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            var usaDateFormatInfo = culture.DateTimeFormat;
            var records = _analyticsRepository.GetAllUserLoginRecords(contactId);
            return records;
        }

        private string GetNameByContactId(int contactId)
        {
            var contact = _analyticsRepository.GetContactById(contactId);
            if (contact == null)
            {
                return string.Empty;
            }
            var account = $"{contact.first_name} {contact.middle_name} {contact.last_name} {contact.suffix}";
            return account;
        }

        private string GetGroup(string route)
        {
            var pathFiller = "http://localhost:5000";
            var uri = new Uri(pathFiller + route);
            var isGroup = ParametersParser.GetBoolValue(HttpUtility.ParseQueryString(uri.Query).Get("isGroup"));
            var isClientGroup = ParametersParser.GetBoolValue(HttpUtility.ParseQueryString(uri.Query).Get("isClientGroup"));
            var contactId = ParametersParser.GetIntValue(HttpUtility.ParseQueryString(uri.Query).Get("contactId"));
            var entityId = ParametersParser.GetIntValue(HttpUtility.ParseQueryString(uri.Query).Get("entityId"));
            if (isGroup.HasValue && isGroup.Value==true)
            {
                return GetGroupName(isClientGroup, contactId, entityId);            
            }
            if (isGroup.HasValue && isGroup.Value == false)
            {
                return _accountRepository.GetDefaultGroup(entityId);
            }
            return string.Empty;
        }

        private string GetGroupName(bool? isClientGroup, int? contactId, int? entityId)
        {
            if (entityId.HasValue && entityId.Value < 0)
            {
                return "All Accounts";
            }
            if (isClientGroup.HasValue &&
                isClientGroup.Value == false)
            {
                return _accountRepository.GetGroupNameOfNotClientGroup(entityId);
            }
            if (isClientGroup.HasValue &&
                isClientGroup.Value == true)
            {
                return _accountRepository.GetGroupNameOfClientGroup(entityId, contactId);
            }
            return string.Empty;
        }

        private string GetAverageTimeLoggedIn(List<LoginSessionRecord> userLoginRecords)
        {
            if (userLoginRecords.Count() < 1)
            {
                return "0";
            }
            var minutesSum = 0;
            foreach (var record in userLoginRecords)
            {
                minutesSum += record.MinutesOnline;
            }
            var averageMinutes = minutesSum / userLoginRecords.Count;

            return GetTimeOnline(averageMinutes);
        }

        public string GetTimeOnline(int recordMinutes)
        {
            var timeStringBuilder = new StringBuilder();
            var days = Math.Abs(recordMinutes / (60 * 24));
            var hours = (recordMinutes % (60 * 24)) / 60;
            if (days > 0)
            {
                timeStringBuilder.Append($"{days} day(s) ");
                timeStringBuilder.Append($"{hours} hour(s) ");
            }
            if (days < 1 && hours > 0)
            {
                timeStringBuilder.Append($"{hours} hour(s) ");
            }
            var minutes = recordMinutes % 60;
            timeStringBuilder.Append($"{minutes} minute(s)");
            return timeStringBuilder.ToString();
        }

        private string GetMostVisitedPage(string route)
        {
            var routeWithoutFirstDelimiter = route.Substring(1);
            var routeParts = routeWithoutFirstDelimiter.Split('/');
            if (routeParts.Length < 2)
            {
                return string.Empty;
            }
            return ProcessParameterlessRoute(routeParts);
        }

        private string ProcessParameterlessRoute(string[] routeParts)
        {
            var indexOfArea = 0;
            var indexOfController = 1;
            var indexOfActionPart = 2;
            var nameStringBuilder = new StringBuilder();
            nameStringBuilder.Append(routeParts[indexOfArea]);
            nameStringBuilder.Append(" ");
            nameStringBuilder.Append(routeParts[indexOfController]);
            if (routeParts.Length > 2)
            {
                nameStringBuilder.Append(" ");
                nameStringBuilder.Append(routeParts[indexOfActionPart]);
            }
            return nameStringBuilder.ToString();
        }

        public List<LoginSessionRecord> GetLoginHistory(string webUserId)
        {
            return _analyticsRepository.GetLoginHistory(webUserId);
        }

        public LoginHistoryViewModel GetLoginHistoryViewModel(string webUserId,int familyId)
        {
            var model = new LoginHistoryViewModel();
            var contact = _analyticsRepository.GetContactByWebUserId(webUserId);
            model.ContactFullName = $"{contact.first_name} {contact.middle_name} {contact.last_name} {contact.suffix}";
            model.WebUserId = webUserId;
            model.ContactId = contact.id;
            model.FamilyId = familyId;
            return model;
        }
    }
}
