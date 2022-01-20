using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Data;
using System.Xml.Linq;
using Renci.SshNet;

namespace DocumentsApp
{
    public class Info
    {
        public string SourceFolder { get; set; }
        public string ArchiveFolder { get; set; }
        public string FileNames { get; set; }
    }

    public class DbEntryModel
    {
        public FileInfo FileInfo { get; set; }
        public int FilesCountInZip { get; set; }
    }

    public class FileWalker
    {
        public static string currDir = AppDomain.CurrentDomain.BaseDirectory;
        public static bool isRunning = false; //Added to Prevent Multiple FTP jobs causing clogged piping

        public Info Info;
        public Func<string, string, bool> CompareFiles;
        public FileWalker(Info info, Func<string, string, bool> compareFiles)
        {
            this.Info = info;
            this.CompareFiles = compareFiles;
        }

        public void Start()
        {
            WalkFiles(new DirectoryInfo(Info.SourceFolder));
        }

        private void WalkFiles(DirectoryInfo dir)
        {
            isRunning = true; //Added to Prevent Multiple FTP jobs causing clogged piping
            try
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo f in files)
                {
                    if (CompareFiles(f.Name, Info.FileNames))
                    {
                        SendViaFtp(f);
                    }
                }
            }
            catch (Exception ex)
            {
//                Console.WriteLine("Method WalkFiles, exception {0}", ex.Message);
                var s = ex.Message.ToString();
                var message = "ERROR Documents App: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + ' ' + s;
                EmailService.SendEmail("Error occured in Documents App", message);
            }
            isRunning = false; //Added to Prevent Multiple FTP jobs causing clogged piping
        }

        private void SendViaFtp(FileInfo fi)
        {
            try
            {
                var doc = XDocument.Load(FileWalker.currDir + @"\settings.xml");


                /*
                 * OLD CODE NOT USING SECURE FTP
                                var uri = doc.Root.Element("FtpAddress").Value + fi.Name;

                                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                                request.Credentials = new NetworkCredential(doc.Root.Element("FtpLogin").Value, doc.Root.Element("FtpPassword").Value);
                                request.Timeout = 600000; //10 mins
                                request.ReadWriteTimeout=600000; //10 mins
                                request.KeepAlive = false;
                //                request.UseBinary = true;
                                request.Method = WebRequestMethods.Ftp.UploadFile;
                                request.UsePassive = false;

                                byte[] fileContents = File.ReadAllBytes(fi.FullName);
                                request.ContentLength = fileContents.Length;
                                Stream requestStream = request.GetRequestStream();
                                requestStream.Write(fileContents, 0, fileContents.Length);
                                requestStream.Close();

                                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                                Console.WriteLine("Upload File {1} Complete, status {0}", response.StatusDescription, fi.Name);
                                response.Close();
                */

                var privateKey = new PrivateKeyFile(FileWalker.currDir + @"\silvercrest");
                var username = doc.Root.Element("FtpLogin").Value;
                var password = doc.Root.Element("FtpPassword").Value;
                var host = doc.Root.Element("FtpHost").Value;
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(username, password));
                methods.Add(new PrivateKeyAuthenticationMethod(username, new[] { privateKey }));

                var connectionInfo = new ConnectionInfo(host, 22, username, methods.ToArray());

                // Upload File
                using (var sftp = new SftpClient(connectionInfo))
                {

                    sftp.Connect();
                    sftp.ChangeDirectory("/../../FTP Contents");
                    //sftp.ChangeDirectory("/MyFolder");
                    using (var uplfileStream = System.IO.File.OpenRead(fi.FullName))
                    {
                        sftp.UploadFile(uplfileStream, fi.Name);
                    }
                    sftp.Disconnect();
                }



                Task[] tasks = new Task[2];
                var dbModel = new DbEntryModel
                {
                    FileInfo = fi,
                    FilesCountInZip = CountFilesInZip(fi.FullName)
                };
                tasks[0] = Task.Factory.StartNew((i) => AddDbRecord(i), dbModel);
                tasks[1] = Task.Factory.StartNew((i) => MoveToArchive(i), fi);
                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("Method SendViaFtp, exception {0}", ex.Message);
                var s = ex.Message.ToString();
                var message = "ERROR Documents App: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + ' ' + s;
                EmailService.SendEmail("Error occured in Documents App", message);
            }
        }

        private void MoveToArchive(object fileInfo)
        {
            FileInfo fi = (FileInfo)fileInfo;
            File.Move(fi.FullName, Info.ArchiveFolder + "\\" + fi.Name);
            Console.WriteLine("The file {0} successfully moved to archive folder", fi.Name);
        }

        private void AddDbRecord(object dbModel)
        {
            try
            {
                DbEntryModel db = (DbEntryModel)dbModel;
                FileInfo fi = db.FileInfo;
                int filesCount = db.FilesCountInZip;
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("p_Web_Zip_File_Upload ", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@file_name", SqlDbType.NVarChar).Value = fi.Name;
                        cmd.Parameters.Add("@number_of_documents", SqlDbType.Int).Value = filesCount;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("The record of file {0} successfully added to db", fi.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method AddDbRecord, exception {0}", ex.Message);
            }
        }

        private int CountFilesInZip(string zipFullName)
        {
            int count = 0;
            try
            {
                using (ZipArchive archive = ZipFile.Open(zipFullName, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!String.IsNullOrEmpty(entry.Name))
                        {
                            count += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Method CountFilesInZip, exception {0}", ex.Message);
            }
            return count;
        }
    }
}
