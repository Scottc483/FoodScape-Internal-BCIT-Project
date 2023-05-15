using Food_Scape.Models;

namespace Food_Scape.ViewModels
{
    public class AccountInfoVM
    {
        public IEnumerable<TicketVM> Tickets { get; set; }
        public FsUser User { get; set; }

        public Review? UserReview { get; set; }

    }
}
