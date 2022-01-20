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
    
    public partial class Small_Cap_Trade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Small_Cap_Trade()
        {
            this.Small_Cap_Randomized_Brokers = new HashSet<Small_Cap_Randomized_Brokers>();
            this.Small_Cap_Randomized_Broker_Types = new HashSet<Small_Cap_Randomized_Broker_Types>();
        }
    
        public int id { get; set; }
        public int security_id { get; set; }
        public string trade_side { get; set; }
        public System.DateTime trade_start { get; set; }
        public Nullable<System.DateTime> trade_end { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    
        public virtual Security Security { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Small_Cap_Randomized_Brokers> Small_Cap_Randomized_Brokers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Small_Cap_Randomized_Broker_Types> Small_Cap_Randomized_Broker_Types { get; set; }
    }
}