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
    
    public partial class Staging_Market_Index_Return
    {
        public string market_index_name { get; set; }
        public System.DateTime month_end { get; set; }
        public string benchmark_name { get; set; }
        public double index_return { get; set; }
        public Nullable<int> market_index_id { get; set; }
        public Nullable<int> benchmark_id { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
    }
}