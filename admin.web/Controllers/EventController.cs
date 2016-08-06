using admin.web.Helpers;
using admin.web.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using DonorGateway.Data;
using DonorGateway.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace admin.web.Controllers
{
    [RoutePrefix("api/event"), Authorize]
    public class EventController : ApiController
    {
        private readonly DataContext context;

        public EventController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.Events.Include(x => x.Template).ToList();

            return Ok(list);
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            var vm = context.Events.AsQueryable().SingleOrDefault(x => x.Name == name);
            return Ok(vm);
        }

        [HttpGet, Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var @event = context.Events.Include(t => t.Template).SingleOrDefault(x => x.Id == id);
            if (@event == null) return NotFound();

            var model = Mapper.Map<EventViewModel>(@event);
            return Ok(model);
        }

        [HttpPost, Route("{id:int}/guests")]
        public IHttpActionResult Guests(int id, GuestSearchViewModel vm)
        {
            var page = vm.Page.GetValueOrDefault(0);
            var pageSize = vm.PageSize.GetValueOrDefault(10);
            var skipRows = (page - 1) * pageSize;
            var ticketMailed = vm.IsMailed ?? false;
            var isWaiting = vm.IsWaiting ?? false;
            var isAttending = vm.IsAttending ?? false;

            var pred = PredicateBuilder.True<Guest>();
            pred = pred.And(p => p.EventId == id);
            if (!string.IsNullOrWhiteSpace(vm.Address)) pred = pred.And(p => p.Address.Contains(vm.Address));
            if (!string.IsNullOrWhiteSpace(vm.FinderNumber)) pred = pred.And(p => p.FinderNumber.StartsWith(vm.FinderNumber));
            if (!string.IsNullOrWhiteSpace(vm.Name)) pred = pred.And(p => p.Name.Contains(vm.Name));
            if (!string.IsNullOrWhiteSpace(vm.City)) pred = pred.And(p => p.City.StartsWith(vm.City));
            if (!string.IsNullOrWhiteSpace(vm.State)) pred = pred.And(p => p.State.Equals(vm.State));
            if (!string.IsNullOrWhiteSpace(vm.ZipCode)) pred = pred.And(p => p.Zipcode.StartsWith(vm.ZipCode));
            if (!string.IsNullOrWhiteSpace(vm.Phone)) pred = pred.And(p => p.Phone.Contains(vm.Phone));
            if (!string.IsNullOrWhiteSpace(vm.Email)) pred = pred.And(p => p.Email.StartsWith(vm.Email));
            if (!string.IsNullOrWhiteSpace(vm.LookupId)) pred = pred.And(p => p.LookupId.StartsWith(vm.LookupId));
            if (vm.IsMailed != null) pred = pred.And(p => p.IsMailed == ticketMailed);
            if (vm.IsWaiting != null) pred = pred.And(p => p.IsWaiting == isWaiting);
            if (vm.IsAttending != null) pred = pred.And(p => p.IsAttending == isAttending);

            var list = context.Guests.AsQueryable()
                .Order(vm.OrderBy, vm.OrderDirection == "desc" ? SortDirection.Descending : SortDirection.Ascending)
                .Where(pred)
                .Skip(skipRows)
                .Take(pageSize)
                .ProjectTo<GuestViewModel>();
            //.ToList();

            var totalCount = context.Guests.Count();
            var filterCount = context.Guests.Where(pred).Count();
            var totalPages = (int)Math.Ceiling((decimal)filterCount / pageSize);

            vm.TotalCount = totalCount;
            vm.FilteredCount = filterCount;
            vm.TotalPages = totalPages;


            var items = Mapper.Map<List<GuestViewModel>>(list);
            vm.Items = list.ToList();

            //vm.Items = list; 

            return Ok(vm);

        }

        [HttpPost, Route("{id:int}/guests/export")]
        public IHttpActionResult Export(int id, GuestSearchViewModel vm)
        {
            var ticketMailed = vm.IsMailed ?? false;
            var isWaiting = vm.IsWaiting ?? false;
            var isAttending = vm.IsAttending ?? false;

            var pred = PredicateBuilder.True<Guest>();
            pred = pred.And(p => p.EventId == id);
            if (!string.IsNullOrWhiteSpace(vm.Address)) pred = pred.And(p => p.Address.Contains(vm.Address));
            if (!string.IsNullOrWhiteSpace(vm.FinderNumber)) pred = pred.And(p => p.FinderNumber.StartsWith(vm.FinderNumber));
            if (!string.IsNullOrWhiteSpace(vm.Name)) pred = pred.And(p => p.Name.Contains(vm.Name));
            if (!string.IsNullOrWhiteSpace(vm.City)) pred = pred.And(p => p.City.StartsWith(vm.City));
            if (!string.IsNullOrWhiteSpace(vm.State)) pred = pred.And(p => p.State.Equals(vm.State));
            if (!string.IsNullOrWhiteSpace(vm.ZipCode)) pred = pred.And(p => p.Zipcode.StartsWith(vm.ZipCode));
            if (!string.IsNullOrWhiteSpace(vm.Phone)) pred = pred.And(p => p.Phone.Contains(vm.Phone));
            if (!string.IsNullOrWhiteSpace(vm.Email)) pred = pred.And(p => p.Email.StartsWith(vm.Email));
            if (!string.IsNullOrWhiteSpace(vm.LookupId)) pred = pred.And(p => p.LookupId.StartsWith(vm.LookupId));
            if (vm.IsMailed != null) pred = pred.And(p => p.IsMailed == ticketMailed);
            if (vm.IsWaiting != null) pred = pred.And(p => p.IsWaiting == isWaiting);
            if (vm.IsAttending != null) pred = pred.And(p => p.IsAttending == isAttending);

            var list = context.Guests.AsQueryable()
                    .Where(pred)
                    .ProjectTo<GuestExportViewModel>();

            var path = HttpContext.Current.Server.MapPath(@"~\app_data\guestlist.csv");

            using (var csv = new CsvWriter(new StreamWriter(File.Create(path))))
            {
                csv.Configuration.RegisterClassMap<GuestMap>();
                csv.WriteHeader<GuestExportViewModel>();
                csv.WriteRecords(list);
            }
            var filename = $"guest-list-{DateTime.Now.ToString("u")}.csv";

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

        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var @event = context.Events.Find(id);
            if (@event == null) return NotFound();

            var template = context.Templates.Find(@event.TemplateId);

            if (template != null) context.Templates.Remove(template);
            context.SaveChanges();

            context.Events.Remove(@event);
            context.SaveChanges();

            return Ok("Deleted Event");
        }

        public IHttpActionResult Post(Event vm)
        {
            context.Templates.Add(vm.Template);
            context.SaveChanges();
            context.Events.Add(vm);
            context.SaveChanges();

            var @event = Mapper.Map<EventViewModel>(vm);

            return Ok(@event);
        }

        public IHttpActionResult Put(Event vm)
        {
            context.Events.AddOrUpdate(vm);
            context.SaveChanges();

            var model = Mapper.Map<EventViewModel>(vm);

            return Ok(model);
        }

        [HttpPost, Route("{id:int}/registerguest")]
        public IHttpActionResult RegisterGuest(int id, Guest guestDto)
        {
            var @event = context.Events.Find(id);

            if (@event == null) return NotFound();

            if (guestDto == null) return NotFound();

            @event.RegisterGuest(guestDto);
            guestDto.Event = @event;

            context.Events.AddOrUpdate(@event);
            context.SaveChanges();

            context.Guests.AddOrUpdate(guestDto);
            context.SaveChanges();

            return Ok(guestDto);
        }

        [HttpPost, Route("{id:int}/addticket")]
        public IHttpActionResult AddTicket(int id, GuestViewModel guestDto)
        {
            var @event = context.Events.Find(id);

            if (@event == null) return NotFound();
            if (guestDto == null) return NotFound();

            var guest = Mapper.Map<Guest>(guestDto);
            
            @event.AddTickets(guest, guestDto.AdditionalTickets);

            context.Events.AddOrUpdate(@event);
            context.SaveChanges();

            context.Guests.AddOrUpdate(guest);
            context.SaveChanges();

            Mapper.Map(guest, guestDto);
            guestDto.Event = @event;
            return Ok(guestDto);
        }

        [HttpPost, Route("{id:int}/mailticket")]
        public IHttpActionResult MailTicket(int id, Guest guestDto)
        {
            var @event = context.Events.Find(id);

            if (@event == null) return NotFound();

            if (guestDto == null) return NotFound();

            @event.MailTicket(guestDto);
            guestDto.Event = @event;

            context.Events.AddOrUpdate(@event);
            context.SaveChanges();

            context.Guests.AddOrUpdate(guestDto);
            context.SaveChanges();

            //TODO: Send mail here

            return Ok(guestDto);
        }

        [HttpPost, Route("{id:int}/addtomail")]
        public IHttpActionResult AddToMail(int id, Guest guestDto)
        {
            var @event = context.Events.Find(id);

            if (@event == null) return NotFound();

            if (guestDto == null) return NotFound();

            @event.MoveToMailQueue(guestDto);
            guestDto.Event = @event;

            context.Events.AddOrUpdate(@event);
            context.SaveChanges();

            context.Guests.AddOrUpdate(guestDto);
            context.SaveChanges();

            return Ok(guestDto);
        }

    }
}