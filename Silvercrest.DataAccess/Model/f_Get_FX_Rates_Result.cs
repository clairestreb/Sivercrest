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
    
    public partial class f_Get_FX_Rates_Result
    {
        public System.DateTime as_of_date { get; set; }
        public int currency_id_from { get; set; }
        public int currency_id_to { get; set; }
        public Nullable<double> spot_rate { get; set; }
        public Nullable<double> bid_30_date { get; set; }
        public Nullable<double> bid_60_date { get; set; }
        public Nullable<double> bid_90_date { get; set; }
        public Nullable<double> bid_180_date { get; set; }
        public Nullable<double> bid_360_date { get; set; }
    }
}
