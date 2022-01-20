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
    
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.Book_Request_Detail = new HashSet<Book_Request_Detail>();
            this.Report_Param_Access = new HashSet<Report_Param_Access>();
            this.Report_Param_Mapping = new HashSet<Report_Param_Mapping>();
            this.Template_Report = new HashSet<Template_Report>();
        }
    
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string rptdesign { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string exclude_condition { get; set; }
        public int display_order { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book_Request_Detail> Book_Request_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report_Param_Access> Report_Param_Access { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report_Param_Mapping> Report_Param_Mapping { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Template_Report> Template_Report { get; set; }
    }
}