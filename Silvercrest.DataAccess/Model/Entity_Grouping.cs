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
    
    public partial class Entity_Grouping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entity_Grouping()
        {
            this.Classifications = new HashSet<Classification>();
            this.Classifications1 = new HashSet<Classification>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classification> Classifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classification> Classifications1 { get; set; }
    }
}