using Food_Scape.Models;

namespace Food_Scape.ViewModels
{
    public class ActivitiesViewModel
    {
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }

}
