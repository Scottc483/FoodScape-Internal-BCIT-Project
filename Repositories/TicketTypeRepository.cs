using Food_Scape.Models;

namespace Food_Scape.Repositories
{
    public class TicketTypeRepository
    {
        // Food Scape Context
        FoodScapeContext _context;
        
        // Add Food Scape Context as context 
        public TicketTypeRepository(FoodScapeContext context)
        {
            _context = context;
        }

        // Get All Ticket Types 
        public IEnumerable<TicketType> GetAllTicketTypes()
        {
            // Get all Ticket Types
            var ticketType = _context.TicketTypes;

            // Return Ticket Types
            return ticketType;
        }



        // Get Ticket Type By Id
        public TicketType GetTicketType(int? type)
        {
           // Get Ticket Type Where Type = type
            var ticketType = _context.TicketTypes.Where(v => v.TicketTypeId == type).FirstOrDefault();

            // Return Ticket Type
            return ticketType;
        }



        // Create Ticket Type 
        public void CreateTicketType(TicketType ticketType)
        {
            // Add Ticket Type
            _context.Add(ticketType);
            // Save Changes
            _context.SaveChanges();
        }

        // Edit Ticket Type
        public void EditTicketType(TicketType ticketType)
        {
            // Update Ticket Type
            _context.Update(ticketType);
            // Save Changes
            _context.SaveChanges();
        }

        // Delete Ticket Type
        public void DeleteTicketType(TicketType ticketType)
        {
            // Remove Ticket Type
            _context.Remove(ticketType);
            // Save Changes
            _context.SaveChanges();
        }
    }
}
