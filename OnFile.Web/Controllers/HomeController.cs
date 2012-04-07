using System;
using System.Web.Mvc;
using OnFile.Infra;

namespace OnFile.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Updated()
        {
            var updated = ServiceLocator.Instance.Data.GetLastChangesdate();

            return Json(updated, JsonRequestBehavior.AllowGet);
        }
    }
}
