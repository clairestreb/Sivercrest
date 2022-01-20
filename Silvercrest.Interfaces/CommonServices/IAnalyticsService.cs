using Silvercrest.ViewModels.Common.Analytics;

namespace Silvercrest.Interfaces.CommonServices
{
    public interface IAnalyticsService
    {
        void RecordNonAccountPageVisit(string userId, string pageRoute);
        void RecordAccountPageVisit(string userId, string route, ViewAccountRouteParametersModel model);
        void UpdateLoggedInTime(string previousLoginRecordId);
        int CreateLoginRecord(string userId, string previousLoginRecordId);
        void UpdateRecordOnLogout(string previousLoginRecordId);
    }
}
