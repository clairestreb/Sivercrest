using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;

namespace UploadZip
{
    class EmailService
    {
        public static string SAMGPortalURL = WebConfigurationManager.AppSettings["SAMGPortalURL"];

        public static string GetEmailSubject(EmailAlertViewModel model)
        {
            string subject = "";
            if (model.DocumentType != "ClientUpload" && model.DocumentType != "TeamUpload")
            {
                subject = "New Document(s) from Silvercrest";
            }
            if (model.DocumentType == "TeamUpload")
            {
                subject = "New Document(s) from your Silvercrest Team";
            }
            if (model.DocumentType == "ClientUpload")
            {
                subject = "New Document(s) Upload by Client [" + model.ClientName + "]";
            }
            return subject;
        }

        public static string GetEmailBody(List<EmailAlertViewModel> model)
        {
            string body = "Hello, <br/> <br/>";
            if (model[0].DocumentType != "ClientUpload" && model[0].DocumentType != "TeamUpload")
            {
                body += "The following document(s) are now available at the Silvercrest Portal.<br/><br/>";
                foreach (var item in model)
                {
                    body += @"<a href='" + SAMGPortalURL + "/Client/Documents/DownloadFilePage?fileId=" + item.FileId + "&asOf=" + item.DocumentDate.ToString("MM-dd-yyyy") + "' >" + item.DisplayName + "</a> ";
                    body += item.DocumentType + " as of ";
                    body += item.DocumentDate.ToString("MM-dd-yyyy");
                    body += "<br/>";
                }
            }
            else if (model[0].DocumentType == "TeamUpload")
            {
                body = "Hello, <br/><br/>The following document(s) were recently uploaded by your Silvercrest Team. <br/><br/>";
                foreach (var item in model)
                {
                    body += @"<a href='" + SAMGPortalURL + "/Client/Documents/DownloadFilePage?fileId=" + item.FileId + "&asOf=" + item.DocumentDate.ToString("MM-dd-yyyy") + "' >" + item.DisplayName + "</a> ";
                    body += "<br/>";
                }
            }
            else if (model[0].DocumentType == "ClientUpload")
            {
                body = "Hello, <br/><br/>The following document(s) were uploaded by " + model[0].ClientName + ". <br/><br/>";
                foreach (var item in model)
                {
                    body += @"<a href='" + SAMGPortalURL + "/Client/Documents/DownloadFilePage?fileId=" + item.FileId + "&asOf=" + item.DocumentDate.ToString("MM-dd-yyyy") + "' >" + item.DisplayName + "</a> ";
                    body += "<br/>";
                }
            }
            string htmlBody = @"
            <style>
                div{
	                font-family: 'Arial', 'Times New Roman', 'Garamond', 'Goudy', serif;
	                font-size: 12px;
	                letter-spacing: 1px;
                }
            </style>
            <center><img src='" + SAMGPortalURL + @"/img/SC_Logo_3D.png' alt='Silvercrest'  style='display: block; margin: 0 auto;' width='135' height='90'></center>  
            <hr/>
            <br/>
            <div><p>" + body + @"</p></div><br></br><br></br><br></br><br></br><br></br>
            <div>Silvercrest Asset Management Group LLC</div>
            <div>1330 Avenue of the Americas, 38th Floor, New York, NY 10019</div>
            <div>Confidentiality Note: This message and all attachments may contain confidential and/or 
            legally privileged information for the firm Silvercrest Asset Management Group LLC, and is
            intended only for the use of the addressee. If you are not the intended recipient, you are hereby notified 
            that any disclosure, copying, distribution or taking of any action in reliance to the contents is strictly
            prohibited. If you have received this message in error, please notify this firm immediately by telephone
            (212-649-0600) or by electronic mail (info@silvercrestgroup.com), and delete this message and all copies and backups thereof.</div>";
            return htmlBody;
        }
/*
        public static async Task ExecuteSendEmail2(string emailTo, string emailSubject, string emailBody, bool isBodyHtml, int alertId)
        {
            string emailFrom = WebConfigurationManager.AppSettings["mailFrom"];
            string displayName = WebConfigurationManager.AppSettings["mailFromName"];
            if (String.IsNullOrEmpty(emailTo))
            {
                return;
            }
            //For development
            //     emailSubject = "This is a test of the new client portal";
            MailMessage mail = new MailMessage(); //emailFrom, "", emailSubject, emailBody)
            mail.From = new MailAddress(emailFrom, displayName);
            mail.Subject = emailSubject;
            mail.Body = emailBody;
            mail.IsBodyHtml = isBodyHtml;

            foreach (var address in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(address);
            }
            mail.Bcc.Add("samgtester1@gmail.com");
            SmtpClient client = new SmtpClient();
            client.Timeout = 500000;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mail);
                DatabaseAccess.UpdateTable(alertId);
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }

*/
        public static bool ExecuteSendEmail(string emailTo, string emailSubject, string emailBody, bool isBodyHtml, int alertId)
        {
            bool successful = true;
            string emailFrom = WebConfigurationManager.AppSettings["mailFrom"];
            string displayName = WebConfigurationManager.AppSettings["mailFromName"];
            if (String.IsNullOrEmpty(emailTo))
            {
                return false;
            }
            //For development
            //     emailSubject = "This is a test of the new client portal";
            MailMessage mail = new MailMessage(); //emailFrom, "", emailSubject, emailBody)
            mail.From = new MailAddress(emailFrom, displayName);
            mail.Subject = emailSubject;
            mail.Body = emailBody;
            mail.IsBodyHtml = isBodyHtml;

            foreach (var address in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(address);
            }
            mail.Bcc.Add("samgtester1@gmail.com"); //p w d --> "goo Aur 2005"
            SmtpClient client = new SmtpClient();
            client.Timeout = 500000;
//            client.EnableSsl = false;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mail);
//                Console.WriteLine("Email Alert Sent");
                DatabaseAccess.UpdateTable(alertId);
            }
            catch (Exception ex)
            {
                successful = false;
                var s = ex.Message.ToString();
                Console.Write("ERROR Sending Email: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + "; Alert ID=" + alertId + "...");
                Console.WriteLine(s);
            }

            return successful;
        }

        public static void ExecuteSendEmailWithoutAlert(string subject, string message, bool isHtml = false)
        {
            string emailFrom = WebConfigurationManager.AppSettings["mailFrom"];
            string emailTo = WebConfigurationManager.AppSettings["mailTo"];
            string displayName = WebConfigurationManager.AppSettings["mailFromName"];

            var client = new SmtpClient();

            MailMessage mail = new MailMessage(); //emailFrom, "", emailSubject, emailBody)
            mail.From = new MailAddress(emailFrom, displayName);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = isHtml;
            mail.To.Add(emailTo);
/*
            MailMessage mail = new MailMessage(emailFrom, emailTo)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };
*/ 
            client.Timeout = 500000;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
                Console.Write("ERROR Sending Email: " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString());
                Console.WriteLine(s);
            }
        }
    }
}
