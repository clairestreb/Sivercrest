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
    
    public partial class Staging_Transaction_Comment
    {
        public bool is_active { get; set; }
        public int source_id { get; set; }
        public int transaction_id { get; set; }
        public int account_id { get; set; }
        public int sequence_num { get; set; }
        public System.DateTime trade_date { get; set; }
        public string comment { get; set; }
        public string insert_by { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string delete_by { get; set; }
        public Nullable<System.DateTime> delete_date { get; set; }
    }
}
