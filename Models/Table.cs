using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("tables")]
public partial class Table
{
    [Key]
    [Column("tableid")]
    public int Tableid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("sectionid")]
    public int? Sectionid { get; set; }

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Sectionid")]
    [InverseProperty("Tables")]
    public virtual Section? Section { get; set; }

    [InverseProperty("Table")]
    public virtual ICollection<Waitingtoken> Waitingtokens { get; set; } = new List<Waitingtoken>();
}
