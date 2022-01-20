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
    
    public partial class Portfolio_Benchmark
    {
        public int account_group_id { get; set; }
        public int benchmark_id { get; set; }
        public int classification_id { get; set; }
        public int rank { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public bool is_manual { get; set; }
    
        public virtual Account_Group Account_Group { get; set; }
        public virtual Benchmark Benchmark { get; set; }
        public virtual Classification Classification { get; set; }
    }
}
