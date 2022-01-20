using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Documents
{
    public class DocumentsViewModel : GenericViewModel
    {
        public int contactId { get; set; }
        public List<Document> Documents { get; set; }
        //public List<int?> Year { get; set; }
        public List<string> DocumentType { get; set; }
        public List<string> EntityName { get; set; }
        public int AsOf { get; set; }

        //public List<string> AsOf { get; set; }
        //public List<string> UploadDate { get; set; }
        public List<bool> IsGroup { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //public List<bool> IsDeleted { get; set; }
        //public List<bool> IsFavorite { get; set; }
        public bool HasPermission { get; set; }
    }
}


