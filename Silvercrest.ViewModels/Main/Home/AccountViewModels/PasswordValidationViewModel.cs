using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Main.AccountViewModels
{
    public class PasswordValidationViewModel
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}
