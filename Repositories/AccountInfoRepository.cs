using Food_Scape.Models;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Food_Scape.Repositories
{
    public class AccountInfoRepository
    {
        FoodScapeContext _context;

        public AccountInfoRepository(FoodScapeContext context)
        {
            _context = context;
        }

        // Get user object from the database by email address
        public FsUser GetUserByEmail(string email)
        {
            var user = _context.FsUsers.Where(u => u.Email == email).FirstOrDefault();

            return user;
        }

        // Get user account information by user ID, including their purchased tickets and review (if applicable)
        public AccountInfoVM GetUserAccountInfoById(int id)
        {
            IEnumerable<TicketVM> userTickets = _context.Tickets.Join(_context.TicketTypes, t => t.TicketTypeId
                                            , ty => ty.TicketTypeId
                                            , (t, ty) => new
                                            {
                                                t.TicketId,
                                                ty.TicketType1,
                                                ty.Price,
                                                ty.EventId,
                                                t.UserId
                                            })
                                        .Join(_context.Events, ty => ty.EventId, e => e.EventId, (ty, e) => new
                                        {
                                            ty.TicketId,
                                            ty.TicketType1,
                                            e.Name, // Add the EventName column from Events
                                            ty.UserId,
                                            ty.Price
                                        })
                                        .Where(t => t.UserId == id).Select(tty => new TicketVM
                                        {
                                            TicketId = tty.TicketId,
                                            Type = tty.TicketType1,
                                            Event = tty.Name,
                                            Price = tty.Price
                                        });

            var user = _context.FsUsers.Where(u => u.UserId == id).FirstOrDefault();

            // Get user review (if applicable) from the database using ReviewRepository
            ReviewRepository rRepo = new(_context);
            Review? userReview = null;
            if (user != null)
            {
                userReview = rRepo.GetUserReview(user.UserId);
            }

            // Construct an AccountInfoVM object containing the user's account information, purchased tickets, and review (if applicable)
            AccountInfoVM accountInfoVM = new AccountInfoVM()
            {
                User = user!,
                Tickets = userTickets,
                UserReview = userReview
            };

            return accountInfoVM;
        }
    }
}
