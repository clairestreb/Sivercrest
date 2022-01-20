using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace UploadZip
{
    class Program
    {
        static void Main(string[] args)
        {

            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                return;
            }
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
                var message = "ERROR: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + ' ' + s;
                EmailService.ExecuteSendEmailWithoutAlert("ERROR-CLIENT PORTAL: Error in .NET Application", message);
            }
        }
        private static void Start()
        {
            //            int a = 0;

            //OPTION 1
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


            // Perform your work here.
            ZipUpload zp = new ZipUpload();
            try
            {
                zp.manageTasks();
            }
            catch (Exception xx)
            {
                throw xx;
            }






            /*****DEBUGGING *****/
            /*
                        List<EmailAlertViewModel> emailsData = new List<EmailAlertViewModel>();
                        DatabaseAccess.GenerateEmailAlerts();
                        while (DatabaseAccess.GetNextEmailAlerts().Tables[0].Rows.Count != 0)
                        {
                            var dataTable = DatabaseAccess.GetNextEmailAlerts();
                            AdoMapper.Mapper(emailsData, dataTable);
                            var body = EmailService.GetEmailBody(emailsData);
                            var subject = EmailService.GetEmailSubject(emailsData[0]);
                            EmailService.ExecuteSendEmail2(emailsData[0].Recipient, subject, body, true, emailsData[0].Id);
                            emailsData.Clear();
                        }

                        ///---------------

                        Task task = Task.Factory.StartNew(() => a = 1);
                        Task taskAlert = Task.Factory.StartNew(() => a = 2);
                        DirectoryInfo dir = new DirectoryInfo(ZipUpload.ftpFolder);
                        while (true)
                        {
                            if (task.Status != TaskStatus.Running)
                            {
                                //Checking to see if any new zip files need to be processed
                                if (dir.GetFiles("*.zip").Length > 0)
                                {
                                    task = Task.Factory.StartNew(() => zp.ProceedZips());
                                    task.Wait();
                                }

                            }

                            if (taskAlert.Status != TaskStatus.Running)
                            {
                                //Checking to see if any email alerts need to be sent
                                taskAlert = Task.Factory.StartNew(() => zp.SendEmailAlerts());
                                taskAlert.Wait();
                            }


                            Thread.Sleep(1 * 60 * 1000);
                        }

            */
        }
    }
}
