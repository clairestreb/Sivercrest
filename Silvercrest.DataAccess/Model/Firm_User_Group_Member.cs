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
    
    public partial class Firm_User_Group_Member
    {
        public int firm_user_group_id { get; set; }
        public int contact_id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int firm_title_id { get; set; }
        public int sort_order { get; set; }
        public string photo { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Firm_Title Firm_Title { get; set; }
        public virtual Firm_User_Group Firm_User_Group { get; set; }
    }
}