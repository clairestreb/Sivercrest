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
    
    public partial class Web_Page_Visit_Record
    {
        public int id { get; set; }
        public string web_user_id { get; set; }
        public string route { get; set; }
        public int visit_count { get; set; }
        public int page_type { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}