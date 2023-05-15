namespace Food_Scape.ViewModels
{
    public class CartItemVM
    {
        public int TypeId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public string? Event { get; set; }

        public string Types { get; set; }
    }
}
