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
    
    public partial class Small_Cap_Randomized_Brokers
    {
        public int trade_id { get; set; }
        public int broker_id { get; set; }
        public int random_order { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual Small_Cap_Broker Small_Cap_Broker { get; set; }
        public virtual Small_Cap_Trade Small_Cap_Trade { get; set; }
    }
}