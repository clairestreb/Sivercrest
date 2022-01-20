using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Holdings
{
    public class IncomeDataViewModel:GenericViewModel
    {
        public string SecType { get; set; }
        public double? SecType_Percent { get; set; }
        public double? Market_Value { get; set; }
        public string largest_sectype { get; set; }
        public double? Sector_Percent { get; set; }
        public string Sector { get; set; }
    }
}
