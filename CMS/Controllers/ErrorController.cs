using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult NoPermission()
        {
            return View();
        }
        public ActionResult Unauthorized()
        {
            return View();
        }
        public ActionResult ErrorMessage()
        {
            return View();
        }
    }
}