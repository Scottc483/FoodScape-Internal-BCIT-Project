using Food_Scape.Models;

namespace Food_Scape.Repositories
{
    public class EventRepository
    {
        // Food Scape Context
        FoodScapeContext _context;

        // Add Food Scape Context as context
        public EventRepository(FoodScapeContext context)
        {
            _context = context;
        }

        // Get All Events
        public IEnumerable<Event> GetAllEvents()
        {
            // Get all Events
            var events = _context.Events;

            // Return Events
            return events;
        }

        public List<Event> GetRecordsList()
        {
            return _context.Events.ToList();
        }
        public Event GetEventById(int? id)
        {
;           // Get Event Where Id = id
            var events = _context.Events.Where(v => v.EventId == id).FirstOrDefault();
            
            // Return Event
            return events;
        }
        // Create Event
        public void CreateEvent(Event events)
        {
            // Add Event
            _context.Add(events);
            // Save Changes
            _context.SaveChanges();
        }

        // Edit Event
        public void EditEvent(Event events)
        {
            // Update Event
            _context.Update(events);
            // Save Changes
            _context.SaveChanges();
        }

        // Delete Event
        public void DeleteEvent(Event events)
        {
            // Remove Event
            _context.Remove(events);
            // Save Changes
            _context.SaveChanges();
        }

    }
}
