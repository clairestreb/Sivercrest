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
    
    public partial class Fee_Snapshot
    {
        public System.DateTime from_date { get; set; }
        public System.DateTime thru_date { get; set; }
        public int account_id { get; set; }
        public int security_id { get; set; }
        public double gross_fee { get; set; }
        public double management_fee { get; set; }
        public double reporting_fee { get; set; }
        public double administration_fee { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
