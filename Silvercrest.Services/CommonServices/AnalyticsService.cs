using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities.Enums;
using Silvercrest.Interfaces.CommonServices;
using Silvercrest.ViewModels.Common.Analytics;
using System;

namespace Silvercrest.Services.CommonServices
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly AnalyticsRepository _repository;

        public AnalyticsService(SLVR_DEVEntities context)
        {
            _repository = new AnalyticsRepository(context);
        }

        public void RecordAccountPageVisit(string userId, string route, ViewAccountRouteParametersModel model)
        {
            var pageType = model.IsGroup == true ? PageType.Group : PageType.Account;
            var checkResult = _repository.CheckIfAccoutPageExists(userId, pageType, model);
            if (checkResult != null)
            {
                UpdatePageVisitRecord(checkResult);
            }
            if (checkResult == null)
            {
                SaveNewPageVisitRecord(route, userId, pageType);
            }
        }
        
        public void RecordNonAccountPageVisit(string userId, string route)
        {
            var parameterlessRoute = GetParameterllesRoute(route);
            var checkResult = _repository.CheckIfNonAccoutPageExists(userId, parameterlessRoute);
            if (checkResult != null)
            {
                UpdatePageVisitRecord(checkResult);
            }
            if (checkResult == null)
            {
                SaveNewPageVisitRecord(parameterlessRoute, userId, PageType.None);
            }
        }

        private void UpdatePageVisitRecord(Web_Page_Visit_Record webRecord)
        {
            webRecord.visit_count = webRecord.visit_count + 1;
            _repository.UpdatePageVisitRecord(webRecord);
        }

        private void SaveNewPageVisitRecord(string route, string userId, PageType pageType)
        {
            var webRecord = new Web_Page_Visit_Record();
            webRecord.route = route;
            webRecord.page_type = (int)pageType;
            webRecord.web_user_id = userId;
            webRecord.visit_count = 1;
            webRecord.insert_date = DateTime.UtcNow;
            webRecord.insert_by = userId;
            _repository.AddPageVisitRecord(webRecord);
        }

        private string GetParameterllesRoute(string route)
        {
            var indexOfParameters = route.IndexOf("?");
            if (indexOfParameters == -1)
            {
                return route;
            }
            return route.Substring(0, indexOfParameters);
        }

        public int CreateLoginRecord(string userId, string previousLoginRecordId)
        {
            var webRecord = GetLoginSessionRecord(previousLoginRecordId);
            var now = DateTime.UtcNow;
            //if (webRecord != null)
            //{
            //    webRecord.logout_date = now;
            //}
            var newLoginRecord = new Web_Login_Session_Record();
            newLoginRecord.web_user_id = userId;
            newLoginRecord.minutes_online = 0;
            newLoginRecord.insert_date = now;
            newLoginRecord.login_date = now;
            newLoginRecord.update_date = now;
            return _repository.AddLoginSessionRecord(webRecord, newLoginRecord);
        }

        public void UpdateLoggedInTime(string previousLoginRecordId)
        {
            var webRecord = GetLoginSessionRecord(previousLoginRecordId);
            var now = DateTime.UtcNow;
            if (webRecord == null || (webRecord.update_date.AddMinutes(1) > now))
            {
                return;
            }
            webRecord.update_date = now;
            webRecord.minutes_online = webRecord.minutes_online + 1;
            _repository.UpdateLoginSessionRecordTime(webRecord);
        }

        public void UpdateRecordOnLogout(string previousLoginRecordId)
        {
            var webRecord = GetLoginSessionRecord(previousLoginRecordId);
            if (webRecord == null)
            {
                return;
            }
            var now = DateTime.UtcNow;
            webRecord.logout_date = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
            _repository.UpdateLoginSessionRecordOnLogout(webRecord);
        }

        private Web_Login_Session_Record GetLoginSessionRecord(string loginRecordId)
        {
            if (string.IsNullOrWhiteSpace(loginRecordId))
            {
                return null;
            }
            int sessionRecordId;
            var parsingResult = int.TryParse(loginRecordId, out sessionRecordId);
            if (sessionRecordId == 0)
            {
                return null;
            }
            return _repository.GetLoginRecord(sessionRecordId);
        }
    }
}
