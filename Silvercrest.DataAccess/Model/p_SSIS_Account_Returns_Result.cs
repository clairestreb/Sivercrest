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
    
    public partial class p_SSIS_Account_Returns_Result
    {
        public Nullable<long> rowid { get; set; }
        public System.DateTime from_date { get; set; }
        public System.DateTime thru_date { get; set; }
        public int account_id { get; set; }
        public string account_code { get; set; }
        public int currId { get; set; }
        public string CurrCode { get; set; }
        public int ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public int is_gross { get; set; }
        public Nullable<double> end_market_value { get; set; }
        public Nullable<double> TWR { get; set; }
        public Nullable<double> cumulative_TWR { get; set; }
    }
}
