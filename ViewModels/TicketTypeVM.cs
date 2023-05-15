namespace Food_Scape.ViewModels
{
    public class TicketTypeVM
    {
        public int TicketTypeId { get; set; }
        public string TicketType1 { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int Day { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? EventId { get; set; }
        public string Name { get; set; }

    }
}
