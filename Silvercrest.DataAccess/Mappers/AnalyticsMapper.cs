using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.ViewModels.TeamMember;
using System.Collections.Generic;

namespace Silvercrest.DataAccess.Mappers
{
    public class AnalyticsMapper
    {
        public static LoginSessionRecord MapLoginSessionRecord(Web_Login_Session_Record webRecord)
        {
            var record = new LoginSessionRecord();
            record.Id = webRecord.id;
            record.LoginDate = webRecord.login_date;
            record.LogoutDate = webRecord.logout_date;
            record.MinutesOnline = webRecord.minutes_online.Value;
            record.UserId = webRecord.web_user_id;
            return record;
        }

        public static List<ManagerContactAnalytics> MapContactAnalyticsRecord(List<p_Web_Team_Contact_Analytics_Result> webRecords)
        {
            var records = new List<ManagerContactAnalytics>();
            System.DateTime dt;
            foreach (var item in webRecords)
            {
                var record = new ManagerContactAnalytics();
                record.ContactId = item.contact_id;
                record.Email = item.email_address;
                record.FamilyId = item.family_group_id;
                record.FirmUserGroupId = item.firm_user_group_id;
                dt = (item.last_login != null ? (System.DateTime)item.last_login: System.DateTime.MinValue);
                record.LastLogin = dt.ToLocalTime();
                record.LoggedInTime = (item.last_login != null ? item.avg_time_logged_in : null);
                record.LoginFrequency = (item.last_login != null ? item.login_freq : null);
                record.UserName = item.display_name;
                records.Add(record);
            }
            return records;
        }
    }
}
