using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class Document
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string AsOf { get; set; }
        public string UploadedBy { get; set; }
        public string DocumentType { get; set; }
        public string EntityName { get; set; }
        public int? Year { get; set; }
        public int? DocumentTypeId { get; set; }
        public string UploadDate { get; set; }
        public bool IsGroup { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSamg { get; set; }
        public int AsOfSort { get; set; }
        public bool IsStrategy { get; set; }
    }
}
