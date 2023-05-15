using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class EventVendor
{
    public int EventVendorId { get; set; }

    public int? EventId { get; set; }

    public int? VendorId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Vendor? Vendor { get; set; }
}
