using AutoMapper;
using DonorGateway.Data;
using DonorGateway.Domain;
using rsvp.web.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace rsvp.web.Controllers
{
    public class EventController : Controller
    {
        private readonly DataContext db;

        public EventController()
        {
            db = new DataContext();
        }

        [Route("{id}")]
        public ActionResult Index(string id)
        {
            var @event = Mapper.Map<EventViewModel>(db.Events.Include(t => t.Template).SingleOrDefault(x => x.Name == id));

            return @event == null ? View("EventNotFound") : View(@event);
        }

        [HttpPost]
        public ActionResult Register(EventViewModel model)
        {
            var guest = Mapper.Map<RegisterFormViewModel>(db.Guests.Include(e => e.Event).Include(t => t.Event.Template).SingleOrDefault(g => g.FinderNumber == model.PromoCode && g.EventId == model.EventId));

            if (guest == null) ModelState.AddModelError("PromoCode", "Invalid Reservation Code");

            if (guest != null && guest.IsRegistered) ModelState.AddModelError("Attendance", "Already registered for event");

            if (!ModelState.IsValid)
            {
                model.Template = db.Templates.FirstOrDefault(x => x.Id == model.TemplateId);
                return View("Index", model);
            }

            return View(guest);


        }

        [HttpPost]
        public ActionResult Confirm(RegisterFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                var @evt = db.Events.Include(x => x.Template).SingleOrDefault(e => e.Id == form.EventId);
                if (@evt != null) form.Template = @evt.Template;
                return View("Register", form);
            }

            var @event = db.Events.Include(x => x.Template).SingleOrDefault(e => e.Id == form.EventId);
            var guest = db.Guests.Include(e => e.Event).Include(t => t.Event.Template).SingleOrDefault(g => g.Id == form.GuestId);

            Mapper.Map<RegisterFormViewModel, Guest>(form, guest);

            @event.RegisterGuest(guest);
            @event.SendMail(guest);

            db.Events.AddOrUpdate(@event);
            db.SaveChanges();

            db.Guests.AddOrUpdate(guest);
            db.SaveChanges();

            var m = Mapper.Map<FinishFormViewModel>(guest);
            m.ParseMessages();

            return View("Finish", m);
        }

        public ActionResult Finish(FinishFormViewModel model)
        {
            return View(model);
        }

        public ActionResult EventNotFound()
        {
            return View();
        }
    }
}