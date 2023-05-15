using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? UserId { get; set; }

    public int? TicketTypeId { get; set; }

    public virtual TicketType? TicketType { get; set; }

    public virtual FsUser? User { get; set; }
}
