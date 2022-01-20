using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Silvercrest.Entities;
using Silvercrest.Entities.Enums;
using Silvercrest.Interfaces.Client;
using Silvercrest.ViewModels.Client.Documents;
using Silvercrest.Web.Areas.Client.Views.Shared;
using Silvercrest.Web.Controllers;
using Silvercrest.Web.Helpers.Files;
using Silvercrest.Web.Helpers.Maintance;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    [Authorize]
    [Maintance]
    public class DocumentsController : BaseController
    {
        private IDocuments _documentsService;

        public DocumentsController(IDocuments service)
        {
            _documentsService = service;
        }

        public ActionResult Index(string contactIdQuery)
        {
            int? contactId;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? (int?)null : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(contactIdQuery));
            contactId = GetContactId(contactId);
            var model = new DocumentsViewModel();
            model = _documentsService.GetDocuments(contactId);
            model.contactId = int.Parse(contactId.ToString());

            ContactId = GetContactId(null);
            var user_role = GetUserRoles();
            if (!user_role)
            {
                if (contactId != ContactId)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Administrator" });
                }
            }

            URLParameters parameters = null;
            if (Session[contactId.ToString()] == null)
            {
                parameters = new URLParameters((int)contactId);
                if (Request.Cookies["IsSecondaryClient"].Value == "true") //Secondary Client
                {
                    Session[contactId.ToString()] = parameters;
                }
            }
            else
            {
                parameters = (URLParameters)Session[contactId.ToString()];
            }

            //model.contactFullName = ((URLParameters)Session[contactId.ToString()]).contactFullName;
            string startDate = null;
            string endDate = null;
            if (Request.Cookies["searchFrom" + contactId] == null && Request.Cookies["searchTo" + contactId] == null)
            {
                startDate = DateTime.Now.AddYears(-1).ToString();
                endDate = DateTime.Now.ToString();
            }
            if (Request.Cookies["searchFrom" + contactId] != null && Request.Cookies["searchTo" + contactId] != null)
            {
                startDate = Request.Cookies["searchFrom" + contactId].Value;
                endDate = Request.Cookies["searchTo" + contactId].Value;
            }

            model.StartDate = startDate;
            model.EndDate = endDate;
            var docUploadDate = model.Documents.Where(x => DateTime.Parse(x.AsOf) >= DateTime.Parse(startDate) && DateTime.Parse(x.AsOf) <= DateTime.Parse(endDate)).ToList();
            model.Documents = docUploadDate;
            model.HasPermission = GetUserRoles();
            model.contactFullName = parameters.contactFullName;
            return View(model);
        }


        public bool GetUserRoles()
        {
            if (Request.Cookies["UserRole"] != null)
            {
                var role = Request.Cookies["UserRole"].Value;

                if (role == UserRole.Administrator.ToString() || role == UserRole.SuperUser.ToString() || role == UserRole.TeamMember.ToString())
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        public int? GetContactId(int? contactId)
        {
            if (contactId == null)
            {
                contactId = int.Parse(Request.Cookies["MainContactId"].Value);
            }
            return contactId;
        }

        public async Task<ActionResult> UploadFile(HttpPostedFileBase uploadFile, int? contactId)
        {
            //Stream fileStream = uploadFile.InputStream;
            string fileName = uploadFile.FileName;
            int? mainContactId = null;
            if (contactId != null)
            {
                mainContactId = int.Parse(Request.Cookies["MainContactId"].Value);
            }

            byte[] data;
            using (Stream inputStream = uploadFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            await _documentsService.Upload(data, fileName, contactId, mainContactId);
            return RedirectToAction("Index", new { contactIdQuery = Silvercrest.Web.Common.Hash.GetEncryptedValue(contactId.ToString()) });
        }

        public ActionResult DownloadFilePage(string fileId, string asOf, int? contactId)
        {
            

            contactId = GetContactId(contactId);

            //if (contactId > 0)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            URLParameters parameters;

            if (Session[contactId.ToString()] != null)
            {
                parameters = (URLParameters)Session[contactId.ToString()];

                return View(new DownloadDocumentPage()
                {
                    FileId = fileId,
                    AsOf = asOf,
                    contactFullName = parameters.contactFullName
                });
            }

            parameters = new URLParameters((int)contactId);
            if (Request.Cookies["IsSecondaryClient"].Value == "true")
            {
                Session[contactId.ToString()] = parameters;
            }

            return View(new DownloadDocumentPage()
            {
                FileId = fileId,
                AsOf = asOf,
                contactFullName = parameters.contactFullName
            });
        }
        public async Task<ActionResult> DownloadFile(string fileId, string asOf)
        {
            var urlHelper = new UrlHelper(Request.RequestContext, RouteTable.Routes);

            string loginPageActionName = nameof(AccountController.Login);
            string loginControllerName = nameof(AccountController).Replace(nameof(Controller), string.Empty);
            string loginPageRoute = urlHelper.Action(loginPageActionName, loginControllerName);

            if (Request.UrlReferrer?.Host != Request.Url.Host || loginPageRoute.Contains(Request.UrlReferrer?.LocalPath))
            {
                return RedirectToAction(nameof(DownloadFilePage), new { fileId, asOf });
            }

            string[] arr = asOf.Split('-');
            var folderName = arr[2] + arr[0];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = cloudFileClient.GetShareReference(WebConfigurationManager.AppSettings["cloudShare"]);

            if (!share.Exists())
            {
                return null;
            }

            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference(folderName);
            CloudFile cloudFile = cloudFileDirectory.GetFileReference(fileId);
            await cloudFile.FetchAttributesAsync();

            MemoryStream fileStream = new MemoryStream();
            using (fileStream)
            {
                cloudFile.DownloadToStream(fileStream);
                byte[] fileBytes = /*new byte[] { };*/fileStream.ToArray();

                string initialFileName = cloudFile.Metadata.FirstOrDefault(x => x.Key == "initialFileName").Value;

                if (string.IsNullOrWhiteSpace(initialFileName))
                {
                    Document document = await _documentsService.GetDocumentByFileIdAsync(fileId);
                    string fileName = document == null ? fileId : document.FileName;
                    string fileExtension = await FileHelper.GetExtensionFromStream(fileStream);
                    initialFileName = $"{fileName}{fileExtension}";
                }

                //cloudFile.DownloadRangeToByteArray(fileBytes, 0, 0, cloudFile.Properties.Length);
                //fileStream.ToArray();
                return File(fileBytes, MediaTypeNames.Application.Octet, initialFileName);
            }
        }


        public ActionResult GetDocuments(string searchFrom, string searchTo, string contactIdQuery)
        {
            int? contactId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(contactIdQuery));
            var fromCookie = new HttpCookie("searchFrom" + contactId);
            DateTime now = DateTime.Now;
            fromCookie.Value = searchFrom;
            fromCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(fromCookie);
            fromCookie = new HttpCookie("searchTo" + contactId);
            fromCookie.Value = searchTo;
            fromCookie.Expires = now.AddMonths(1);
            Response.Cookies.Add(fromCookie);
            //Response.Cookies["searchFrom" + contactId].Value = searchFrom;
            //Response.Cookies["searchTo" + contactId].Value = searchTo;
            return RedirectToAction("Index", "Documents", new { contactIdQuery = contactIdQuery });
        }

        public ActionResult MakeDocumentFavorite(string fileId, bool isStrategy, bool isFavorite, bool isSamg, string contactIdQuery)
        {
            int? contactId = null;
            contactId = string.IsNullOrEmpty(contactIdQuery) || contactIdQuery == "undefined" ? GetContactId(contactId) : int.Parse(Silvercrest.Web.Common.Hash.GetDecryptedValue(contactIdQuery));
            //int contactId = int.Parse(Request.Cookies["MainContactId"].Value);
            string userName = User.Identity.Name;
            _documentsService.MakeDocumentFavorite(contactId, fileId, isStrategy, isFavorite, userName, isSamg);
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Documents", new { contactIdQuery = contactIdQuery });
        }

        public ActionResult RemoveDocument(string fileId)
        {
            //int contactId =  int.Parse(Request.Cookies["MainContactId"].Value);
            string userName = User.Identity.Name;
            _documentsService.RemoveDocument(fileId, userName);
            //return RedirectToAction("Index");
            return Json(new { success = true, responseText = "Document has been deleted!" }, JsonRequestBehavior.AllowGet);
        }
    }
}