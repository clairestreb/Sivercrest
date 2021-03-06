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
    
    public partial class Firm_User_Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Firm_User_Group()
        {
            this.Account_Group = new HashSet<Account_Group>();
            this.Account_User_Group = new HashSet<Account_User_Group>();
            this.APX_Portfolio_Statement = new HashSet<APX_Portfolio_Statement>();
            this.Contacts = new HashSet<Contact>();
            this.Firm_User_Group_Attribute = new HashSet<Firm_User_Group_Attribute>();
            this.Firm_User_Group_Member = new HashSet<Firm_User_Group_Member>();
            this.Web_Secondary_Team = new HashSet<Web_Secondary_Team>();
            this.Revenue_Snapshot = new HashSet<Revenue_Snapshot>();
            this.Web_Secondary_Team1 = new HashSet<Web_Secondary_Team>();
            this.Transaction_Inter_Account = new HashSet<Transaction_Inter_Account>();
            this.User_Group_Member = new HashSet<User_Group_Member>();
        }
    
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public int branch_office_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Group> Account_Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_User_Group> Account_User_Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APX_Portfolio_Statement> APX_Portfolio_Statement { get; set; }
        public virtual AUM_SAMG_Manager_Mapping AUM_SAMG_Manager_Mapping { get; set; }
        public virtual Branch_Office Branch_Office { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Firm_User_Group_Attribute> Firm_User_Group_Attribute { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Firm_User_Group_Member> Firm_User_Group_Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Secondary_Team> Web_Secondary_Team { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Revenue_Snapshot> Revenue_Snapshot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Secondary_Team> Web_Secondary_Team1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Inter_Account> Transaction_Inter_Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Group_Member> User_Group_Member { get; set; }
    }
}
