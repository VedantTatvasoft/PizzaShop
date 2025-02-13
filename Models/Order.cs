using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("customerid")]
    public int? Customerid { get; set; }

    [Column("orderdate", TypeName = "timestamp without time zone")]
    public DateTime Orderdate { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [Column("paymentmodeid")]
    public int? Paymentmodeid { get; set; }

    [Column("totalamount")]
    public decimal Totalamount { get; set; }

    [Column("instruction")]
    [StringLength(300)]
    public string Instruction { get; set; } = null!;

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("Paymentmodeid")]
    [InverseProperty("Orders")]
    public virtual Paymentmode? Paymentmode { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
