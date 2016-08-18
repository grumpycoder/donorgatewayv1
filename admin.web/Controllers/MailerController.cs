using admin.web.Helpers;
using admin.web.ViewModels;
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
    [RoutePrefix("api/mailer"), Authorize]
    public class MailerController : ApiController
    {

        private readonly DataContext context;

        public MailerController()
        {
            context = new DataContext();
        }

        [HttpGet, Route("campaigns")]
        public IHttpActionResult Campiagns()
        {
            var campaigns = context.Campaigns.ToList();
            return Ok(campaigns);
        }

        [HttpGet, Route("reasons")]
        public IHttpActionResult Reasons()
        {
            var reasons = context.SuppressReasons.ToList();
            return Ok(reasons);
        }

        public IHttpActionResult Get()
        {
            var list = context.Mailers.OrderBy(x => x.Id).Skip(0).Take(20).ToList();
            return Ok(list);
        }

        [Route("search")]
        public IHttpActionResult Search(MailerSearchViewModel vm)
        {
            var page = vm.Page.GetValueOrDefault(0);
            var pageSize = vm.PageSize.GetValueOrDefault(10);
            var skipRows = (page - 1) * pageSize;

            var suppress = vm.Suppress ?? false;

            var pred = PredicateBuilder.True<Mailer>();
            //pred = pred.And(p => p.Suppress == suppress);
            if (!string.IsNullOrWhiteSpace(vm.FirstName)) pred = pred.And(p => p.FirstName.Contains(vm.FirstName));
            if (!string.IsNullOrWhiteSpace(vm.LastName)) pred = pred.And(p => p.LastName.Contains(vm.LastName));
            if (!string.IsNullOrWhiteSpace(vm.FinderNumber)) pred = pred.And(p => p.FinderNumber.StartsWith(vm.FinderNumber));
            if (!string.IsNullOrWhiteSpace(vm.SourceCode)) pred = pred.And(p => p.SourceCode.StartsWith(vm.SourceCode));
            if (!string.IsNullOrWhiteSpace(vm.ZipCode)) pred = pred.And(p => p.ZipCode.StartsWith(vm.ZipCode));
            if (!string.IsNullOrWhiteSpace(vm.State)) pred = pred.And(p => p.State.StartsWith(vm.State));
            if (!string.IsNullOrWhiteSpace(vm.Address)) pred = pred.And(p => p.Address.StartsWith(vm.Address));
            if (!string.IsNullOrWhiteSpace(vm.City)) pred = pred.And(p => p.City.StartsWith(vm.City));
            if (vm.CampaignId != null) pred = pred.And(p => p.CampaignId == vm.CampaignId);
            if (vm.ReasonId != null) pred = pred.And(p => p.ReasonId == vm.ReasonId);

            var list = context.Mailers.AsQueryable()
                         .Order(vm.OrderBy, vm.OrderDirection == "desc" ? SortDirection.Descending : SortDirection.Ascending)
                         .Include(r => r.Reason)
                         .Where(pred)
                         .Skip(skipRows)
                         .Take(pageSize)
                         .ProjectTo<MailerViewModel>();

            var totalCount = context.Mailers.Count();
            var filterCount = context.Mailers.Where(pred).Count();
            var totalPages = (int)Math.Ceiling((decimal)filterCount / pageSize);

            vm.TotalCount = totalCount;
            vm.FilteredCount = filterCount;
            vm.TotalPages = totalPages;

            vm.Items = list.ToList();
            return Ok(vm);
        }


        [HttpPut]
        public IHttpActionResult Put(Mailer mailer)
        {
            var m = context.Mailers.Find(mailer.Id);
            if (m == null) return NotFound();

            context.Mailers.AddOrUpdate(mailer);
            context.SaveChanges();
            return Ok(m);
        }

    }
}
