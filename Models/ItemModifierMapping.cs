using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("item_modifier_mapping")]
public partial class ItemModifierMapping
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("itemid")]
    public int? Itemid { get; set; }

    [Column("modifiergroupid")]
    public int? Modifiergroupid { get; set; }

    [ForeignKey("Itemid")]
    [InverseProperty("ItemModifierMappings")]
    public virtual Item? Item { get; set; }

    [ForeignKey("Modifiergroupid")]
    [InverseProperty("ItemModifierMappings")]
    public virtual Modifiersgroup? Modifiergroup { get; set; }
}
