using Food_Scape.Models;

namespace Food_Scape.ViewModels
{
    public class VendorVM
    {
        public IEnumerable<EventVM> Events { get; set; }
        public Vendor Vendor { get; set; }
    }
}
