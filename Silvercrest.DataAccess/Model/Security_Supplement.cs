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
    
    public partial class Security_Supplement
    {
        public string sedol { get; set; }
        public string isin { get; set; }
        public string cusip { get; set; }
        public int currency_id { get; set; }
        public Nullable<int> security_id { get; set; }
        public string name { get; set; }
    }
}
