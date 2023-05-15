using System;
using System.Collections.Generic;

namespace Food_Scape.Models;

public partial class Ipn
{
    public string PaymentId { get; set; } = null!;

    public int? UserId { get; set; }

    public string? Amount { get; set; }

    public string? Currency { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual FsUser? User { get; set; }
}
