
using Food_Scape.Models;
using Food_Scape.ViewModels;

namespace Food_Scape.Repositories
{
    public class VendorRepository
    {
        FoodScapeContext _context;

        public VendorRepository(FoodScapeContext context)
        {
            _context = context;
        }

        //Get all the vendors form the db.
        public IEnumerable<Vendor> GetRecords()
        {
            var vendors = _context.Vendors;
            return vendors;
        }

        //Get vendors and their linked events in a VendorVM.
        public IEnumerable<VendorVM> GetRecordsVM()
        {
            var vendors = _context.Vendors;
            IEnumerable<EventVM> eventsVendor;
            List<VendorVM> vendorsVM = new List<VendorVM>();

            foreach (var vendor in vendors)
            {
                IEnumerable<EventVM> events = _context.Events.Join(_context.EventVendors, e => e.EventId, ev => ev.EventId,
                    (e, ev) => new EventVM()
                    {
                        EventId = e.EventId,
                        Name = e.Name,
                        VendorId = ev.VendorId,
                    }).Where(e => e.VendorId == vendor.VendorId);

                eventsVendor = events;

                VendorVM vendorVM = new VendorVM()
                {
                    Events = eventsVendor,
                    Vendor = vendor
                };

                vendorsVM.Add(vendorVM);
            }


            return vendorsVM;
        }

        //Get vendors by VendorType.
        public IEnumerable<Vendor> GetRecordsByType(string type)
        {
            var vendors = _context.Vendors.Where(x => x.VendorType == type);

            return vendors;
        }

        //Get vendors and their linked events by VendorType in a VendorVM.
        public IEnumerable<VendorVM> GetRecordsByTypeVM(string type)
        {
            var vendors = _context.Vendors.Where(x => x.VendorType == type);

            IEnumerable<EventVM> eventsVendor;
            List<VendorVM> vendorsVM = new List<VendorVM>();

            foreach (var vendor in vendors)
            {
                IEnumerable<EventVM> events = _context.Events.Join(_context.EventVendors, e => e.EventId, ev => ev.EventId,
                    (e, ev) => new EventVM()
                    {
                        EventId = e.EventId,
                        Name = e.Name,
                        VendorId = ev.VendorId,
                    }).Where(e => e.VendorId == vendor.VendorId);

                eventsVendor = events;

                VendorVM vendorVM = new VendorVM()
                {
                    Events = eventsVendor,
                    Vendor = vendor
                };

                vendorsVM.Add(vendorVM);
            }

            return vendorsVM;
        }

        // Get Records List form the db.
        public List<Vendor> GetRecordsList()
        {
            return _context.Vendors.ToList();
        }

        //Get one vendor by id form the db.
        public Vendor GetRecord(int? id)
        {
            var vendor = _context.Vendors.Where(v => v.VendorId == id).FirstOrDefault();

            return vendor;
        }

        //Create a new vendor in db.
        public void CreateRecord(Vendor vendor)
        {
            _context.Add(vendor);
            _context.SaveChanges();
        }

        //Edit an existing vendor in db.
        public void EditRecord(Vendor vendor)
        {
            _context.Update(vendor);
            _context.SaveChanges();
        }

        //Delete a vendor form the db.
        public void DeleteRecord(Vendor vendor)
        {
            _context.Remove(vendor);
            _context.SaveChanges();
        }
    }
}
