using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("modifiers")]
public partial class Modifier
{
    [Key]
    [Column("modifierid")]
    public int Modifierid { get; set; }

    [Column("modifiergroupid")]
    public int? Modifiergroupid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("rate", TypeName = "money")]
    public decimal Rate { get; set; }

    [Column("unit")]
    [StringLength(20)]
    public string Unit { get; set; } = null!;

    [Column("quantity")]
    public decimal Quantity { get; set; }

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Modifiergroupid")]
    [InverseProperty("Modifiers")]
    public virtual Modifiersgroup? Modifiergroup { get; set; }
}
