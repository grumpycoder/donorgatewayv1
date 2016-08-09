using System.Linq;
using System.Web.Http;
using DonorGateway.Data;

namespace admin.web.Controllers
{
    public class DemographicChangeController : ApiController
    {
        private readonly DataContext context; 

        public DemographicChangeController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.DemographicChanges.ToList();

            return Ok(list);
        }
    }
}
