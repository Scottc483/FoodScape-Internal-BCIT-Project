using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Food_Scape.Models;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? VendorType { get; set; }

    public string? Location { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<EventVendor> EventVendors { get; } = new List<EventVendor>();
}
