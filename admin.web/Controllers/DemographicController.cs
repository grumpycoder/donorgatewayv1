using admin.web.Helpers;
using admin.web.ViewModels;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using DonorGateway.Data;
using DonorGateway.Domain;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using EntityFramework.Utilities;

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

        [Route("search")]
        public IHttpActionResult Search(DemographicSearchViewModel vm)
        {
            var page = vm.Page.GetValueOrDefault(0);
            var pageSize = vm.PageSize.GetValueOrDefault(10);
            var skipRows = (page - 1) * pageSize;

            var pred = PredicateBuilder.True<DemographicChange>();
            if (!string.IsNullOrWhiteSpace(vm.Name)) pred = pred.And(p => p.Name.Contains(vm.Name));
            if (!string.IsNullOrWhiteSpace(vm.LookupId)) pred = pred.And(p => p.LookupId.Contains(vm.LookupId));
            if (!string.IsNullOrWhiteSpace(vm.FinderNumber)) pred = pred.And(p => p.FinderNumber.StartsWith(vm.FinderNumber));
            if (!string.IsNullOrWhiteSpace(vm.Street)) pred = pred.And(p => p.Street.StartsWith(vm.Street));
            if (!string.IsNullOrWhiteSpace(vm.Street2)) pred = pred.And(p => p.Street2.StartsWith(vm.Street2));
            if (!string.IsNullOrWhiteSpace(vm.City)) pred = pred.And(p => p.City.StartsWith(vm.City));
            if (!string.IsNullOrWhiteSpace(vm.State)) pred = pred.And(p => p.State.StartsWith(vm.State));
            if (!string.IsNullOrWhiteSpace(vm.Zipcode)) pred = pred.And(p => p.Zipcode.StartsWith(vm.Zipcode));
            if (!string.IsNullOrWhiteSpace(vm.Email)) pred = pred.And(p => p.Email.StartsWith(vm.Email));
            if (!string.IsNullOrWhiteSpace(vm.Phone)) pred = pred.And(p => p.Phone.StartsWith(vm.Phone));
            //if (!string.IsNullOrWhiteSpace(vm.Source)) pred = pred.And(p => p.Source.Equals(vm.Source));

            var list = context.DemographicChanges.AsQueryable()
                         .Order(vm.OrderBy, vm.OrderDirection == "desc" ? SortDirection.Descending : SortDirection.Ascending)
                         .Where(pred)
                         .Skip(skipRows)
                         .Take(pageSize)
                         .ProjectTo<DemographicViewModel>();

            var totalCount = context.DemographicChanges.Count();
            var filterCount = context.DemographicChanges.Where(pred).Count();
            var totalPages = (int)Math.Ceiling((decimal)filterCount / pageSize);

            vm.TotalCount = totalCount;
            vm.FilteredCount = filterCount;
            vm.TotalPages = totalPages;

            vm.Items = list.ToList();
            return Ok(vm);
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
