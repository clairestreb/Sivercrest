using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Utilities
{
    public static class DateConverter
    {
        public static string ConvertDateToString(DateTime? date)
        {
            return date.Value.ToString("MMMM dd, yyyy ", CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
