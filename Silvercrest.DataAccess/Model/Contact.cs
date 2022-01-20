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
    
    public partial class Contact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contact()
        {
            this.Account_Snapshot = new HashSet<Account_Snapshot>();
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.Contact_Address = new HashSet<Contact_Address>();
            this.Contact_Phone = new HashSet<Contact_Phone>();
            this.Entity_Document = new HashSet<Entity_Document>();
            this.Firm_User_Group_Member = new HashSet<Firm_User_Group_Member>();
            this.Related_Party = new HashSet<Related_Party>();
            this.Statement_Delivery_Queue = new HashSet<Statement_Delivery_Queue>();
            this.Web_Account_Group = new HashSet<Web_Account_Group>();
            this.Web_Document_Favorite = new HashSet<Web_Document_Favorite>();
            this.Web_Strategy_Document_Favorite = new HashSet<Web_Strategy_Document_Favorite>();
            this.Web_Supplemental_Access = new HashSet<Web_Supplemental_Access>();
        }
    
        public int id { get; set; }
        public string code { get; set; }
        public int source_id { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string suffix { get; set; }
        public string email_address_1 { get; set; }
        public string email_address_2 { get; set; }
        public string email_address_3 { get; set; }
        public Nullable<int> default_phone_id { get; set; }
        public Nullable<int> default_address_id { get; set; }
        public string insert_by { get; set; }
        public System.DateTime insert_date { get; set; }
        public string update_by { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string company_name { get; set; }
        public string username { get; set; }
        public int firm_user_group_id { get; set; }
        public string prefix { get; set; }
        public bool active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Snapshot> Account_Snapshot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact_Address> Contact_Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact_Phone> Contact_Phone { get; set; }
        public virtual Firm_User_Group Firm_User_Group { get; set; }
        public virtual Source Source { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entity_Document> Entity_Document { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Firm_User_Group_Member> Firm_User_Group_Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Related_Party> Related_Party { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Statement_Delivery_Queue> Statement_Delivery_Queue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Account_Group> Web_Account_Group { get; set; }
        public virtual Web_Contact_Default_Entity Web_Contact_Default_Entity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Document_Favorite> Web_Document_Favorite { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Strategy_Document_Favorite> Web_Strategy_Document_Favorite { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Web_Supplemental_Access> Web_Supplemental_Access { get; set; }
        public virtual Web_User_Role Web_User_Role { get; set; }
        public virtual Web_User_Settings Web_User_Settings { get; set; }
    }
}
