using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silvercrest.Services;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Azure;
using System.Web.Configuration;
using System.IO;
using Silvercrest.Web.Common;
using Silvercrest.ViewModels;
using Silvercrest.Web.Areas.Client.Views.Shared;

namespace Silvercrest.Web.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {    
            return RedirectToAction("Index", "Home", new { Area = "Client" });
        }

        public ActionResult TermsOfUse()
        {
            var view = new GenericViewModel();
            if(Request.Cookies["mainContactId"] == null || Request.Cookies["mainContactId"].Value == "")
            {
                view.contactFullName = "";
            }
            else
            {
                view.contactFullName = URLParameters.GetContactFullName(int.Parse(Request.Cookies["mainContactId"].Value));
            }
            return View(view);
        }

        public FileContentResult ImportantInformation()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = cloudFileClient.GetShareReference(WebConfigurationManager.AppSettings["cloudShare"]);
            if (share.Exists())
            {
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference("WebsiteDocuments");
                CloudFile cloudFile = cloudFileDirectory.GetFileReference("SAMG_Important_Information.pdf");
                MemoryStream fileStream = new MemoryStream();
                using (fileStream)
                {
                    cloudFile.DownloadToStream(fileStream);
                    byte[] fileBytes = fileStream.ToArray();
                    string mimeType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "inline; filename=SAMG_Important_Information.pdf");
                    return File(fileBytes, mimeType);
                    //var fsResult = new FileStreamResult(fileStream, "application/pdf");
                    //return fsResult;
                }
            }
            return null;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string Encrypt(string input)
        {

            return Hash.GetEncryptedValue(input);
        }

        public string Decrypt(string input)
        {
            return Hash.GetDecryptedValue(input);
        }

    }
}