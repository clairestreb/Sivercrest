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
    
    public partial class Staging_Contact_Address
    {
        public bool is_active { get; set; }
        public int contact_id { get; set; }
        public int address_type_id { get; set; }
        public string street_1 { get; set; }
        public string street_2 { get; set; }
        public string street_3 { get; set; }
        public string street_4 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    }
}
