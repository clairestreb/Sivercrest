using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Silvercrest.Entities
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SourceIdd { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        public string EmailAddress3 { get; set; }
        public int? DefaultPhoneId { get; set; }
        public int? DefaultAddressId { get; set; }
        public string InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public int FirmUserGroupId { get; set; }
        public string Prefix { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Account> Accounts { get; set; }
        public virtual Family Family { get; set; }
    }
}
