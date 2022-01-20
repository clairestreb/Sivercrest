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
    
    public partial class f_SSIS_Security_Result
    {
        public int id { get; set; }
        public string symbol_type_code { get; set; }
        public Nullable<int> security_type_id { get; set; }
        public string symbol { get; set; }
        public string cusip { get; set; }
        public string ticker { get; set; }
        public string isin { get; set; }
        public string sedol { get; set; }
        public string proprietary { get; set; }
        public string name { get; set; }
        public Nullable<int> country_id_dom { get; set; }
        public Nullable<int> state_id { get; set; }
        public Nullable<int> currency_id_dom { get; set; }
        public Nullable<int> currency_id_exch { get; set; }
        public Nullable<int> exchange_id { get; set; }
        public Nullable<double> coupon_rate { get; set; }
        public Nullable<System.DateTime> maturity_date { get; set; }
        public Nullable<int> payment_frequency { get; set; }
        public Nullable<int> underlying_security_id { get; set; }
        public Nullable<double> valuation_factor { get; set; }
        public Nullable<int> source_id { get; set; }
    }
}
