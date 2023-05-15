using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Food_Scape.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    [Display(Name = "Review")]
    public string? Review1 { get; set; }

    public int? Rating { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual FsUser? User { get; set; }
}
