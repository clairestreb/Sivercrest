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
    
    public partial class Staging_Tax_Lot
    {
        public bool is_active { get; set; }
        public System.DateTime as_of_date { get; set; }
        public int account_id { get; set; }
        public int security_id { get; set; }
        public int lot_number { get; set; }
        public bool is_short { get; set; }
        public bool is_unsupervised { get; set; }
        public double quantity { get; set; }
        public double unadjusted_quantity { get; set; }
        public double market_value { get; set; }
        public Nullable<System.DateTime> original_cost_date { get; set; }
        public Nullable<int> original_face { get; set; }
        public Nullable<long> tax_lot_id { get; set; }
        public Nullable<double> accrued_interest { get; set; }
        public Nullable<double> annual_income { get; set; }
        public Nullable<double> unrealized_gl { get; set; }
        public Nullable<double> unrealized_gl_fx { get; set; }
        public int holding_duration_id { get; set; }
        public Nullable<double> cost_basis { get; set; }
        public double unadjusted_cost_basis { get; set; }
        public Nullable<double> unit_cost { get; set; }
        public double unadjusted_unit_cost { get; set; }
        public string bond_status_code { get; set; }
        public Nullable<System.DateTime> bond_status_date { get; set; }
        public string bond_description { get; set; }
        public Nullable<System.DateTime> effective_maturity_date { get; set; }
        public double interest_or_dividend_rate { get; set; }
        public Nullable<double> call_price { get; set; }
        public Nullable<System.DateTime> call_date { get; set; }
        public Nullable<double> put_price { get; set; }
        public Nullable<System.DateTime> put_date { get; set; }
        public Nullable<double> duration { get; set; }
        public Nullable<double> duration_on_cost { get; set; }
        public Nullable<double> duration_to_maturity { get; set; }
        public Nullable<double> duration_to_next_call { get; set; }
        public Nullable<double> duration_to_worst { get; set; }
        public Nullable<double> convexity { get; set; }
        public Nullable<double> convexity_to_maturity { get; set; }
        public Nullable<double> convexity_to_worst { get; set; }
        public Nullable<double> yield { get; set; }
        public Nullable<double> yield_to_next_call { get; set; }
        public Nullable<double> yield_to_worst { get; set; }
        public Nullable<double> ytm_on_cost { get; set; }
        public Nullable<double> ytm_to_maturity { get; set; }
        public Nullable<double> amortization_balance { get; set; }
        public Nullable<double> amortization_mtd { get; set; }
        public Nullable<double> amortization_ytd { get; set; }
        public Nullable<double> amortization_ttd { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string delete_by { get; set; }
        public Nullable<System.DateTime> delete_date { get; set; }
    }
}
