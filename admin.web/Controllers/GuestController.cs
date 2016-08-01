using DonorGateway.Data;
using DonorGateway.Domain;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace admin.web.Controllers
{
    [RoutePrefix("api/guest")]
    public class GuestController : ApiController
    {
        private readonly DataContext context;

        public GuestController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.Guests;
            return Ok(list);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var vm = context.Guests.FirstOrDefault(x => x.Id == id);
            return Ok(vm);
        }

        public IHttpActionResult Put(Guest vm)
        {
            var @event = context.Events.Find(vm.EventId);

            if (vm.IsWaiting == true) @event.GuestWaitingCount += vm.TicketCount ?? 0;
            if (vm.IsAttending == true) @event.GuestAttendanceCount += vm.TicketCount ?? 0;

            context.Events.AddOrUpdate(@event);
            context.SaveChanges();

            context.Guests.AddOrUpdate(vm);
            context.SaveChanges();
            //TODO: Return updated event details
            return Ok(vm);
        }
    }
}