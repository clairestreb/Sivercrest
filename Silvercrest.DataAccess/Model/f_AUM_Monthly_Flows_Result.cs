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
    
    public partial class f_AUM_Monthly_Flows_Result
    {
        public Nullable<System.DateTime> as_of_date { get; set; }
        public Nullable<int> mgr_id { get; set; }
        public string mgr_code { get; set; }
        public Nullable<int> prev_mgr_id { get; set; }
        public string prev_mgr_code { get; set; }
        public Nullable<int> asset_manager_id { get; set; }
        public string asset_manager { get; set; }
        public Nullable<int> account_class_id { get; set; }
        public Nullable<int> account_id { get; set; }
        public string account_code { get; set; }
        public Nullable<bool> is_unsupervised { get; set; }
        public string transaction_type { get; set; }
        public string transaction_code { get; set; }
        public Nullable<System.DateTime> trade_date { get; set; }
        public Nullable<double> new_accounts_flow { get; set; }
        public Nullable<double> closed_accounts_flow { get; set; }
        public Nullable<double> existing_accounts_flow { get; set; }
    }
}
