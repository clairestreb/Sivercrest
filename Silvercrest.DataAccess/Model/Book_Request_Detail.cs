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
    
    public partial class Book_Request_Detail
    {
        public int book_request_id { get; set; }
        public int report_id { get; set; }
        public int collate_order { get; set; }
        public int report_param_id { get; set; }
        public string report_param_value { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual Book_Request Book_Request { get; set; }
        public virtual Report Report { get; set; }
        public virtual Report_Param Report_Param { get; set; }
    }
}