using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? Address { get; set; }

    public int? Capacity { get; set; }

    public string? Name { get; set; }

    public int? AgeRestriction { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<EventVendor> EventVendors { get; } = new List<EventVendor>();

    public virtual ICollection<TicketType> TicketTypes { get; } = new List<TicketType>();
}
