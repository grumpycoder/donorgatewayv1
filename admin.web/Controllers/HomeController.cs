using System.Web.Mvc;

namespace admin.web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "rsvp")]
        public ActionResult Events()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Demographics()
        {
            return View();
        }

        [Authorize(Roles = "tax")]
        public ActionResult DonorTax()
        {
            return View();
        }

        [Authorize(Roles = "mailer")]
        public ActionResult Mailers()
        {
            return View();
        }
    }
}