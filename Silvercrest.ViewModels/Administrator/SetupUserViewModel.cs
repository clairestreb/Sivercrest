using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels
{
    public class SetupUserViewModel:GenericViewModel
    {
        public List<Contact> Contacts { get; set; }
        public string[] Emails { get; set; }
    }
}
