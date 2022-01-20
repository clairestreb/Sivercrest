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
    
    public partial class Account_Snapshot
    {
        public System.DateTime as_of_date { get; set; }
        public int account_id { get; set; }
        public string account_code { get; set; }
        public string long_name { get; set; }
        public string status { get; set; }
        public int account_type_id { get; set; }
        public string tax_status { get; set; }
        public string tax_number { get; set; }
        public Nullable<System.DateTime> open_date { get; set; }
        public Nullable<System.DateTime> close_date { get; set; }
        public int close_reason_id { get; set; }
        public int account_class_id { get; set; }
        public Nullable<int> relationship_id { get; set; }
        public Nullable<int> family_group_id { get; set; }
        public int custodian_id { get; set; }
        public int asset_manager_id { get; set; }
        public Nullable<int> firm_user_group_id_owner { get; set; }
        public Nullable<int> firm_user_group_id_manager { get; set; }
        public bool is_institution { get; set; }
        public bool is_new_client { get; set; }
        public bool is_existing_funds { get; set; }
        public bool aum_exclude { get; set; }
        public Nullable<int> recon_ipn_status { get; set; }
        public string eq_strategy { get; set; }
        public string fi_strategy { get; set; }
        public Nullable<int> contact_id { get; set; }
        public string prefix { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string suffix { get; set; }
        public string company_name { get; set; }
        public string long_name_2 { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string domicile_state { get; set; }
    
        public virtual Account_Class Account_Class { get; set; }
        public virtual Account_Close_Reason Account_Close_Reason { get; set; }
        public virtual Account_Type Account_Type { get; set; }
        public virtual Asset_Manager Asset_Manager { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Source Source { get; set; }
        public virtual Family_Group Family_Group { get; set; }
        public virtual Relationship Relationship { get; set; }
    }
}
