using AutoMapper;
using DonorGateway.Data;
using rsvp.web.ViewModels;
using System;
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
            var @event = Mapper.Map<EventViewModel>(db.Events.FirstOrDefault(x => x.Name == id));

            if (@event == null) return View("EventNotFound");

            return View(@event);
        }

        [HttpPost]
        public ActionResult Register(EventViewModel model)
        {
            var guest = Mapper.Map<RegisterFormViewModel>(db.Guests.Include(e => e.Event).Include(t => t.Event.Template)
                                                            .FirstOrDefault(g => g.FinderNumber == model.PromoCode));

            if (guest == null) ModelState.AddModelError("PromoCode", "Invalid Reservation Code");

            if (guest != null && guest.IsRegistered) ModelState.AddModelError("Attendance", "Already registered for event");

            if (ModelState.IsValid) return View(guest);

            model.Template = db.Templates.FirstOrDefault(x => x.Id == model.TemplateId);
            return View("Index", model);
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

            var guest = db.Guests.Include(e => e.Event).Include(t => t.Event.Template).SingleOrDefault(x => x.Id == form.GuestId);

            var eventViewModel = Mapper.Map<EventViewModel>(db.Events.Include(t => t.Template).FirstOrDefault(e => e.Id == form.EventId));

            if (eventViewModel.IsAtCapacity)
            {
                form.IsWaiting = true;
                form.WaitingDate = DateTime.Now;
                eventViewModel.GuestWaitingCount += form.TicketCount ?? 0;
            }
            else
            {
                eventViewModel.GuestAttendanceCount += form.TicketCount ?? 0;
            }

            form.ResponseDate = DateTime.Now;
            Mapper.Map(form, guest);
            var @event = db.Events.Find(form.EventId);
            Mapper.Map(eventViewModel, @event);
            db.Guests.AddOrUpdate(guest);
            db.Events.AddOrUpdate(@event);
            db.SaveChanges();

            var m = Mapper.Map<FinishFormViewModel>(guest);
            m.ProcessMessages();

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