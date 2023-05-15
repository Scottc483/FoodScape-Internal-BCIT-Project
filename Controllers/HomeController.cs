using Food_Scape.Models;
using Food_Scape.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Food_Scape.ViewModels;
using System.Diagnostics;

namespace Food_Scape.Controllers
{
    public class HomeController : Controller
    {
        
        FoodScapeContext _foodScapeContext;

        public HomeController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }


        // Home Page For Users
        public IActionResult Index()
        {
            

            // Vendor Repository to get all vendors
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            var vendors = vendorRepo.GetRecords();

            ReviewRepository reviewRepo = new ReviewRepository(_foodScapeContext);
            var reviews = reviewRepo.GetReviewTestemonials();


            HomeIndexVM viewModel = new()
            {
                Vendors = vendors,
                Reviews = reviews
            };


            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //FAQ/Help Page for all users
        [HttpGet("FAQ")]
        public IActionResult FAQ(bool? success, string? error)
        {
            var viewModel = new ContactVM
            {
                Success = success ?? false,
                Error = error ?? string.Empty,
            };

            return View(viewModel);
        }

        // Payment Page For Users
        public IActionResult Payment()
        {
            return View();
        }

        // Pricing Page For Users
        public IActionResult Pricing()
        {
            return View();
        }

        // Activities Page For Users
        [HttpGet]
        public IActionResult Activites(string sort)
        {

            // Vendor Repository to get all vendors
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            var vendors = vendorRepo.GetRecords();

            // Event Repository to get all events
            EventRepository eventRepo = new EventRepository(_foodScapeContext);
            var events = eventRepo.GetAllEvents();


            // Sort vendors based on selected criteria (Currently not using)
            switch (sort)
            {
                case "name":
                    vendors = vendors.OrderBy(v => v.Name).ToList();
                    break;
                case "description":
                    vendors = vendors.OrderBy(v => v.Description).ToList();
                    break;
                case "type":
                    vendors = vendors.OrderBy(v => v.VendorType).ToList();
                    break;
            }

            // View Model to pass vendors and events to view
            var viewModel = new ActivitiesViewModel
            {
                Vendors = vendors,
                Events = events
            };

            return View(viewModel);
        }


        // Admin Page For Users
        public IActionResult Admin()
        {
            return View();
        }
        // Account Info Page For Users
        public IActionResult AccountInfo()
        {
            return View();
        }

    }
}