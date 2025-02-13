using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("section")]
public partial class Section
{
    [Key]
    [Column("sectionid")]
    public int Sectionid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [InverseProperty("Section")]
    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

    [InverseProperty("Section")]
    public virtual ICollection<Waitingtoken> Waitingtokens { get; set; } = new List<Waitingtoken>();
}
