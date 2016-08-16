using System.Web.Mvc;

namespace admin.web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Events()
        {
            return View();
        }

        public ActionResult Demographics()
        {
            return View();
        }

        public ActionResult DonorTax()
        {
            return View();
        }
    }
}