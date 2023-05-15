using Food_Scape.Models;

namespace Food_Scape.ViewModels
{
    public class HomeIndexVM
    {
        public IEnumerable<Vendor>? Vendors { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }
    }
}
