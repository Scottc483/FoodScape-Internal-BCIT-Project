using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using iTextSharp.text.pdf.qrcode;
using System;

namespace Food_Scape.Controllers
{
    public class ReviewController : Controller
    {
        private FoodScapeContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IServiceProvider _serviceProvider;

        public ReviewController(FoodScapeContext context, UserManager<IdentityUser> userManager, IServiceProvider serviceProvider)
        {
            _context = context;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }

        // GET: ReviewController
        [Route("reviews")]
        public ActionResult Index()
        {
            try
            {
                // Create a new instance of the ReviewRepository class
                ReviewRepository rRepo = new(_context);

                // Call the GetAllReviews method of the ReviewRepository instance to retrieve all reviews from the database
                var reviews = rRepo.GetAllReviews();

                // If successful, pass the reviews to the View and return the view
                return View(reviews);
            }
            catch (Exception ex)
            {
                // If an exception is caught, set an error message in the ViewBag and return the view
                ViewBag.ErrorMessage = "An error occurred while getting the reviews: " + ex.Message;
                return View();
            }
        }

        // GET: ReviewController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                // Create a new instance of the ReviewRepository class
                ReviewRepository rRepo = new(_context);

                // Call the GetReviewById method of the ReviewRepository instance to retrieve the review with the specified ID from the database
                var review = rRepo.GetReviewById(id);

                // If successful, pass the review to the View and return the view
                return View(review);
            }
            catch (Exception ex)
            {
                // If an exception is caught, set an error message in the ViewBag and return the view
                ViewBag.ErrorMessage = "An error occurred while getting the review: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Review/Create")]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                // Check if the model state is valid
                if (ModelState.IsValid)
                {
                    // Create a new ReviewRepository instance
                    ReviewRepository rRepo = new(_context);

                    try
                    {
                        // Create a new UserRepository instance and get the user by email
                        UserRepository uRepo = new(_context, _serviceProvider);
                        var user = uRepo.GetUserByEmail(collection["UserId"]!);

                        int rating;
                        Review review = new Review();
                        review.Review1 = collection["Review1"];
                        review.UserId = user.UserId;
                        review.UpdatedDate = DateTime.Now;
                        review.CreatedDate = DateTime.Now;

                        // Try to parse the Rating value from the form collection
                        if (int.TryParse(collection["Rating"], out rating))
                        {
                            // If successful, set the Rating property of the review
                            review.Rating = rating;
                        }
                        else
                        {
                            // If unsuccessful, handle the error by setting an error message in the ViewBag and returning the view
                            ViewBag.ErrorMessage = "The Rating value is not valid";
                            return View();
                        }

                        // Create the review using the ReviewRepository instance
                        rRepo.CreateReview(review);

                        // If successful, redirect to the Index action
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        // If an exception is caught, set an error message in the ViewBag and return the view
                        ViewBag.ErrorMessage = "An error occurred while creating this review: Please try again later";
                        return View();
                    }
                }

                // If the model state is invalid, redirect to the Index action
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // If an exception is caught, set an error message in the ViewBag and return the view
                ViewBag.ErrorMessage = "An error occurred while creating this review: Please try again later";
                return View();
            }
        }

        // GET: ReviewController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                ReviewRepository rRepo = new(_context);
                var review = rRepo.GetReviewById(id);
                return View(review);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while editing this review: Please try again later";
                return View();
            }
        }

        // POST: ReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // Retrieve the review from the database using the ReviewRepository instance
                ReviewRepository rRepo = new(_context);
                var review = rRepo.GetReviewById(id);

                // Update the review properties based on the form collection values
                review.Review1 = collection["Review1"];
                review.Rating = int.Parse(collection["Rating"]!);
                review.UpdatedDate = DateTime.Now;

                // Save the changes to the database using the ReviewRepository instance
                rRepo.UpdateReview(review);

                // If successful, redirect to the Index action
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // If an exception is caught, set an error message in the ViewBag and return the view
                ViewBag.ErrorMessage = "An error occurred while updating this review: Please try again later";
                return View();
            }
        }


        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                // Create a new instance of the ReviewRepository class
                ReviewRepository rRepo = new(_context);

                // Call the DeleteReview method of the ReviewRepository instance to delete the review with the specified ID
                rRepo.DeleteReview(id);

                // If successful, redirect to the Index action
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // If an exception is caught, set an error message in the ViewBag and return the view
                ViewBag.ErrorMessage = "An error occurred while deleting this review: Please try again later";
                return View();
            }
        }

    }

}