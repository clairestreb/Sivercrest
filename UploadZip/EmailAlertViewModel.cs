using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadZip
{
    class EmailAlertViewModel
    {
        public int Id { get; set; }
        public string  Recipient { get; set; }
        public string FileId { get; set; }
        public string DisplayName { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string ClientName { get; set; }

    }
}
