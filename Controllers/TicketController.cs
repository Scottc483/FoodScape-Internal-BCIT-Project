using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data;

namespace Food_Scape.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketController : Controller
    {
        FoodScapeContext _foodScapeContext;
        private IServiceProvider _serviceProvider;

        public TicketController(FoodScapeContext foodScapeContext, IServiceProvider serviceProvider)
        {
            _foodScapeContext = foodScapeContext;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Tickets Admin Pages / CRUD
        /// 
        /// The method retrieves a list of events 
        /// from the database using Ticket Repository. 
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("admin/tickets/list")]
        public IActionResult Index(string msg, string err, string searchString)
        {
            TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
            var tickets = ticketRepository.GetAllTickets();

            // check if search string is empty,
            // if not empty, look for Events in db where
            // properties contain search string
            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = _foodScapeContext
                               .Tickets
                               .Where(x => x.TicketId.ToString().Contains(searchString)).ToList();
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
            return View(tickets);
        }

        [Route("admin/tickets/details/{id}")]
        public IActionResult Details(int id)
        {
            TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
            TicketTypeRepository ticketTypeRepository = new TicketTypeRepository(_foodScapeContext);
            UserRepository userRepository = new UserRepository(_foodScapeContext, _serviceProvider);

            Ticket ticket = ticketRepository.GetTicketById(id);
            ticket.User = userRepository.GetUserById(ticket.UserId);
            ticket.TicketType = ticketTypeRepository.GetTicketType(ticket.TicketTypeId);

            return View(ticket);
        }

        [Route("admin/tickets/create")]
        public IActionResult Create(int userId)
        {
            ViewBag.SelectedUser = userId;

            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            TicketTypeRepository ticketTypeRepo = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            var ticketTypes = ticketTypeRepo.GetAllTicketTypes().ToList();
            
            foreach(var ticketType in ticketTypes)
            {
                ticketType.Event = eventRepo.GetEventById(ticketType.EventId);
            }

            ViewBag.Users = userRepo.GetAllUsers();
            ViewBag.TicketTypes = ticketTypes;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickets/create")]
        public IActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(ticket.UserId == null || ticket.TicketTypeId == null)
                    {
                        throw new Exception("Error");
                    }
                    TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
                    ticketRepository.CreateTicket(ticket);

                    return RedirectToAction("Index", new { msg = "Successfully created new Ticket!" });
                }
                catch
                {
                    return RedirectToAction("Index", new { err = "Failed creating new Ticket." });
                }
            }

            return View(ticket);
        }

        [Route("admin/tickets/edit")]
        public IActionResult Edit(int id)
        {
            TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);

            Ticket ticket = ticketRepository.GetTicketById(id);
            ticket.User = userRepo.GetUserById(ticket.UserId);

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickets/edit")]
        public IActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
                    ticketRepository.EditTicket(ticket);
                    return RedirectToAction("Index", new { msg = "Successfully edited Ticket!" });
                }
                catch
                {
                    return RedirectToAction("Index", new { err = "Failed editing Ticket!" });
                }
            }

            return View(ticket);
        }

        [Route("admin/tickets/delete")]
        public IActionResult Delete(int id)
        {
            TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
            Ticket ticket = ticketRepository.GetTicketById(id);

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/tickets/delete")]
        public IActionResult Delete(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketRepository ticketRepository = new TicketRepository(_foodScapeContext);
                    ticketRepository.DeleteTicket(ticket);
                    return RedirectToAction("Index", new { msg = "Successfully edited Ticket!" });
                }
                catch 
                {
                    return RedirectToAction("Index", new { err = "Failed editing Ticket!" });
                }
            }

            return View(ticket);
        }

    }
}
