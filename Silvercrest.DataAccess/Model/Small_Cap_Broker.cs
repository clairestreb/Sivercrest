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
    
    public partial class Small_Cap_Broker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Small_Cap_Broker()
        {
            this.Small_Cap_Randomized_Brokers = new HashSet<Small_Cap_Randomized_Brokers>();
        }
    
        public int id { get; set; }
        public Nullable<int> broker_type_id { get; set; }
        public string name { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual Small_Cap_Broker_Type Small_Cap_Broker_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Small_Cap_Randomized_Brokers> Small_Cap_Randomized_Brokers { get; set; }
    }
}
