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
    
    public partial class f_Get_Positions_By_Account_Result
    {
        public Nullable<System.DateTime> as_of_date { get; set; }
        public Nullable<int> account_id { get; set; }
        public string account_code { get; set; }
        public string short_name { get; set; }
        public string long_name { get; set; }
        public string status { get; set; }
        public string tax_status { get; set; }
        public Nullable<System.DateTime> open_date { get; set; }
        public Nullable<System.DateTime> close_date { get; set; }
        public string base_ccy_ISO_2 { get; set; }
        public string base_ccy_ISO_3 { get; set; }
        public string account_class { get; set; }
        public Nullable<int> security_id { get; set; }
        public string symbol { get; set; }
        public string security_name { get; set; }
        public string bond_description { get; set; }
        public string currency_of_exch { get; set; }
        public string currency_of_exch_3 { get; set; }
        public Nullable<double> local_price { get; set; }
        public Nullable<double> base_price { get; set; }
        public string security_type { get; set; }
        public Nullable<int> lotnum_OR_numlots { get; set; }
        public Nullable<bool> is_short { get; set; }
        public Nullable<bool> is_unsupervised { get; set; }
        public Nullable<System.DateTime> pos_purchase_date { get; set; }
        public Nullable<double> pos_quantity { get; set; }
        public Nullable<double> pos_unadjusted_quantity { get; set; }
        public Nullable<double> base_market_value { get; set; }
        public Nullable<double> base_accrued_interest { get; set; }
        public Nullable<double> base_annual_income { get; set; }
        public Nullable<double> base_unrealized_gl { get; set; }
        public Nullable<double> base_unrealized_gl_fx { get; set; }
        public Nullable<double> base_cost_basis { get; set; }
        public Nullable<double> base_unadjusted_cost_basis { get; set; }
        public Nullable<double> base_unit_cost { get; set; }
        public Nullable<double> base_unadjusted_unit_cost { get; set; }
        public Nullable<double> yield { get; set; }
        public Nullable<double> maturity { get; set; }
        public Nullable<double> duration { get; set; }
        public Nullable<double> coupon { get; set; }
        public Nullable<double> ytm_cost { get; set; }
        public Nullable<double> ytm_mv { get; set; }
        public Nullable<double> ytw_mv { get; set; }
    }
}
