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
    
    public partial class Web_Email_Alert_Documents
    {
        public int alert_id { get; set; }
        public int web_document_id { get; set; }
        public System.DateTime insert_date { get; set; }
    
        public virtual Web_Email_Alerts Web_Email_Alerts { get; set; }
    }
}