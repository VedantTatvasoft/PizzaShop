using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("items")]
public partial class Item
{
    [Key]
    [Column("itemid")]
    public int Itemid { get; set; }

    [Column("categoryid")]
    public int? Categoryid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("type")]
    [StringLength(30)]
    public string Type { get; set; } = null!;

    [Column("rate", TypeName = "money")]
    public decimal Rate { get; set; }

    [Column("quantity")]
    public decimal Quantity { get; set; }

    [Column("unit")]
    [StringLength(20)]
    public string Unit { get; set; } = null!;

    [Column("available")]
    public bool? Available { get; set; }

    [Column("defaulttax")]
    public bool? Defaulttax { get; set; }

    [Column("taxpercentage")]
    public bool? Taxpercentage { get; set; }

    [Column("shortcode")]
    [StringLength(30)]
    public string Shortcode { get; set; } = null!;

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("itemimage")]
    [StringLength(300)]
    public string? Itemimage { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("Items")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ItemModifierMapping> ItemModifierMappings { get; set; } = new List<ItemModifierMapping>();
}
