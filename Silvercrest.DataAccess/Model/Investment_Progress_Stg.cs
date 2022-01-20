//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Silvercrest.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Investment_Progress_Stg
    {
        public string period_to_date { get; set; }
        public System.DateTime from_date { get; set; }
        public System.DateTime thru_date { get; set; }
        public int account_id { get; set; }
        public int currency_id { get; set; }
        public int classification_id { get; set; }
        public double market_value_1 { get; set; }
        public double market_value_2 { get; set; }
        public double cost_basis { get; set; }
        public double realized_gl_cost { get; set; }
        public double realized_gl_mv { get; set; }
        public double unrealized_gl_cost { get; set; }
        public double unrealized_gl_mv { get; set; }
        public double realized_gl_fx { get; set; }
        public double unrealized_gl_fx { get; set; }
        public double accrued_interest_1 { get; set; }
        public double accrued_interest_2 { get; set; }
        public double dividends { get; set; }
        public double interest { get; set; }
        public double contributions { get; set; }
        public double withdrawals { get; set; }
        public double transfers_in { get; set; }
        public double transfers_out { get; set; }
        public double fees { get; set; }
        public double management_fees { get; set; }
        public double port_fees_paid_by_client { get; set; }
        public double mgmt_fees_paid_by_client { get; set; }
    }
}
