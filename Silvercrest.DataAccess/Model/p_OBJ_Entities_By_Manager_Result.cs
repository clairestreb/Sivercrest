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
    
    public partial class p_OBJ_Entities_By_Manager_Result
    {
        public string manager_code { get; set; }
        public string entity_code { get; set; }
        public string entity_name { get; set; }
        public string goal { get; set; }
        public System.DateTime objective_date { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> ins_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }
        public int display_order { get; set; }
    }
}