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
    
    public partial class p_RPT_SAMG_Strategy_Tracker_Result
    {
        public Nullable<System.DateTime> as_of_date { get; set; }
        public string manager_code { get; set; }
        public string strategy { get; set; }
        public Nullable<int> model_id { get; set; }
        public string model_code { get; set; }
        public Nullable<int> model_ct { get; set; }
        public Nullable<double> model_eq_wt { get; set; }
        public Nullable<int> account_id { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public Nullable<double> account_market_value { get; set; }
        public Nullable<int> acct_count_csml { get; set; }
        public Nullable<double> account_value_csml { get; set; }
        public Nullable<int> acct_model_count { get; set; }
        public Nullable<double> acct_model_value { get; set; }
        public Nullable<double> acct_model_wt_cs_ml { get; set; }
        public Nullable<double> cs_ml_total { get; set; }
        public Nullable<double> equity_total { get; set; }
        public Nullable<double> count_compliance { get; set; }
        public Nullable<double> value_compliance { get; set; }
        public Nullable<double> compliance_threshold { get; set; }
        public string ISO_Code_3 { get; set; }
    }
}
