using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;

namespace Food_Scape.Controllers
{
    public class TransactionsController : Controller
    {
        FoodScapeContext _foodScapeContext; 
        private IServiceProvider _serviceProvider;

        public TransactionsController(FoodScapeContext foodScapeContext, IServiceProvider serviceProvider)
        {
            _foodScapeContext = foodScapeContext;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method returns a list of all the transactions made through PayPal
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>IActionResult</returns>
        [Authorize(Roles = "Admin")]
        [Route("admin/transactions")]
        public IActionResult Index(string searchString)
        {
            TransactionRepository transactionRepo = new(_foodScapeContext);

            var items = transactionRepo.GetAllTransactions().ToList(); 

            if (!string.IsNullOrEmpty(searchString))
            {
                items = _foodScapeContext
                         .Ipns
                         .Where(x => x.PaymentId == searchString || x.UserId.ToString().Contains(searchString)).ToList();
            }

            return View(items);
        }


        /// <summary>
        /// This method handles the payment success via PayPal. 
        /// </summary>
        /// <param name="psVM"></param>
        /// <returns>Json object</returns>

        [HttpPost]
        public JsonResult PaySuccess([FromBody] PaySuccessVM psVM)
        {
           
            try
            {
                UserRepository uRepo = new UserRepository(_foodScapeContext, _serviceProvider);
                TransactionRepository transactionRepo = new(_foodScapeContext);
                TicketRepository tRepo = new TicketRepository(_foodScapeContext);
                var userEmail = User.Identity?.Name;
                var user = uRepo.GetUserByEmail(userEmail);



                if (user != null)
                {
                        // Associate the payment with the user
                        Ipn ipn = new Ipn
                        {
                            UserId = user.UserId,
                            PaymentId = psVM.PaymentId,
                            Amount = psVM.Amount,
                            Currency = psVM.Currency,
                            PaymentMethod = psVM.PaymentMethod
                        };

                    bool Ipncreate = transactionRepo.createIPN(ipn);

                    if (Ipncreate)
                    {
                        foreach (CartItemVM cartItem in psVM.CartItems)
                        {
                            for (int i = 0; i < cartItem.Quantity; i++)
                            {
                                Ticket ticket = new Ticket
                                {
                                    UserId = user.UserId,
                                    TicketTypeId = cartItem.TypeId
                                };
                                bool ticketCreate = tRepo.CreateTicket(ticket);
                                if (!ticketCreate)
                                {
                                    return Json("Ticket Purchase Failed Please Contact and Admin");
                                }
                            }
                        }
                    };
                }
                else
                {
                    return Json("User not found");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(psVM);
        }



        public IActionResult Confirmation(string confirmationId)
        {
            Ipn transaction = _foodScapeContext.Ipns.FirstOrDefault(t => t.PaymentId == confirmationId);

            return View("Confirmation", transaction);
        }
        public IActionResult DownloadTicketPdf(string paymentId)
        {
            // Get the ticket information for the payment ID
            Ipn transaction = _foodScapeContext.Ipns.FirstOrDefault(t => t.PaymentId == paymentId);

            TransactionRepository tRepo = new TransactionRepository(_foodScapeContext);
            // Generate the PDF file using a library such as iTextSharp
            byte[] pdfBytes = tRepo.GenerateTicketPdf(transaction);

            // Return the PDF file as a FileResult
            return File(pdfBytes, "application/pdf", $"{transaction.PaymentId}.pdf");
        }

    }
}
