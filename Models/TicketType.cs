using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class TicketType
{
    public int TicketTypeId { get; set; }

    public string? TicketType1 { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public int? Day { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
