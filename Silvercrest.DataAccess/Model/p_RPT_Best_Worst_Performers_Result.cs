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
    
    public partial class p_RPT_Best_Worst_Performers_Result
    {
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<int> account_id { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public Nullable<int> security_id { get; set; }
        public string symbol { get; set; }
        public string security_name { get; set; }
        public string bond_description { get; set; }
        public Nullable<System.DateTime> from_date { get; set; }
        public Nullable<System.DateTime> close_date { get; set; }
        public Nullable<double> pct_portfolio { get; set; }
        public Nullable<double> GL_Period { get; set; }
        public Nullable<double> Pct_Gain { get; set; }
    }
}
