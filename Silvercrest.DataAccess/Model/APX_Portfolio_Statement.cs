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
    
    public partial class APX_Portfolio_Statement
    {
        public int account_group_id { get; set; }
        public int firm_user_group_id { get; set; }
        public Nullable<bool> is_monthly { get; set; }
        public Nullable<bool> is_quarterly { get; set; }
    
        public virtual Account_Group Account_Group { get; set; }
        public virtual Firm_User_Group Firm_User_Group { get; set; }
    }
}