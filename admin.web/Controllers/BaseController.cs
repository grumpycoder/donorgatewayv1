using DonorGateway.Data;
using System.Linq;
using System.Web.Mvc;

namespace admin.web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                using (var context = new DataContext())
                {
                    var username = User.Identity.Name;
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var user = context.Users.SingleOrDefault(u => u.UserName == username);
                        if (user != null)
                        {
                            ViewData.Add("FullName", user.FullName);
                            ViewData.Add("UserPhotoType", user.UserPhotoType);
                            ViewData.Add("UserPhoto", user.UserPhoto);
                        }
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}