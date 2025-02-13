using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("customer")]
[Index("Email", Name = "customer_email_key", IsUnique = true)]
[Index("Phonenumber", Name = "customer_phonenumber_key", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("customerdate", TypeName = "timestamp without time zone")]
    public DateTime Customerdate { get; set; }

    [Column("phonenumber")]
    [StringLength(50)]
    public string Phonenumber { get; set; } = null!;

    [Column("comment")]
    [StringLength(100)]
    public string? Comment { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("table_id")]
    public int? TableId { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Customer")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [InverseProperty("Customer")]
    public virtual ICollection<Waitingtoken> Waitingtokens { get; set; } = new List<Waitingtoken>();
}
