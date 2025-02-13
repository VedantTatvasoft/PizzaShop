using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("rating")]
public partial class Rating
{
    [Key]
    [Column("ratingid")]
    public int Ratingid { get; set; }

    [Column("customerid")]
    public int? Customerid { get; set; }

    [Column("orderid")]
    public int? Orderid { get; set; }

    [Column("foodrating")]
    public int Foodrating { get; set; }

    [Column("service")]
    public int Service { get; set; }

    [Column("ambience")]
    public int Ambience { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Ratings")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("Ratings")]
    public virtual Order? Order { get; set; }
}
