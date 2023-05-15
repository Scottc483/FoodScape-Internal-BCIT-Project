using Food_Scape.Models;
using Microsoft.Extensions.Logging;

namespace Food_Scape.Repositories
{
    public class EventVendorRepository
    {
        FoodScapeContext _foodScapeContext;

        public EventVendorRepository(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }

        // returns list of all EventVendors
        public IEnumerable<EventVendor> GetAllEventVendors() 
        {
            return _foodScapeContext.EventVendors;
        }

        // returns EventVendor where EventVendorId equals parameter id
        public EventVendor GetEventVendorById(int id)
        {
            return _foodScapeContext.EventVendors.Where(x => x.EventVendorId == id).FirstOrDefault();
        }

        // returns EventVendor where EventId equals parameter eventId
        public IEnumerable<EventVendor> GetEventVendorByEventId(int? eventId)
        {
            return _foodScapeContext.EventVendors.Where(x => x.EventId == eventId);
        }

        // returns EventVendor where VendorId equals parameter vendorId
        public IEnumerable<EventVendor> GetEventVendorByVendorId(int? vendorId)
        {
            return _foodScapeContext.EventVendors.Where(x => x.VendorId == vendorId);
        }

        // adds newly created EventVendor to db
        public string CreateEventVendor(EventVendor newEventVendor)
        {
            _foodScapeContext.EventVendors.Add(newEventVendor);
            _foodScapeContext.SaveChanges();
            return "";
        }

        // updates EventVendor record from db
        public string EditEventVendor(EventVendor newEventVendor)
        {
            var oldEventVendor = _foodScapeContext.EventVendors.Where(x => x.EventVendorId == newEventVendor.EventVendorId).FirstOrDefault();
            oldEventVendor.VendorId = newEventVendor.VendorId;
            oldEventVendor.EventId = newEventVendor.EventId;
            _foodScapeContext.SaveChanges();
            return "";
        }

        // deletes EventVendor from db
        public string DeleteEventVendor(EventVendor eventVendor)
        {
            _foodScapeContext.Remove(eventVendor);
            _foodScapeContext.SaveChanges();

            return "";
        }


    }
}
