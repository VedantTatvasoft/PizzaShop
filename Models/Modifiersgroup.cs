using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("modifiersgroup")]
public partial class Modifiersgroup
{
    [Key]
    [Column("modifiergroupid")]
    public int Modifiergroupid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("categoryid")]
    public int? Categoryid { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("Modifiersgroups")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Modifiergroup")]
    public virtual ICollection<ItemModifierMapping> ItemModifierMappings { get; set; } = new List<ItemModifierMapping>();

    [InverseProperty("Modifiergroup")]
    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
