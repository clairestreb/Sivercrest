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
    
    public partial class p_Web_Contact_Entities_Result
    {
        public Nullable<System.DateTime> as_of_date { get; set; }
        public Nullable<int> contact_id { get; set; }
        public Nullable<int> entity_id { get; set; }
        public Nullable<bool> is_group { get; set; }
        public Nullable<bool> is_client_group { get; set; }
        public string entity_code { get; set; }
        public string display_name { get; set; }
        public Nullable<double> total_value { get; set; }
        public Nullable<double> pct { get; set; }
        public Nullable<int> sort_order { get; set; }
        public Nullable<bool> is_default { get; set; }
    }
}
