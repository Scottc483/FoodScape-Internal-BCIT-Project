using Food_Scape.Models;
using System.ComponentModel.DataAnnotations;

namespace Food_Scape.ViewModels
{
    public class ReviewVM
    {
        public int ReviewId { get; set; }
        public int? UserId { get; set; }
        public string? Review { get; set; }
        public int? Rating { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
