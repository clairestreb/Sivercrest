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
    
    public partial class Web_Document_Deleted
    {
        public int id { get; set; }
        public string file_id { get; set; }
        public string display_name { get; set; }
        public Nullable<int> entity_id { get; set; }
        public Nullable<int> is_group { get; set; }
        public int document_type_id { get; set; }
        public System.DateTime document_date { get; set; }
        public Nullable<int> template_id { get; set; }
        public Nullable<int> contact_id { get; set; }
        public Nullable<int> upload_contact_id { get; set; }
        public System.DateTime upload_date { get; set; }
        public string delete_by { get; set; }
        public System.DateTime delete_date { get; set; }
    
        public virtual Web_Document_Type Web_Document_Type { get; set; }
    }
}
