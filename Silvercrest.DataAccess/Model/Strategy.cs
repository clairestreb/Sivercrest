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
    
    public partial class Strategy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Strategy()
        {
            this.Account_Strategy_Override = new HashSet<Account_Strategy_Override>();
            this.Security_Strategy = new HashSet<Security_Strategy>();
            this.SMA_Strategy = new HashSet<SMA_Strategy>();
            this.Strategy_Allocation_Target = new HashSet<Strategy_Allocation_Target>();
            this.Strategy_Drilldown = new HashSet<Strategy_Drilldown>();
            this.Strategy_Drilldown1 = new HashSet<Strategy_Drilldown>();
            this.Strategy_Rollup = new HashSet<Strategy_Rollup>();
            this.Strategy_Rollup1 = new HashSet<Strategy_Rollup>();
            this.Strategy_Translate = new HashSet<Strategy_Translate>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int classification_id { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public int sort_order { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Strategy_Override> Account_Strategy_Override { get; set; }
        public virtual Classification Classification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Security_Strategy> Security_Strategy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SMA_Strategy> SMA_Strategy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Allocation_Target> Strategy_Allocation_Target { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Drilldown> Strategy_Drilldown { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Drilldown> Strategy_Drilldown1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Rollup> Strategy_Rollup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Rollup> Strategy_Rollup1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Strategy_Translate> Strategy_Translate { get; set; }
    }
}
