using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.Utilities;
using Silvercrest.ViewModels.Client.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class DocumentRepository
    {
        private SLVR_DEVEntities _context;
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private ProcedureAdoHelper _helper = new ProcedureAdoHelper();

        public DocumentRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public int? SetDocument(string fileId, string fileName, int? contactId, int? entityId)
        {
            int result = _context.p_Web_Document_Index(fileId, fileName, contactId, entityId);
            return result;
        }

        public void DeleteDocument(DocumentsViewModel view)
        {
            var fileToDelete = _context.Web_Document.Where(x => x.contact_id == view.contactId).FirstOrDefault();

            _context.Web_Document.Remove(fileToDelete);
            _context.SaveChanges();
        }

        public DataSet MakeDocumentFavorite(int? contactId, string fileId, bool isStrategy, bool isFavorite, bool isSamg, string userName)
        {
            DataSet data = new DataSet();
            if (isSamg)
            {
                data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_SAMG_Document_Favorite", contactId, fileId, isFavorite, userName);
            }
            if (isStrategy == false && !isSamg)
            {
                data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Document_Favorite", contactId, fileId, isFavorite, userName);
            }
            if (isStrategy == true && !isSamg)
            {
                data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Strategy_Document_Favorite", contactId, fileId, isFavorite, userName);
            }
            return data;
        }

        public DataSet RemoveDocument(string fileId, string userName)
        {
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Document_Delete", fileId, userName);
            return data;
        }

        public DocumentsViewModel GetDocument(int? contactId)
        {
            /*
            List<p_Web_Documents_By_Contact_Result> result = _context.p_Web_Documents_By_Contact(contactId).ToList();
            var model = new DocumentsViewModel
            {
                Year = result.Select(x => x.yyyy).Distinct().ToList(),
                DocumentType = result.Select(x => x.document_type).Distinct().ToList(),
                EntityName = result.Select(x => x.entity_name).Distinct().ToList(),
                Documents = DocumentMapper.MapDocumentsList(result)
            };
            return model;
            */
            DataSet data = _helper.ExecuteStoredProcedure(connectionString, "p_Web_Documents_By_Contact", contactId);
            var model = new DocumentsViewModel();
            DataRowCollection rows;

            //Entity Name
            List<string> entities = new List<string>();
            rows = data.Tables[1].Rows;
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    entities.Add(row["entity_name"].ToString());
                }
            }
            model.EntityName = entities;

            //Document Type
            List<string> docType = new List<string>();
            rows = data.Tables[2].Rows;
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    docType.Add(row["document_type"].ToString());
                }
            }
            model.DocumentType = docType;

            //Year YYYY
            //List<int?> yyyy = new List<int?>();
            //rows = data.Tables[2].Rows;
            //if (rows.Count > 0)
            //{
            //    foreach (DataRow row in rows)
            //    {
            //        yyyy.Add((int?)row["yyyy"]);
            //    }
            //}
            //model.Year = yyyy;

            ////Document Date
            //List<string> asOf = new List<string>();
            //rows = data.Tables[0].Rows;
            //if (rows.Count > 0)
            //{
            //    foreach (DataRow row in rows)
            //    {
            //        asOf.Add(row["document_date"].ToString());
            //    }
            //}
            //model.AsOf = asOf;

            ////Upload date
            //List<string> uploadDate = new List<string>();
            //rows = data.Tables[0].Rows;
            //if (rows.Count > 0)
            //{
            //    foreach (DataRow row in rows)
            //    {
            //        uploadDate.Add(row["upload_time"].ToString());
            //    }
            //}
            //model.UploadDate = uploadDate;

            //IsGroup
            List<bool> isGroup = new List<bool>();
            rows = data.Tables[1].Rows;
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    isGroup.Add((bool)row["is_group"]);
                }
            }
            model.IsGroup = isGroup;

            ////is_favorite
            //List<bool> isFavorite = new List<bool>();
            //rows = data.Tables[0].Rows;
            //if (rows.Count > 0)
            //{
            //    foreach (DataRow row in rows)
            //    {
            //        isFavorite.Add((bool)row["is_favorite"]);
            //    }
            //}
            //model.IsFavorite = isFavorite;

            //Documents
            List<Document> docs = new List<Document>();
            rows = data.Tables[0].Rows;
            Document doc = null;
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    doc = new Document();
                    doc.Id = row["file_id"].ToString();
                    doc.FileName = row["display_name"].ToString();
                    doc.EntityName = row["entity_name"].ToString();
                    doc.DocumentTypeId = (int)row["document_type_id"];
                    doc.DocumentType = row["document_type"].ToString();
                    doc.AsOf = ((DateTime)row["document_date"]).ToString("MM'/'dd'/'yyyy");
                    doc.UploadedBy = row["upload_by"].ToString();
                    doc.IsGroup = ((bool)row["is_group"]);
                    doc.IsFavorite = ((bool)row["is_favorite"]);
                    doc.IsSamg = ((bool)row["is_samg_doc"]);
                    int parseAsOfSort;
                    var resParse = Int32.TryParse((row["as_of_sort"]).ToString(), out parseAsOfSort);
                    if (resParse == true)
                    {
                        doc.AsOfSort = parseAsOfSort;
                    }
                    doc.IsStrategy = ((bool)row["is_strategy"]);

                    docs.Add(doc);
                }
            }
            model.Documents = docs;

            return model;
        }


        public async Task<Document> GetDocumentByFileIdAsync(string fileId)
        {
            string query = $"SELECT * FROM Web_Document WHERE file_id = '{fileId}'";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var adapter = new SqlDataAdapter(query, sqlConnection);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);
                var document = new Document();

                if (dataSet.Tables["Table"].Rows.Count <= default(int))
                {
                    return null;
                }

                var row = dataSet.Tables["Table"].Rows[0];

                document.Id = row["id"].ToString();
                document.FileName = row["display_name"].ToString();

                sqlConnection.Close();
                
                return document;
            }
        }
    }

}
