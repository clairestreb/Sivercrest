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
    
    public partial class Staging_FX_Rate
    {
        public bool is_active { get; set; }
        public System.DateTime as_of_date { get; set; }
        public int currency_id_from { get; set; }
        public int currency_id_to { get; set; }
        public double spot_rate { get; set; }
        public Nullable<double> bid_30_day { get; set; }
        public Nullable<double> bid_60_day { get; set; }
        public Nullable<double> bid_90_day { get; set; }
        public Nullable<double> bid_180_day { get; set; }
        public Nullable<double> bid_360_day { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    }
}
