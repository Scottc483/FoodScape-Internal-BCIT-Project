using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Food_Scape.Controllers
{
    public class EventController : Controller
    {
        FoodScapeContext _foodScapeContext;

        public EventController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }

        /// <summary>
        /// Event Admin Pages / CRUD
        /// 
        /// The method retrieves a list of events 
        /// from the database using Event, Vendor, and EventVendor Repositories. 
        /// </summary>
        /// <returns>IActionResult</returns>
        [Authorize(Roles = "Admin")]
        [Route("admin/event/list")]
        public IActionResult Index(string msg, string err, string searchString)
        {
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            EventVendorRepository evRepo = new EventVendorRepository(_foodScapeContext);
            var events = eventRepo.GetAllEvents().ToList();

            // check if search string is empty,
            // if not empty, look for Events in db where
            // properties contain search string
            if (!string.IsNullOrEmpty(searchString))
            {
                events = _foodScapeContext
                         .Events
                         .Where(x =>
                                x.EventId.ToString().Contains(searchString) ||
                                x.Address.Contains(searchString) ||
                                x.Capacity.ToString().Contains(searchString) ||
                                x.AgeRestriction.ToString().Contains(searchString) ||
                                x.Name.Contains(searchString) ||
                                x.Description.Contains(searchString)).ToList();
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

            return View(events);
        }

        // Event Details
        [Authorize(Roles = "Admin")]
        [Route("admin/event/details/{id}")]
        public IActionResult Details(int id)
        {
            EventRepository eventRepository = new EventRepository(_foodScapeContext);
            Event events = eventRepository.GetEventById(id);

            return View(events);
        }

        // Event Create
        [Authorize(Roles = "Admin")]
        [Route("admin/event/create")]
        public IActionResult Create()
        {
            return View();
        }

        // Event Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/event/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Event events)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EventRepository eventRepository = new EventRepository(_foodScapeContext);
                    eventRepository.CreateEvent(events);
                    return RedirectToAction("Index", new { msg = "Successfully created Event!" });
                }
                catch
                {
                    return RedirectToAction("Index", new { err = "Failed creating Event." });
                }
            }

            return View(events);
        }

        // Event Edit
        [Authorize(Roles = "Admin")]
        [Route("admin/event/edit")]
        public IActionResult Edit(int id)
        {
            EventRepository eventRepository = new EventRepository(_foodScapeContext);
            Event events = eventRepository.GetEventById(id);

            return View(events);
        }

        // Event Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/event/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Event events)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EventRepository eventRepository = new EventRepository(_foodScapeContext);
                    eventRepository.EditEvent(events);
                    return RedirectToAction("Index", new { msg = "Successfully edited Event!" });
                }
                catch 
                {
                    return RedirectToAction("Index", new { err = "Failed editing Event." });
                }
            }

            return View(events);
        }

        // Event Delete
        [Authorize(Roles = "Admin")]
        [Route("admin/event/delete")]
        public IActionResult Delete(int id)
        {
            EventRepository eventRepository = new EventRepository(_foodScapeContext);
            Event events = eventRepository.GetEventById(id);

            return View(events);
        }

        // Event Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/event/delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Event events)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EventRepository eventRepository = new EventRepository(_foodScapeContext);
                    eventRepository.DeleteEvent(events);
                    return RedirectToAction("Index", new { msg = "Successfully deleted Event!" });
                }
                catch
                {
                    return RedirectToAction("Index", new { err = "Failed deleting Event." });
                }
            }

            return View(events);
        }

        // User's FrontEnd Pages
        // Event Index
        [Route("events")]
        public IActionResult Info()
        {
            EventRepository eventRepository = new EventRepository(_foodScapeContext);
            var events = eventRepository.GetAllEvents().ToList();

            ReviewRepository reviewRepository = new ReviewRepository(_foodScapeContext);
            var reviews = reviewRepository.GetAllReviews();

            var viewModel = new EventInfoViewModel
            {
                Events = events,
                Reviews = reviews
            };

            return View(viewModel);
        }


    }
}
