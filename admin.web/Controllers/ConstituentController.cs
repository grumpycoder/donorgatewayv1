using admin.web.Helpers;
using admin.web.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DonorGateway.Data;
using DonorGateway.Domain;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Helpers;
using System.Web.Http;

namespace admin.web.Controllers
{
    [RoutePrefix("api/constituent"), Authorize]
    public class ConstituentController : ApiController
    {

        private readonly DataContext context;

        public ConstituentController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get(int id)
        {
            var list = context.Constituents.SingleOrDefault(c => c.Id == id);
            return Ok(list);
        }

        [Route("search")]
        public IHttpActionResult Search(ConsituentSearchViewModel vm)
        {
            var page = vm.Page.GetValueOrDefault(0);
            var pageSize = vm.PageSize.GetValueOrDefault(10);
            var skipRows = (page - 1) * pageSize;

            var pred = PredicateBuilder.True<Constituent>();
            if (!string.IsNullOrWhiteSpace(vm.Name)) pred = pred.And(p => p.Name.Contains(vm.Name));
            if (!string.IsNullOrWhiteSpace(vm.FinderNumber)) pred = pred.And(p => p.FinderNumber.Contains(vm.FinderNumber));
            if (!string.IsNullOrWhiteSpace(vm.LookupId)) pred = pred.And(p => p.LookupId.Contains(vm.LookupId));
            if (!string.IsNullOrWhiteSpace(vm.City)) pred = pred.And(p => p.State.StartsWith(vm.City));
            if (!string.IsNullOrWhiteSpace(vm.State)) pred = pred.And(p => p.State.StartsWith(vm.State));
            if (!string.IsNullOrWhiteSpace(vm.Zipcode)) pred = pred.And(p => p.Zipcode.StartsWith(vm.Zipcode));
            if (!string.IsNullOrWhiteSpace(vm.Email)) pred = pred.And(p => p.Email.Contains(vm.Email));
            if (!string.IsNullOrWhiteSpace(vm.Phone)) pred = pred.And(p => p.Phone.Contains(vm.Phone));

            var list = context.Constituents.AsQueryable()
                         .Order(vm.OrderBy, vm.OrderDirection == "desc" ? SortDirection.Descending : SortDirection.Ascending)
                         .Where(pred)
                         .Include(x => x.TaxItems)
                         .Skip(skipRows)
                         .Take(pageSize)
                         .ProjectTo<ConstituentViewModel>();

            var totalCount = context.Constituents.Count();
            var filterCount = context.Constituents.Where(pred).Count();
            var totalPages = (int)Math.Ceiling((decimal)filterCount / pageSize);

            vm.TotalCount = totalCount;
            vm.FilteredCount = filterCount;
            vm.TotalPages = totalPages;

            vm.Items = list.ToList();
            return Ok(vm);
        }

        public IHttpActionResult Put(Constituent vm)
        {
            if (vm.Id == 0) return NotFound();

            var demoChange = Mapper.Map<DemographicChange>(vm);
            demoChange.Source = Source.Tax;
            context.DemographicChanges.Add(demoChange);

            context.Constituents.AddOrUpdate(vm);
            context.SaveChanges();

            Mapper.Map<ConstituentViewModel>(vm);

            return Ok(vm);
        }

        [HttpPost, Route("{id:int}/taxitem")]
        public IHttpActionResult AddTaxItem(int id, TaxItem item)
        {
            context.TaxItems.AddOrUpdate(item);
            context.SaveChanges();
            return Ok(item);
        }

        [HttpPut, Route("{id:int}/taxitem")]
        public IHttpActionResult UpdateTaxItem(int id, TaxItem item)
        {
            var o = context.TaxItems.Find(item.Id);
            if (o == null) return NotFound();

            context.TaxItems.AddOrUpdate(item);
            context.SaveChanges();
            return Ok(item);
        }

        [HttpDelete, Route("deletetax/{id:int}")]
        public IHttpActionResult DeleteTaxItem(int id)
        {
            var tax = context.TaxItems.Find(id);
            if (tax == null) return NotFound();

            context.TaxItems.Remove(tax);
            context.SaveChanges();
            return Ok();
        }

    }
}
