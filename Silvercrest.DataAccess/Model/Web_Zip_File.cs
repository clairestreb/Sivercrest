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
    
    public partial class Web_Zip_File
    {
        public int id { get; set; }
        public string file_name { get; set; }
        public System.DateTime upload_date { get; set; }
        public Nullable<int> number_of_documents { get; set; }
        public Nullable<System.DateTime> process_date { get; set; }
    }
}