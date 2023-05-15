using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Food_Scape.Controllers
{
  
    public class AccountInfoController : Controller
    {
        private FoodScapeContext _context;
        private IServiceProvider _serviceProvider;

        public AccountInfoController(FoodScapeContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // GET: /AccountInfo
        [Route("/AccountInfo")]
        public IActionResult Index()
        {
            // Check if the user is logged in
            if (User.Identity?.Name == null)
            {
                // Redirect the user to the login page if they're not logged in
                return Redirect("/Identity/Account/Login");
            }
            else
            {
                // Get the user's email address from the logged in user object
                var userEmail = User.Identity!.Name;

                // Get the user object from the database using the user's email address
                UserRepository userRepo = new UserRepository(_context, _serviceProvider);
                var user = userRepo.GetUserByEmail(userEmail);

                // Get the user's account information from the database using their user ID
                AccountInfoRepository accountInfoRepo = new AccountInfoRepository(_context);
                var currentUser = accountInfoRepo.GetUserAccountInfoById(user.UserId);

                // Return the AccountInfoVM object to the view
                return View(currentUser);
            }
        }

        // GET: /AccountInfo/DownloadFile
        public IActionResult DownloadFile()
        {
            // Call the DownloadSinghFile method to get a memory stream containing the file data
            var memory = DownloadSinghFile("document.pdf", "wwwroot\\files");

            // Return the memory stream as a file download
            return File(memory.ToArray(), "application/pdf", "tickets.pdf");
        }

        // Helper method to download a file and return a memory stream containing the file data
        private MemoryStream DownloadSinghFile(string fileName, string uploadPath)
        {
            // Build the path to the file
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, fileName);

            // Create a memory stream to hold the file data
            var memory = new MemoryStream();

            // If the file exists, download the data and add it to the memory stream
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }

            // Reset the position of the memory stream to the beginning
            memory.Position = 0;

            // Return the memory stream
            return memory;
        }
    }
}
