using System.Web.Mvc;

namespace Silvercrest.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }
    }
}