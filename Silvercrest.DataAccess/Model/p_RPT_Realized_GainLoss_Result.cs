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
    
    public partial class p_RPT_Realized_GainLoss_Result
    {
        public Nullable<int> SUPV_SORT { get; set; }
        public Nullable<int> SRT { get; set; }
        public Nullable<int> CSRT { get; set; }
        public string grouping1_name { get; set; }
        public string grouping2_name { get; set; }
        public Nullable<int> account_sort { get; set; }
        public string a_g_code { get; set; }
        public string long_name { get; set; }
        public Nullable<System.DateTime> open_date { get; set; }
        public Nullable<System.DateTime> close_date { get; set; }
        public string ISO_Code_3 { get; set; }
        public Nullable<double> quantity { get; set; }
        public Nullable<double> cost_basis { get; set; }
        public Nullable<double> amortization { get; set; }
        public Nullable<double> adj_cost_basis { get; set; }
        public Nullable<double> proceeds { get; set; }
        public Nullable<double> realized_gl_st { get; set; }
        public Nullable<double> realized_gl_lt { get; set; }
        public Nullable<double> realized_gl_5y { get; set; }
        public string symbol { get; set; }
        public string security_name { get; set; }
        public string bond_description { get; set; }
        public string cov_uncov { get; set; }
        public Nullable<int> record_count { get; set; }
    }
}