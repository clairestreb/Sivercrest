using System.ComponentModel;

namespace Silvercrest.Entities.Enums
{
    public enum TwoFactorAuth
    {
        [Description("Inactive")]
        Inactive = 0,
        [Description("Email")]
        Email = 1,
        [Description("Text Message")]
        TextMessage = 2
    }
}
