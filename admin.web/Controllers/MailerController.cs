using DonorGateway.Data;
using System.Web.Http;

namespace admin.web.Controllers
{
    [RoutePrefix("api/event"), Authorize]
    public class MailerController : ApiController
    {

        private readonly DataContext context;

        public MailerController()
        {
            context = new DataContext();
        }
    }
}
