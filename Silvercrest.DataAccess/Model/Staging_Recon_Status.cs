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
    
    public partial class Staging_Recon_Status
    {
        public bool is_active { get; set; }
        public System.DateTime as_of_date { get; set; }
        public int account_id { get; set; }
        public string recon_frequency { get; set; }
        public bool is_reconciled { get; set; }
        public string reconciled_by { get; set; }
        public string comments { get; set; }
        public Nullable<System.DateTime> perf_calc_date { get; set; }
        public Nullable<System.DateTime> statement_received { get; set; }
        public System.DateTime insert_date { get; set; }
    }
}
