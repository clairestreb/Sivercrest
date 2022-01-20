using Silvercrest.Entities;
using Silvercrest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Silvercrest.Web.Areas.Client.Controllers
{
    public class HomeAdoController: BaseController
    {
        private IClientAdoService _service;
        public HomeAdoController(IClientAdoService service)
        {
            _service = service;
        }

        //public async Task<JsonResult> GetFullInfo(int? contactId)
        //{
        //    var result = Json(await _service.FillInfo(new UserInfo { ContactId = contactId }), JsonRequestBehavior.AllowGet);
        //    return result;
        //}

        //public async Task<JsonResult> GetAccounts(int? contactId)
        //{
        //    return Json(await _service.GetAccounts(contactId), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetCharts(int? contactId)
        //{
        //    return Json(_service.GetCharts(contactId), JsonRequestBehavior.AllowGet);
        //}
    }
}