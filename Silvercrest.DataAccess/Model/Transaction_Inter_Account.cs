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
    
    public partial class Transaction_Inter_Account
    {
        public int firm_user_group_id { get; set; }
        public int family_group_id { get; set; }
        public int source_id { get; set; }
        public int transaction_id { get; set; }
        public int account_id { get; set; }
        public System.DateTime trade_date { get; set; }
    
        public virtual Family_Group Family_Group { get; set; }
        public virtual Firm_User_Group Firm_User_Group { get; set; }
    }
}
