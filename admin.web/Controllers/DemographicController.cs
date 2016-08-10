using CsvHelper;
using DonorGateway.Data;
using DonorGateway.Domain;
using EntityFramework.Utilities;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
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

        public IHttpActionResult Get(int? page, int? pageSize)
        {
            var size = pageSize ?? 2;
            var pageNumber = (page ?? 1);
            var list = context.DemographicChanges.OrderBy(x => x.Id).Skip(size * (pageNumber - 1)).Take(size).ToList();

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


        [HttpPost, Route("export")]
        public IHttpActionResult Export()
        {

            var list = context.DemographicChanges.ToList();
            var path = HttpContext.Current.Server.MapPath(@"~\app_data\guestlist.csv");

            using (var csv = new CsvWriter(new StreamWriter(File.Create(path))))
            {
                csv.WriteHeader<DemographicChange>();
                csv.WriteRecords(list);
            }

            var filename = $"demographic-changes-{DateTime.Now.ToString("u")}.csv";

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            response.Content.Headers.Add("x-filename", filename);

            return ResponseMessage(response);
        }


    }
}
