using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Food_Scape.Controllers
{
    public class TicketTypeController : Controller
    {
        FoodScapeContext _foodScapeContext;

        public TicketTypeController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }

        /// <summary>
        /// The method retrieves a list of ticket types and corresponding events 
        /// from the database using TicketTypeRepository and EventRepository. 
        /// </summary>
        /// <returns>IActionResult</returns>

        [Route("tickets")]

        public IActionResult Index()
        {
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
            var ticketType = ticketTypeRepository.GetAllTicketTypes().ToList();

            EventRepository eRepo = new EventRepository(_foodScapeContext);
            var events = eRepo.GetAllEvents();

            List<TicketTypeVM> ticketVM = new List<TicketTypeVM>();
            foreach (var ticket in ticketType)
            {
                var eventName = events.FirstOrDefault(ev => ev.EventId == ticket.EventId)?.Name;
                var startDate = events.FirstOrDefault(ev => ev.EventId == ticket.EventId)?.StartDate;
                string strDate = startDate.ToString();
                var endDate = events.FirstOrDefault(ev => ev.EventId == ticket.EventId)?.EndDate;
                string enDate = endDate.ToString();

                var location = events.FirstOrDefault(ev => ev.EventId == ticket.EventId)?.Address;
                var about = events.FirstOrDefault(ev => ev.EventId == ticket.EventId)?.Description;
                TicketTypeVM tempTicket = new TicketTypeVM
                {
                    TicketTypeId = ticket.TicketTypeId,
                    EventId = ticket.EventId,
                    TicketType1 = ticket.TicketType1,
                    Price = ticket.Price,
                    Discount = ticket.Discount,
                    Name = eventName,
                    StartDate = strDate.Substring(0,10),
                    EndDate= enDate.Substring(0,10),
                    Address= location,
                    Description = about,
                };
                ticketVM.Add(tempTicket);

            }
            return View(ticketVM);
        }

        /// <summary>
        /// Tickets Admin Pages / CRUD
        /// 
        /// The method retrieves a list of TicketTypes 
        /// from the database using TicketType Repository. 
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("admin/tickettypes/list")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex(string msg, string err, string searchString)
        {
            TicketTypeRepository ticketTypeRepo = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            var ticketTypes = ticketTypeRepo.GetAllTicketTypes();

            // check if search string is empty,
            // if not empty, look for TicketTypes in db where
            // properties contain search string
            if (!string.IsNullOrEmpty(searchString))
            {
                ticketTypes = _foodScapeContext
                                .TicketTypes
                                .Where(x => 
                                    x.TicketTypeId.ToString().Contains(searchString) ||
                                    x.TicketType1.Contains(searchString) ||
                                    x.Price.ToString().Contains(searchString) ||
                                    x.Discount.ToString().Contains(searchString) ||
                                    x.Day.ToString().Contains(searchString) || 
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

            // attach event for each ticketType
            foreach (var ticket in ticketTypes)
            {
                ticket.Event = eventRepo.GetEventById(ticket.EventId);
            }

            // store err and msg in ViewBag to be displayed
            ViewBag.Message = msg;
            ViewBag.Error = err;

            return View(ticketTypes);
        }

        [Route("admin/tickettypes/details/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            TicketType ticketType = ticketTypeRepository.GetTicketType(id);
            ticketType.Event = eventRepo.GetEventById(ticketType.EventId);

            return View(ticketType);
        }

        [Route("admin/tickettypes/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            var events = eventRepo.GetAllEvents().ToList();

            // make list of SelectListItems for events
            var preEventsDropDownList = events.Select(e =>
                new SelectListItem { Value = e.EventId.ToString(), Text = e.Name }
            ).ToList();
            var eventsDropDownList = new SelectList(preEventsDropDownList, "Value", "Text");

            ViewBag.EventSelectList = eventsDropDownList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickettypes/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
                    ticketTypeRepository.CreateTicketType(ticketType);
                    return RedirectToAction("AdminIndex", new { msg = "Succesfully created new Ticket Type!" });
                }
                catch
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed creating new Ticket Type." });
                }
            }
            return View();
        }

        [Route("admin/tickettypes/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            TicketType ticketType = ticketTypeRepository.GetTicketType(id);
            ticketType.Event = eventRepo.GetEventById(ticketType.EventId);

            // store event name from attached event from ticket type in view bag
            if(ticketType.Event != null)
            {
                ViewBag.SelectedEvent = ticketType.Event.Name;
            }
            else
            {
                ViewBag.SelectedEvent = "";
            }

            // make the dropdown for all events
            var events = eventRepo.GetAllEvents().ToList();
            var preEventsDropDownList = events.Select(e =>
                new SelectListItem { Value = e.EventId.ToString(), Text = e.Name }
            ).ToList();
            var eventsDropDownList = new SelectList(preEventsDropDownList, "Value", "Text");

            ViewBag.EventSelectList = eventsDropDownList;

            return View(ticketType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickettypes/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
                    ticketTypeRepository.EditTicketType(ticketType);
                    return RedirectToAction("AdminIndex", new { msg = "Succesfully Edited Ticket Type!" });
                }
                catch
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed editing Ticket Type." });
                }
            }

            return View(ticketType);
        }

        [Route("admin/tickettypes/delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            TicketType ticketType = ticketTypeRepository.GetTicketType(id);
            ticketType.Event = eventRepo.GetEventById(ticketType.EventId);

            return View(ticketType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickettypes/delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
                    ticketTypeRepository.DeleteTicketType(ticketType);
                    return RedirectToAction("AdminIndex", new { msg = "Succesfully deleted Ticket Type!" });
                }
                catch
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed deleting Ticket Type." });
                }
            }

            return View(ticketType);
        }
    }
}
