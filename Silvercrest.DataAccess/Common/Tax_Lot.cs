using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Common
{
    public class Tax_Lot
    {

        public int account_id { get; set; }
        public int security_id { get; set; }
        public double quantity { get; set; }
        public DateTime original_cost_date { get; set; }
        public double base_cost_basis { get; set; }
        public double base_price { get; set; }
        public double base_accrued_interest { get; set; }
        public double base_total_value { get; set; }
        public double base_annual_income { get; set; }
        public double yield { get; set; }

        public Tax_Lot(int account_id, int security_id, double quantity, DateTime original_cost_date, double base_cost_basis, double base_price, double base_accrued_interest, 
                double base_total_value, double base_annual_income, double yield)
        {
            this.account_id = account_id;
            this.security_id = security_id;
            this.quantity = quantity;
            this.original_cost_date = original_cost_date;
            this.base_cost_basis = base_cost_basis;
            this.base_price = base_price;
            this.base_accrued_interest = base_accrued_interest;
            this.base_total_value = base_total_value;
            this.base_annual_income = base_annual_income;
            this.yield = yield;

        }


    }
}
