using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("paymentmode")]
public partial class Paymentmode
{
    [Key]
    [Column("paymentmodeid")]
    public int Paymentmodeid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [InverseProperty("Paymentmode")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
