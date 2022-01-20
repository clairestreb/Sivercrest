using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.ViewModels.Client.Documents;
using System.IO;
using System.Web.Mvc;
using Silvercrest.Entities;

namespace Silvercrest.Interfaces.Client
{
    public interface IDocuments
    {
        DocumentsViewModel GetDocuments(int? contactId);
        bool SetDocument(string fileId, string fileName, int? contactId, int? entityId);
        Task<bool> Upload(byte[] array, string fileName, int? contactId, int? mainContactId);
        Task ProceedZips();
        void RemoveDocument(string fileId, string userName);
        void MakeDocumentFavorite(int? contactId, string fileId, bool isStrategy, bool isFavorite, string userName, bool isSamg);
        Task<Document> GetDocumentByFileIdAsync(string fileId);
    }
}
