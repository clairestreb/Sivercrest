using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Silvercrest.Services.CommonServices
{
    public static class EmailService
    {
        public static async Task ExecuteSendEmail(string emailTo, string emailSubject, string emailBody, bool isBodyHtml)
        {
            string emailFrom = WebConfigurationManager.AppSettings["mailFrom"];
            string emailFromName = WebConfigurationManager.AppSettings["mailFromName"];
            if (String.IsNullOrEmpty(emailTo))
            {
                return;
            }
            //For development
            //     emailSubject = "This is a test of the new client portal";
            MailMessage mail = new MailMessage(); //emailFrom, "", emailSubject, emailBody)
            mail.From = new MailAddress(emailFrom, emailFromName);
            mail.Subject = emailSubject;
            mail.Body = emailBody;
            mail.IsBodyHtml = isBodyHtml;
            foreach (var address in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(address);
            }
//            if (emailSubject.ToUpper().Contains("ACTIVATION"))
//            {
                mail.Bcc.Add("samgtester1@gmail.com");
//            }
            SmtpClient client = new SmtpClient();
            client.Timeout = 500000;
//            client.EnableSsl = false;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }

        public static Task SendEmailAboutNewRegistration(string emailTo, string password, string urlToFollow)
        {
            var title = "New Registration";
            var sb = new StringBuilder();
            sb.Append("<div>UserName: " + emailTo);
            sb.Append("<br/>Password: " + password);
            sb.Append("<br/><br/><br/>Please click <a href =\"" + urlToFollow + "\">here</a> to setup your password and security questions.");


            var bodyTemplate = sb.ToString();
            return WrapTemplate(emailTo, title, bodyTemplate, true);
        }

        public static Task SendEmailForgotPassword(string emailTo, string token, string baseUrl, string userId, int timeAlive)
        {
            var title = "Silvercrest Client Portal: Password Reset";
            var time = System.Web.HttpUtility.UrlEncode(DateTime.Now.AddHours(timeAlive).ToString());
            var callbackUrl = baseUrl + "/Account/ResetPassword?code=" + Encrypt(token, "") + "&alive=" + Encrypt(time, "") + "&id=" + Encrypt(userId, "");
            var sb = new StringBuilder();
            sb.Append("Hello,<br/><br/>Please reset your password by clicking <a href=\"" + callbackUrl + "\"><b>here</b></a>");

            var content = sb.ToString();
            return WrapTemplate(emailTo, title, content, true);
        }

        public static Task SendEmailToTeam(string subject, string message, string[] teamEmails, string emailFrom, string senderName)
        {
            Task result = null;
            string strMessage = Regex.Replace(message, @"\r\n?|\n", "<br/>");
            string strTo = "";
            message = "\n\n\n--------------- Email From Client ---------------\n" + message;
            strMessage += "<br/><br/><br/>To reply to this email, please click <a href='mailto:" + emailFrom + "?subject=RE: " + subject +"&body=" + 
                System.Web.HttpUtility.UrlEncode(message).Replace("+", "%20") + "'>here</a>";
            foreach (var c in teamEmails)
            {
                strTo = strTo + c + ";";
            }
            result = WrapTemplate(strTo, "Email from [" + senderName + "]: " + subject, strMessage, true);
            return result;
        }

        public static Task SendTwoFactorAuthCodeEmail(string email, string code)
        {
            var title = "Silvercrest Verification";
            var sb = new StringBuilder();
            sb.Append($"This is your Silvercrest Verification Code: {code}");
            var bodyTemplate = sb.ToString();
            return WrapTemplate(email, title, bodyTemplate, true);
        }

        public static Task SendActivationMail(string email, string password, string baseUrl)
        {
            int timeAlive = 24;
            var retoken = System.Web.HttpUtility.UrlEncode(password);
            var time = System.Web.HttpUtility.UrlEncode(DateTime.Now.AddHours(timeAlive).ToString());
            var urlToFollow = baseUrl + "/Account/ChangePassworForFirstLogin?userEmail=" + Encrypt(email, "") + "&code=" + Encrypt(retoken, "") + "&alive=" + Encrypt(time, "");
            var title = "Silvercrest Client Portal: Account activation";
            var sb = new StringBuilder();
            sb.Append("Hello,<br/><br/>Your Silvercrest Portal Username is: <b><a rel='nofollow' style='text-decoration:none;'>" + email + "</a></b>");
            sb.Append("<br/><br/><br/>Please click <a href =\"" + urlToFollow + "\"><b>here</b></a> to setup your password and security questions.");
            sb.Append("<br/><br/><br/>For security reasons, this link will only be active for 24 hours. If the link has expired, please contact your Silvercrest team to receive a new activation email.");
            var bodyTemplate = sb.ToString();
            return WrapTemplate(email, title, bodyTemplate, true);
        }
        
        public static Task WrapTemplate(string emailTo, string emailSubject, string emailBody, bool isBodyHtml)
        {
            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

            string htmlBody = @"
            <style>
                div{
	                font-family: 'Arial', 'Times New Roman', 'Garamond', 'Goudy', serif;
	                font-size: 12px;
	                letter-spacing: 1px;
                }
            </style>
            <center><img src='" + strUrl + @"img/SC_Logo_3D.png' alt='Silvercrest'  style='display: block; margin: 0 auto;' width='135' height='90'></center>  
            <hr/>
            <br/>
            <div><p>" + emailBody + @"</p></div><br></br><br></br><br></br><br></br><br></br>
            <div>Silvercrest Asset Management Group LLC</div>
            <div>1330 Avenue of the Americas, 38th Floor, New York, NY 10019</div>
            <div>Confidentiality Note: This message and all attachments may contain confidential and/or 
            legally privileged information for the firm Silvercrest Asset Management Group LLC, and is
            intended only for the use of the addressee. If you are not the intended recipient, you are hereby notified 
            that any disclosure, copying, distribution or taking of any action in reliance to the contents is strictly
            prohibited. If you have received this message in error, please notify this firm immediately by telephone
            (212-649-0600) or by electronic mail (info@silvercrestgroup.com), and delete this message and all copies and backups thereof.</div>";
                        
            return ExecuteSendEmail(emailTo, emailSubject, htmlBody, true);
        }


        public static string Encrypt(string textForEncrypt, string passPhrase)
        {
            passPhrase = "b87df5f7-94fe-4b86-97d6-6d9c024b67cb";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(textForEncrypt);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(passPhrase, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    textForEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textForEncrypt;
        }
    }
}
