using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silvercrest.ViewModels.Client.UserSettings
{
    public class IndexViewModel : GenericViewModel
    {
        public List<SecretQuestion> SecretQuestions { get; set; }

        public string FullName { get; set; }

        public bool ShowSecuritySection { get; set; }

        public bool ShowCustomizeSection { get; set; }

        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Question 1")]
        public int SecretQuestionsId1 { get; set; }

        [Display(Name = "Question 2")]
        public int SecretQuestionsId2 { get; set; }

        [Display(Name = "Question 3")]
        public int SecretQuestionsId3 { get; set; }

        [Display(Name = "Answer 1")]
        public string QuestionAnswer1 { get; set; }

        [Display(Name = "Answer 2")]
        public string QuestionAnswer2 { get; set; }

        [Display(Name = "Answer 3")]
        public string QuestionAnswer3 { get; set; }

        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "All groups")]
        public List<ClientAccount> GroupList { get; set; }

        public string SelectedGroup { get; set; }

        public string defaultGroup { get; set; }

        public string CurrentUnsupervisedValue { get; set; }
        public string NewUnsupervisedValue { get; set; }

        public string ContactIdQuery { get; set; }

        public TwoFactorAuth TwoFactorAuth { get; set; }
    }
}
