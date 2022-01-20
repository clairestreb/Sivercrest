using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Azure;
using System.IO;
using System.IO.Compression;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Diagnostics;
using System.Threading;

namespace UploadZip
{
    public class ZipUpload
    {
        public static string ftpFolder = WebConfigurationManager.AppSettings["ftpFolder"];


        public void manageTasks()
        {

            Thread zipThread = null;
            Thread alertThread = null;
            DirectoryInfo dir = new DirectoryInfo(ZipUpload.ftpFolder);
     //SendEmailAlerts();
            while (true)
            {

                //Only One thread is alive at any moment for these requests
                //                            ProceedZips();     //Needs to be commented out if you want to do this in threads.. was only used for debugging non threading       
                //                    Console.WriteLine("MAIN THREAD: Spawned Thread To Collate Statements By Contact");
                //Checking to see if any new zip files need to be processed
                if (dir.GetFiles("*.zip").Length > 0)
                {
                    if (zipThread == null || !zipThread.IsAlive)
                    {
                        zipThread = new Thread(ProceedZips);
                        zipThread.Start();
                        while (!zipThread.IsAlive) ; //Wait for thread to at least start
                    }
                }


                //Only One thread is alive at any moment for these requests
                //                                           SendEmailAlerts();     //Needs to be commented out if you want to do this in threads.. was only used for debugging non threading       
                //                    Console.WriteLine("MAIN THREAD: Spawned Thread To Collate Statements By Contact");
                DatabaseAccess.GenerateEmailAlerts();

                if (DatabaseAccess.GetNextEmailAlerts().Tables[0].Rows.Count != 0) 
                {
                    SendEmailAlerts();
/*                    if (alertThread == null || !alertThread.IsAlive)
                    {
                        alertThread = new Thread(SendEmailAlerts);
                        alertThread.Start();
                        while (!alertThread.IsAlive) ; //Wait for thread to at least start
                    }
 */               }

                Thread.Sleep(120*1000); //Wait for 120 seconds

            }
        }


        public bool Upload(byte[] array, string fileName, int? contactId = null, int? mainContactId = null)
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
//                    await cloudFile.UploadFromByteArrayAsync(array, 0, array.Count());
                    cloudFile.UploadFromByteArray(array, 0, array.Count());
                    cloudFile.Metadata.Add("initialFileName", fileName);
                    cloudFile.SetMetadata();
                }
                return true;
            }
            return false;
        }

        public void SendEmailAlerts()
        {
            List<EmailAlertViewModel> emailsData = new List<EmailAlertViewModel>();
            int k = 0;
            int attempts = 0;

            //Setting limits on this loop so not constantly sending out emails, in case a zip file needs to be loaded
            while (attempts < 100 && (DatabaseAccess.GetNextEmailAlerts().Tables[0].Rows.Count != 0))
            {
                var dataTable = DatabaseAccess.GetNextEmailAlerts();
                AdoMapper.Mapper(emailsData, dataTable);
                var body = EmailService.GetEmailBody(emailsData);
                var subject = EmailService.GetEmailSubject(emailsData[0]);
                //If Email Send fails, wait 5 seconds before retrying, if succeeeds, then increment count
                if(EmailService.ExecuteSendEmail(emailsData[0].Recipient, subject, body, true, emailsData[0].Id))
                {
                    k++;
                }
                else
                {
                    Thread.Sleep(5000);
                }
                emailsData.Clear();
                Thread.Sleep(2000);

                // Doing this in the loop because we may have deleted emails that should not be sent out
                // OR reduced documents to existing recipients due to deleted documents
                DatabaseAccess.GenerateEmailAlerts();
                attempts++;
            }

            if (k > 0)
            {
                Console.WriteLine("----Sent " + k + " Client Email Alerts--------> " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString());
            }

        }

        public void ProceedZips()
        {
                           Console.WriteLine("----Processing Zip files----------> " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() );

            bool processErrors = false;
            string output = ftpFolder + "Log.txt";
            using (StreamWriter outputFile = new StreamWriter(output))
            {
                outputFile.Write("Start ");
                bool result;
                DirectoryInfo dir = new DirectoryInfo(ftpFolder);
                string errDir = ftpFolder + "ErrorLog";

                outputFile.WriteLine(dir);
                outputFile.WriteLine(DateTime.Now.ToString());

                FileInfo[] files = dir.GetFiles();
                bool deleteFile;
                FileStream delFs = null;
                foreach (FileInfo f in files)
                {
                    int counter = 0;
                    int count = 0;
                    int attempt = 0;
                    outputFile.WriteLine(f.FullName);
                    if (f.FullName.Substring(f.FullName.Length - 3, 3) == "zip")
                    {
                        deleteFile = true;
                        try
                        {
                            delFs = f.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                            delFs.Close();

                            using (ZipArchive archive = ZipFile.OpenRead(f.FullName))
                            {
                                foreach (var entry in archive.Entries)
                                {
                                    try
                                    {
                                        outputFile.Write(entry.Name);
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
                                        result = false;
                                        attempt = 0;
                                        while (!result && attempt < 3)
                                        {
                                            result = Upload(data, fileName, null, null);
                                            attempt++;
                                        }
                                        if (result)
                                        {
                                            counter++;
                                            outputFile.WriteLine();
                                        }
                                        else
                                        {
                                            processErrors = true;
                                            outputFile.WriteLine(" ---------------- ERROR");
                                            File.WriteAllBytes(errDir + @"\" + fileName, data);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        processErrors = true;
                                        outputFile.WriteLine(ex.Message);
                                        throw ex; //NEW LOGIC Added 9/11/2020 to See issues more clearly
                                    }
                                }
                                count = archive.Entries.Count;
                            }
                        }
                        catch(Exception xx)
                        {
                            deleteFile = false;
                            throw xx; //NEW LOGIC Added 9/11/2020 to See issues more clearly
                        }

                        if (deleteFile)
                        {
                            try
                            {
                                f.Delete();
                                outputFile.WriteLine(f.FullName + " deleted");
                            }
                            catch (Exception ex)
                            {
                                processErrors = true;
                                outputFile.WriteLine(ex.Message);
                                throw ex;
                            }
                        }
                        //                        }
                    }
                    outputFile.WriteLine("--------------------------------------------------------------------");
                }

                if (processErrors)
                {
                    File.Copy(output, errDir + @"\err_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt");
                }
            }
        }
    }
}
