using Food_Scape.ViewModels;

namespace Food_Scape.ViewModels
{
    public class PaySuccessVM
    {
        public int? UserId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string PaymentId { get; set; } = null!;

        public string? Amount { get; set; }

        public string? Currency { get; set; }

        public string? PaymentMethod { get; set; }

        public CartItemVM[]? CartItems { get; set; }

    }
}
