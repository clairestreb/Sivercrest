using Silvercrest.Interfaces.Client;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.ViewModels.Client.Documents;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.DataAccess.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using System.IO;
using System.IO.Compression;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Silvercrest.Entities;

namespace Silvercrest.Services.Client
{
    public class DocumentsService : IDocuments
    {
        private DocumentRepository _repository;

        public DocumentsService(SLVR_DEVEntities context)
        {
            _repository = new DocumentRepository(context);
        }

        public DocumentsViewModel GetDocuments(int? contactId)
        {
            return _repository.GetDocument(contactId);
        }

        public void RemoveDocument(string fileId, string userName)
        {
            _repository.RemoveDocument(fileId, userName);
        }

        public void MakeDocumentFavorite(int? contactId, string fileId, bool isStrategy, bool isFavorite, string userName, bool isSamg)
        {
            _repository.MakeDocumentFavorite(contactId, fileId, isStrategy, isFavorite, isSamg, userName);
        }

        public bool SetDocument(string fileId, string fileName, int? contactId, int? entityId)
        {
            if (_repository.SetDocument(fileId, fileName, contactId, entityId) > 0)
            {
                return true;
            }
            return false;
        }



        public async Task<bool> Upload(byte[] array, string fileName, int? contactId, int? mainContactId)
        {
            var fileId = Guid.NewGuid().ToString();
            int transactionResult = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("p_Web_Document_Index", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("file_id", SqlDbType.NVarChar).Value = fileId;
                    cmd.Parameters.Add("file_name", SqlDbType.NVarChar).Value = fileName;
                    cmd.Parameters.Add("contact_id", SqlDbType.Int).Value = contactId.HasValue ? contactId : null;
                    cmd.Parameters.Add("upload_contact_id", SqlDbType.Int).Value = mainContactId.HasValue ? mainContactId : null;
                    SqlParameter retValue = cmd.Parameters.Add("return", SqlDbType.Int);
                    retValue.Direction = ParameterDirection.ReturnValue;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    transactionResult = (int)retValue.Value;
                }
            }
            if (transactionResult > 0)
            {
                var folderName = transactionResult.ToString();
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();
                CloudFileShare share = cloudFileClient.GetShareReference(WebConfigurationManager.AppSettings["cloudShare"]);
                if (share.Exists())
                {
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                    CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference(folderName);
                    var creationResult = cloudFileDirectory.CreateIfNotExists();
                    CloudFile cloudFile = cloudFileDirectory.GetFileReference(fileId);
                    await cloudFile.UploadFromByteArrayAsync(array, 0, array.Count());
                    //await cloudFile.UploadFromStreamAsync(fileStream);
                    cloudFile.Metadata.Add("initialFileName", fileName);
                    cloudFile.SetMetadata();
                }
                return true;
            }
            else
                return false;
        }

        public async Task ProceedZips()
        {
            using (StreamWriter outputFile = new StreamWriter(@"C:\FTP Contents\Log.txt"))
            {
                outputFile.Write("Start");
                bool result;
                //DirectoryInfo dir = new DirectoryInfo(new DirectoryInfo(HttpRuntime.AppDomainAppPath).Parent.FullName + "\\FTP Contents");
                DirectoryInfo dir = new DirectoryInfo(@"C:\FTP Contents");

                outputFile.WriteLine(dir);
                outputFile.WriteLine(DateTime.Now.ToString());

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo f in files)
                {
                    int counter = 0;
                    int count = 0;
                    outputFile.WriteLine(f.FullName);
                    if (f.FullName.Substring(f.FullName.Length - 3, 3) == "zip")
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(f.FullName))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                //Stream stream = entry.Open();
                                //MemoryStream str = new MemoryStream();
                                //stream.CopyTo(str);
                                try
                                {
                                    outputFile.WriteLine(entry.Name);
                                    byte[] data;
                                    using (Stream inputStream = entry.Open())
                                    {
                                        MemoryStream memoryStream = inputStream as MemoryStream;
                                        if (memoryStream == null)
                                        {
                                            memoryStream = new MemoryStream();
                                            inputStream.CopyTo(memoryStream);
                                        }
                                        data = memoryStream.ToArray();
                                    }

                                    string fileName = entry.Name;
                                    result = await Upload(data, fileName, null, null);
                                    if (result)
                                    {
                                        counter++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    outputFile.WriteLine(ex.Message);
                                }

                            }
                            count = archive.Entries.Count;
                        }
                        if (count == counter)
                        {
                            try
                            {
                                f.Delete();
                            }
                            catch (Exception ex)
                            {
                                outputFile.WriteLine(ex.Message);
                            }
                            outputFile.WriteLine(f.FullName + " deleted");
                        }
                    }

                }
            }
        }

        public async Task<Document> GetDocumentByFileIdAsync(string fileId)
        {
            return await _repository.GetDocumentByFileIdAsync(fileId);
        }
    }
}
