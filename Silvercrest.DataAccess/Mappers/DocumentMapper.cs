using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class DocumentMapper
    {
        public static List<Silvercrest.Entities.Document> MapDocumentsList(IList<p_Web_Documents_By_Contact_Result> list)
        {
            var mappedDocumentList = new List<Silvercrest.Entities.Document>();
            foreach (var document in list)
            {
                var mappedDocument = new Silvercrest.Entities.Document();
                mappedDocument.Id = document.file_id;
                mappedDocument.DocumentType = document.document_type;
                //mappedDocument.Year = document.yyyy;
                var date = document.document_date;
                mappedDocument.AsOf = date.Value.ToString("MM'/'dd'/'yyyy");
                mappedDocument.DocumentTypeId = document.document_type_id;
                mappedDocument.EntityName = document.entity_name;
                mappedDocument.FileName = document.display_name;
                mappedDocument.UploadedBy = document.upload_by;
                //mappedDocument.UploadDate = document.upload_time.Value.ToString();
                mappedDocument.IsGroup = document.is_group.Value;
                mappedDocument.IsSamg = document.is_samg_doc.Value;

                mappedDocumentList.Add(mappedDocument);
            }
            return mappedDocumentList;
        }

    }
}
