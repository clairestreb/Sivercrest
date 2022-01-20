using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace DocumentsApp
{
    public static class EmailService
    {
        public static void SendEmail(string subject, string message, bool isHtml = false)
        {
            string emailFrom = WebConfigurationManager.AppSettings["mailFrom"];
            string emailTo = WebConfigurationManager.AppSettings["mailTo"];

            var client = new SmtpClient();

            MailMessage mail = new MailMessage(emailFrom, emailTo)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };
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
