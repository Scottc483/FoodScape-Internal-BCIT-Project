using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class TicketTypeEvent
{
    public int TicketTypeEventId { get; set; }

    public int? EventId { get; set; }

    public string? Type { get; set; }

    public int? Quantity { get; set; }

    public virtual Event? Event { get; set; }

    public virtual TicketType? TypeNavigation { get; set; }
}
