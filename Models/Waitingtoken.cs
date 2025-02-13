using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("waitingtoken")]
public partial class Waitingtoken
{
    [Key]
    [Column("waitingid")]
    public int Waitingid { get; set; }

    [Column("waitingtoken")]
    public int Waitingtoken1 { get; set; }

    [Column("customerid")]
    public int? Customerid { get; set; }

    [Column("tableid")]
    public int? Tableid { get; set; }

    [Column("noofperson")]
    public int Noofperson { get; set; }

    [Column("sectionid")]
    public int? Sectionid { get; set; }

    [Column("requested_time", TypeName = "timestamp without time zone")]
    public DateTime? RequestedTime { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Waitingtokens")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("Sectionid")]
    [InverseProperty("Waitingtokens")]
    public virtual Section? Section { get; set; }

    [ForeignKey("Tableid")]
    [InverseProperty("Waitingtokens")]
    public virtual Table? Table { get; set; }
}
