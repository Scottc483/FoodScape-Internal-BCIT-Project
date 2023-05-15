using System.ComponentModel.DataAnnotations;

namespace Food_Scape.ViewModels
{
    public class ContactVM
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public bool? Success { get; set; }
        public string? Error { get; set; }
    }
}
