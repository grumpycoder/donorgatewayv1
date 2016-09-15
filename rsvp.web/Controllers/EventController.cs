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
            var @event = db.Events.SingleOrDefault(x => x.Name == id);
            if (@event == null) return View("EventNotFound");


            @event.ParseTemplate();

            var vm = Mapper.Map<EventViewModel>(@event);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Register(EventViewModel model)
        {
            var guest = Mapper.Map<RegisterFormViewModel>(db.Guests.Include(e => e.Event).Include(t => t.Event.Template).SingleOrDefault(g => g.FinderNumber == model.PromoCode && g.EventId == model.EventId));

            if (guest == null) ModelState.AddModelError("PromoCode", "Invalid Reservation Code");

            if (guest != null && guest.IsRegistered) ModelState.AddModelError("Attendance", "Already registered for event");

            var @event = db.Events.Find(model.EventId);

            db.Events.AddOrUpdate(@event);
            db.SaveChanges();

            if (ModelState.IsValid) return View(guest);

            model.Template = db.Templates.FirstOrDefault(x => x.Id == model.TemplateId);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Confirm(RegisterFormViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                var @evt = db.Events.Include(x => x.Template).SingleOrDefault(e => e.Id == dto.EventId);
                if (@evt != null) dto.Template = @evt.Template;
                return View("Register", dto);
            }

            var @event = db.Events.Include(x => x.Template).SingleOrDefault(e => e.Id == dto.EventId);
            if (@event == null) return View("EventNotFound");

            var guest = db.Guests.Include(e => e.Event).Include(t => t.Event.Template).SingleOrDefault(g => g.Id == dto.GuestId);

            var compareGuest = Mapper.Map<Guest>(dto);

            if (!guest.Equals(compareGuest))
            {
                var demoChange = Mapper.Map<DemographicChange>(compareGuest);
                demoChange.Source = Source.RSVP;
                demoChange.UpdatedBy = "Donor";
                db.DemographicChanges.Add(demoChange);
            }
            Mapper.Map(dto, guest);

            @event.RegisterGuest(guest);
            @event.SendEmail(guest);

            //Need copy of template before discarding changes. 
            var template = @event.Template.Copy();

            //Do not modify template of event. 
            db.Entry(@event.Template).State = EntityState.Unchanged;

            db.Events.AddOrUpdate(@event);

            db.Guests.AddOrUpdate(guest);
            db.SaveChanges();

            var finishFormViewModel = Mapper.Map<FinishFormViewModel>(guest);

            finishFormViewModel.Template = template;

            return View("Finish", finishFormViewModel);
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