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
    
    public partial class Staging_Intraday_DW_Refresh
    {
        public bool is_active { get; set; }
        public System.DateTime as_of_date { get; set; }
        public int entity_id { get; set; }
        public bool is_group { get; set; }
        public Nullable<System.DateTime> refresh_date { get; set; }
    }
}