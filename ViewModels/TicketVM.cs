using Food_Scape.Models;

namespace Food_Scape.ViewModels
{
    public class TicketVM
    {
        public int TicketId { get; set; }

        public string Type { get; set; }

        public string Event { get; set; }

        public decimal? Price { get; set; }
    }
}
