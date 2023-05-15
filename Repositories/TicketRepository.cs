using Food_Scape.Models;

namespace Food_Scape.Repositories
{
    public class TicketRepository
    {
        // Food Scape Context
        FoodScapeContext _context;

        // Add Food Scape Context as context
        public TicketRepository(FoodScapeContext context)
        {
            _context = context;
        }
        // Get All Tickets
        public IEnumerable<Ticket> GetAllTickets()
        {
            // Get all Tickets
            var tickets = _context.Tickets;

            // Return Tickets
            return tickets;
        }

        // Get Ticket By Id
        public Ticket GetTicketById(int? id)
        {
;           // Get Ticket Where Id = id
            var ticket = _context.Tickets.Where(v => v.TicketId == id).FirstOrDefault();

            // Return Ticket
            return ticket;
        }

        public IEnumerable<Ticket> GetAllTicketsForUser(int id)
        {
            var tickets = _context.Tickets.Where(x => x.UserId == id);
            return tickets;
        }

        // Create Ticket
        public bool CreateTicket(Ticket ticket)
        {
            try
            {
                // Add Ticket
                _context.Add(ticket);
                // Save Changes
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Edit Ticket
        public void EditTicket(Ticket ticket)
        {
            // Update Ticket
            _context.Update(ticket);
            // Save Changes
            _context.SaveChanges();
        }

        // Delete Ticket
        public void DeleteTicket(Ticket ticket)
        {
            // Remove Ticket
            _context.Remove(ticket);
            // Save Changes
            _context.SaveChanges();
        }




    }
}
