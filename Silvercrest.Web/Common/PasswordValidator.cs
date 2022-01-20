using Silvercrest.ViewModels.Main.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silvercrest.Web.Common
{
    public static class PasswordValidator
    {
        public static PasswordValidationViewModel ValidatePassword(string password)
        {
            var model = new PasswordValidationViewModel();
            List<string> errors = new List<string>();
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 25;

            if (password == null) throw new ArgumentNullException();
            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                    {
                        hasUpperCaseLetter = true;
                    }
                    if (char.IsLower(c))
                    {
                        hasLowerCaseLetter = true;
                    }
                    if (char.IsDigit(c))
                    {
                        hasDecimalDigit = true;
                    }
                }
            }

            bool isValid = meetsLengthRequirements && hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit;
            if (!isValid)
            {
                if (!meetsLengthRequirements)
                {
                    errors.Add("Password must be from 8 to 25 characters");
                }
                if (!hasDecimalDigit)
                {
                    errors.Add("Password must contain digit");
                }
                if (!hasLowerCaseLetter || !hasUpperCaseLetter)
                {
                    errors.Add("Password must contain uppercase and lowercase character");
                }
            }
            model.IsValid = isValid;
            model.Errors = errors;
            return model;
        }


        public static string ValidateRole(List<Entities.Enums.UserRole> list)
        {
            if (list.Exists(x => x == Entities.Enums.UserRole.SuperUser))
                return "SuperUser";
            if (list.Exists(x => x == Entities.Enums.UserRole.Administrator))
                return "Administrator";
            if (list.Exists(x => x == Entities.Enums.UserRole.TeamMember))
                return "TeamMember";
            else
                return "Client";
        }

        public static bool ValidateBrowser(HttpBrowserCapabilitiesBase browser)
        {
            //switch (browser.Browser)
            //{
            //    case1
            //    default:
            //        break;
            //}
            return true;
        }
    }
}