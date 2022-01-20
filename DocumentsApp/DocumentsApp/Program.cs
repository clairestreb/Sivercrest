using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Configuration;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.Web.Configuration;
using Renci.SshNet;

namespace DocumentsApp
{
    class Program
    {
//        private static string host = "52.234.130.240";
//        private static string username = @"silvercrest.local\silvercrest";
//        private static string password = "Welcome2017!@";

/*
        public static int Send(string fileName)
        {
            var privateKey = new PrivateKeyFile(FileWalker.currDir + @"\silvercrest");

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
                using (var uplfileStream = System.IO.File.OpenRead(FileWalker.currDir + @"\" + fileName))
                {
                    sftp.UploadFile(uplfileStream, fileName);
                }
                sftp.Disconnect();
            }
            return 0;
        }

        static void Main(string[] args)
        {
            Send("Rohan.txt");

        }
*/

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            try
            {
                string runningProcess = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(runningProcess);

                if (processes.Length > 1)
                {
                    Console.WriteLine("Exiting with Option 1 where same process name found");
                    Thread.Sleep(5000);

                    Environment.Exit(0); // This will kill my instance.
                }

                if (Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                {
                    Console.WriteLine("Exiting with Option 2 where same process found");
                    Thread.Sleep(5000);

                    Environment.Exit(0); // This will kill my instance.

                }

                DoJob1();
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
                var message = "ERROR Documents App: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + ' ' + s;
                EmailService.SendEmail("Error occured in Documents App", message);
            }
        }

        private static void DoJob1()
        {
            Console.WriteLine("Doing job");
            if (!File.Exists(FileWalker.currDir + @"\settings.xml"))
            {
                XDocument settings = new XDocument(
                    new XElement("settings",
                        new XElement("SourceFolder", ConfigurationManager.AppSettings["SourceFolder"]),
                        new XElement("ArchiveFolder", ConfigurationManager.AppSettings["ArchiveFolder"]),
                        new XElement("FileNames", ConfigurationManager.AppSettings["FileNames"]),
                        new XElement("FtpHost", ConfigurationManager.AppSettings["FtpHost"]),
                        new XElement("FtpLogin", ConfigurationManager.AppSettings["FtpLogin"]),
                        new XElement("FtpPassword", ConfigurationManager.AppSettings["FtpPassword"])
                    ));
                settings.Save(FileWalker.currDir + "/settings.xml");
            }
            var doc = XDocument.Load(FileWalker.currDir + @"\settings.xml");
            Info info = new Info
            {
                SourceFolder = doc.Root.Element("SourceFolder").Value,
                FileNames = doc.Root.Element("FileNames").Value,
                ArchiveFolder = doc.Root.Element("ArchiveFolder").Value
            };
            var dir = new DirectoryInfo(info.SourceFolder);
            var walker = new FileWalker(info, (a, b) => a.Contains(b));
            //            Task task = Task.Factory.StartNew(() => walker.Start());
            while (true)
            {
                //                if (task.Status != TaskStatus.Running)
                //                {
                if ((dir.GetFiles().Length > 0) && !FileWalker.isRunning) //Added to Prevent Multiple FTP jobs causing clogged piping)
                {
                    //                task = Task.Factory.StartNew(() => walker.Start());
                    walker.Start();
                }
                //        }
                Thread.Sleep(1 * 60 * 1000);
            }
        }
    }
}
