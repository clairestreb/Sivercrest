using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Utilities
{
    public static class PasswordGenerator
    {
        public static string GetPass()
        {
            string password = "";
            var r = new Random();
            while (password.Length < 12)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                {
                    password += c;
                }
            }
            return password;
        }
    }
}
