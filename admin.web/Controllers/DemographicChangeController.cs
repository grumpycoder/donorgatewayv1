using System.Web.Http;
using DonorGateway.Data;
using DonorGateway.Domain;

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
            //var demo = new DemographicChange()
            //{
            //    FinderNumber = "11111"
            //}; 

            return Ok("Got It");
        }
    }
}
