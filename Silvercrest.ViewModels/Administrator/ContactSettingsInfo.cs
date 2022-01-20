using Silvercrest.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Silvercrest.ViewModels.Administrator
{
    public class ContactSettingsInfo : GenericViewModel
    {
        public int? ContactId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string ContactCode { get; set; }
        public TwoFactorAuth? TwoFactorAuth { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([+]?[0-9]+[+-]?)+$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
    }
}
