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
    
    public partial class p_VLD_Account_Strategy_Benchmark_Result
    {
        public Nullable<System.DateTime> as_of_date { get; set; }
        public string manager_code { get; set; }
        public int account_id { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string asset_class { get; set; }
        public string strategy { get; set; }
        public Nullable<double> asset_class_value { get; set; }
        public Nullable<double> asset_class_pct { get; set; }
        public string bm_name { get; set; }
        public int index_rank { get; set; }
    }
}
