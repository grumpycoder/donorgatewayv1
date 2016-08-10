using DonorGateway.Data;
using EntityFramework.Utilities;
using System.Linq;
using System.Web.Http;

namespace admin.web.Controllers
{
    [RoutePrefix("api/demographics"), Authorize]
    public class DemographicsController : ApiController
    {
        private readonly DataContext context;

        public DemographicsController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.DemographicChanges.ToList();

            return Ok(list);
        }

        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var demo = context.DemographicChanges.Find(id);

            if (demo == null) return NotFound();

            context.DemographicChanges.Remove(demo);
            context.SaveChanges();
            return Ok("Deleted");
        }

        [HttpDelete]
        public IHttpActionResult Delete()
        {

            var count = EFBatchOperation.For(context, context.DemographicChanges).Where(x => x.Id != null).Delete();
            return Ok($"Deleted {count}");
        }
    }
}
