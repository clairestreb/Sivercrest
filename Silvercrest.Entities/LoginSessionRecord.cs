using System;

namespace Silvercrest.Entities
{
    public class LoginSessionRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public int MinutesOnline { get; set; }
        public DateTime? LastTimeUpdated { get; set; }
        public LoginSessionRecord()
        {
            var now = DateTime.UtcNow;
            LoginDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
        }
    }
}
