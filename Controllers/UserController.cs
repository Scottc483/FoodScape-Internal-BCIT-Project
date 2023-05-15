using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food_Scape.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private FoodScapeContext _foodScapeContext;
        private IServiceProvider _serviceProvider;

        public UserController(FoodScapeContext foodScapeContext, IServiceProvider serviceProvider)
        {
            _foodScapeContext = foodScapeContext;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// User Admin Pages / CRUD
        /// 
        /// The method retrieves a list of users 
        /// from the database using User Repository. 
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("admin/user/list")]
        public IActionResult Index(string msg, string err, string searchString)
        {
            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            var users = userRepo.GetAllUsers();

            // check if search string is empty,
            // if not empty, look for FsUsers in db where
            // properties contain search string
            if (!string.IsNullOrEmpty(searchString))
            {
                users = _foodScapeContext.FsUsers.Where(x => 
                                                        x.UserId.ToString().Contains(searchString) ||
                                                        x.FirstName.Contains(searchString) || 
                                                        x.LastName.Contains(searchString) || 
                                                        x.Email.Contains(searchString)).ToList();
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
            return View(users);
        }

        [Route("admin/user/details/{id}")]
        public IActionResult Details(int id)
        {
            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            FsUser fsUser = userRepo.GetUserById(id);

            return View(fsUser);
        }

        [Route("admin/user/tickets/{userId}")]
        public IActionResult UserTicketDetail(int userId)
        {
            // initialize repositories
            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            TicketRepository ticketRepo = new TicketRepository(_foodScapeContext);
            TicketTypeRepository ticketTypeRepo = new TicketTypeRepository(_foodScapeContext);
            EventRepository eventRepo = new EventRepository(_foodScapeContext);

            // grab the user based on parameter userId
            FsUser user = userRepo.GetUserById(userId);

            // grab all the tickets for the user
            var tickets = ticketRepo.GetAllTicketsForUser(userId).ToList();
            user.Tickets = tickets;

            // assign the ticket type for each ticket from the user
            foreach (var ticket in user.Tickets)
            {
                // add event details to the tickets
                ticket.TicketType = ticketTypeRepo.GetTicketType(ticket.TicketTypeId);
                ticket.TicketType.Event = eventRepo.GetEventById(ticket.TicketType.EventId);
            }

            return View(user);
        }

        [Route("admin/user/edit")]
        public IActionResult Edit(int id)
        {
            UserRepository uRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            FsUser newUser = uRepo.GetUserById(id);

            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/user/edit")]
        public IActionResult Edit(FsUser fsUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
                    userRepo.EditUserInfo(fsUser);
                    return RedirectToAction("Index", new { msg = "Successfully edited User!" });
                }
                catch 
                {
                    return RedirectToAction("Index", new { err = "Failed editing User." });
                }
            }

            return View(fsUser);
        }

        [Route("admin/user/delete")]
        public IActionResult Delete(int id)
        {
            UserRepository userRepo = new UserRepository(_foodScapeContext, _serviceProvider);
            FsUser fsUser = userRepo.GetUserById(id);

            return View(fsUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/user/delete")]
        public async Task<IActionResult> Delete(FsUser fsUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserRepository userRepository = new UserRepository(_foodScapeContext, _serviceProvider);
                    bool res = await userRepository.DeleteUser(fsUser);
                    return RedirectToAction("Index", new { msg = "Successfully deleted User!" });
                }
                catch
                {
                    return RedirectToAction("Index", new { err = "Failed deleting User." });
                }
            }

            return View(fsUser);
        }
    }
}
