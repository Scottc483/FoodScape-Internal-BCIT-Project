using Food_Scape.Models;
using Food_Scape.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Food_Scape.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventVendorController : Controller
    {
        FoodScapeContext _foodScapeContext;

        public EventVendorController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }

        /// <summary>
        /// EventVendor Admin Pages / CRUD
        /// 
        /// The method retrieves a list of events 
        /// from the database using Event, Vendor, and EventVendor Repositories. 
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("admin/eventvendors/list")]
        public ActionResult Index(int? eventId, int? vendorId, string msg, string err, string searchString)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);

            var eventVendors = evRepo.GetAllEventVendors();


            // grab event vendors if event id or vendor id is not null
            if(eventId != null)
            {
                eventVendors = evRepo.GetEventVendorByEventId(eventId);
            }

            if (vendorId != null)
            {
                eventVendors = evRepo.GetEventVendorByVendorId(vendorId);
            }

            // assign all event and vendor for each EventVendor to be displayed in table in view
            foreach(var ev in eventVendors)
            {
                ev.Event = eventRepo.GetEventById(ev.EventId);
                ev.Vendor = vendorRepo.GetRecord(ev.VendorId);
            }

            // check if search string is empty,
            // if not empty, look for EventVendors in db where
            // properties contain search string
            if (!string.IsNullOrEmpty(searchString))
            {
                eventVendors = _foodScapeContext
                               .EventVendors
                               .Where(x =>
                                      x.EventVendorId.ToString().Contains(searchString) ||
                                      x.Vendor.Name.Contains(searchString) ||
                                      x.Event.Name.Contains(searchString)).ToList();
            }

            // switch value of msg and err into empty string if null
            if (msg == null)
            {
                msg = "";
            }

            if (err == null)
            {
                err = "";
            }

            // store err and msg in ViewBag to be displayed
            ViewBag.Message = msg;
            ViewBag.Error = err;
            return View(eventVendors);
        }

        // GET: EventVendorController/Details/5
        [Route("admin/eventvendors/details/{id}")]
        public ActionResult Details(int id)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);

            var eventVendor = evRepo.GetEventVendorById(id);

            eventVendor.Event = eventRepo.GetEventById(eventVendor.EventId);
            eventVendor.Vendor = vendorRepo.GetRecord(eventVendor.VendorId);

            return View(eventVendor);
        }

        // GET: EventVendorController/Create
        [Route("admin/eventvendors/create")]
        public ActionResult Create()
        {
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);

            var events = eventRepo.GetAllEvents().ToList();
            var vendors = vendorRepo.GetRecords().ToList();

            // make list of SelectListItems for events and vendors
            var preEventsDropDownList = events.Select(e =>
                new SelectListItem { Value = e.EventId.ToString(), Text = e.Name }
            ).ToList();
            var eventsDropDownList = new SelectList(preEventsDropDownList, "Value", "Text");

            var preVendorsDropDownList = vendors.Select(v =>
                new SelectListItem { Value = v.VendorId.ToString(), Text = v.Name }
            ).ToList();
            var vendorsDropDownList = new SelectList(preVendorsDropDownList, "Value", "Text");

            // store the dropdown lists in ViewBag to be displayed in view
            ViewBag.EventSelectList = eventsDropDownList;
            ViewBag.VendorSelectList = vendorsDropDownList;

            return View();
        }

        // POST: EventVendorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/eventvendors/create")]
        public ActionResult Create(EventVendor eventVendor)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            try
            {
                evRepo.CreateEventVendor(eventVendor);
                return RedirectToAction("Index", new {msg = "Successfully created new Event Vendor!" });
            }
            catch
            {
                return RedirectToAction("Index", new {err = "Failed creating new Event Vendor."});
            }
        }

        // GET: EventVendorController/Edit/5
        [Route("admin/eventvendors/edit")]
        public ActionResult Edit(int id)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);

            var eventVendor = evRepo.GetEventVendorById(id);

            var events = eventRepo.GetAllEvents().ToList();
            var vendors = vendorRepo.GetRecords().ToList();

            ViewBag.SelectedEvent = eventVendor.Event.Name;
            ViewBag.SelectedVendor = eventVendor.Vendor.Name;

            // make list of SelectListItems for events and vendors
            var preEventsDropDownList = events.Select(e =>
                new SelectListItem { Value = e.EventId.ToString(), Text = e.Name }
            ).ToList();
            var eventsDropDownList = new SelectList(preEventsDropDownList, "Value", "Text");

            var preVendorsDropDownList = vendors.Select(v =>
                new SelectListItem { Value = v.VendorId.ToString(), Text = v.Name }
            ).ToList();
            var vendorsDropDownList = new SelectList(preVendorsDropDownList, "Value", "Text");

            // store the dropdown lists in ViewBag to be displayed in view
            ViewBag.EventSelectList = eventsDropDownList;
            ViewBag.VendorSelectList = vendorsDropDownList;

            return View(eventVendor);
        }

        // POST: EventVendorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/eventvendors/edit")]
        public ActionResult Edit(EventVendor newEventVendor)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            try
            {
                evRepo.EditEventVendor(newEventVendor);
                return RedirectToAction("Index", new { msg = "Successfully edited Event Vendor!" });
            }
            catch
            {
                return RedirectToAction("Index", new { err = "Failed editing Event Vendor." });
            }
        }

        // GET: EventVendorController/Delete/5
        [Route("admin/eventvendors/delete")]
        public ActionResult Delete(int id)
        {           
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);

            var eventVendor = evRepo.GetEventVendorById(id);
            eventVendor.Event = eventRepo.GetEventById(eventVendor.EventId);
            eventVendor.Vendor = vendorRepo.GetRecord(eventVendor.VendorId);

            return View(eventVendor);
        }

        // POST: EventVendorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/eventvendors/delete")]
        public ActionResult Delete(EventVendor eventVendor)
        {
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            var ev = evRepo.GetEventVendorById(eventVendor.EventVendorId);
            try
            {
                evRepo.DeleteEventVendor(ev);
                return RedirectToAction("Index", new { msg = "Successfully deleted Event Vendor!" });
            }
            catch
            {
                return RedirectToAction("Index", new { err = "Failed deleting Event Vendor." });
            }
        }
    }
}
