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
    
    public partial class Web_Secondary_Team
    {
        public int secondary_firm_user_group_id { get; set; }
        public int primary_firm_user_group_id { get; set; }
        public int family_group_id { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    
        public virtual Family_Group Family_Group { get; set; }
        public virtual Firm_User_Group Firm_User_Group { get; set; }
        public virtual Firm_User_Group Firm_User_Group1 { get; set; }
    }
}
