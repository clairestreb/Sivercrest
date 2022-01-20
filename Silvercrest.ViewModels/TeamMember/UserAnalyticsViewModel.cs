using System;

namespace Silvercrest.ViewModels.TeamMember
{
    public class UserAnalyticsViewModel:GenericViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LastLogin { get; set; }
        public string LoginFrequency { get; set; }
        public string LoggedInTime { get; set; } 
        public string ViewedPage { get; set; }
        public string ViewedAccount { get; set; }
        public string ViewedGroup { get; set; }
        public string WebUserId { get; set; }
        public int? ContactId { get; set; }
        public int? FamilyId { get; set; }
        public int? FirmUserGroupId { get; set; }
        public bool? IsFromClientAnalytics { get; set; }
    }
}