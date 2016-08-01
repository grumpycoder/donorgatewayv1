using DonorGateway.Data;
using DonorGateway.Domain;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace web.Controllers
{
    [RoutePrefix("api/template")]
    public class TemplateController : ApiController
    {
        private readonly DataContext context;

        public TemplateController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.Templates;
            return Ok(list);
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            var vm = context.Templates.FirstOrDefault(x => x.Name == name);
            return Ok(vm);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var vm = context.Templates.Find(id);
            return Ok(vm);
        }

        public IHttpActionResult Put(Template vm)
        {
            context.Templates.AddOrUpdate(vm);
            context.SaveChanges();
            return Ok(vm);
        }

        public IHttpActionResult Post(Template vm)
        {
            return Ok(vm);
        }

        public IHttpActionResult Delete(int id)
        {
            var template = context.Templates.Find(id);
            context.Templates.Remove(template);
            context.SaveChanges();
            return Ok($"Deleted {id}");
        }
    }
}