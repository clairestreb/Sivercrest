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
    
    public partial class Thomson_Reuters
    {
        public string instrument_id { get; set; }
        public string id_type { get; set; }
        public string security_name { get; set; }
        public string bid_price { get; set; }
        public string mid_price { get; set; }
        public string ask_price { get; set; }
        public string close_price { get; set; }
        public string as_of_date { get; set; }
        public Nullable<int> security_id { get; set; }
    }
}
