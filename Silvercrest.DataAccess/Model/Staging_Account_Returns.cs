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
    
    public partial class Staging_Account_Returns
    {
        public bool is_active { get; set; }
        public System.DateTime from_date { get; set; }
        public System.DateTime thru_date { get; set; }
        public int account_id { get; set; }
        public int currency_id { get; set; }
        public int classification_id { get; set; }
        public Nullable<int> is_gross { get; set; }
        public Nullable<double> end_market_value { get; set; }
        public Nullable<double> TWR { get; set; }
        public Nullable<double> cumulative_TWR { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    }
}
