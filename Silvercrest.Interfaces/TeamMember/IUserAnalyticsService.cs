using Silvercrest.Entities;
using Silvercrest.ViewModels.TeamMember;
using System.Collections.Generic;

namespace Silvercrest.Interfaces.TeamMember
{
    public interface IUserAnalyticsService
    {
        UserAnalyticsViewModel GetAnalytics(int contactId,int familyId);
        LoginHistoryViewModel GetLoginHistoryViewModel(string webUserId,int familyId);
        List<LoginSessionRecord> GetLoginHistory(string webUserId);
        string GetTimeOnline(int recordMinutes);
        List<ManagerContactAnalytics> GetAllAnalytics(int? contactId);
    }
}
