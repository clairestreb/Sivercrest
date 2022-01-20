using Silvercrest.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.TeamMember
{
    public class AccountAccessInfo:GenericViewModel
    {
        public int? ContactId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? FamilyId { get; set; }
        public int? FirmUserGroupId { get; set; }
        public bool IsActive { get; set; }
        public bool SendNotifications { get; set; }
        public bool Post { get; set; }
        public string ContactCode { get; set; }
        public bool EconomicCommentary { get; set; }
        public TwoFactorAuth TwoFactorAuth { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([+]?[0-9]+[+-]?)+$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
    }
}
