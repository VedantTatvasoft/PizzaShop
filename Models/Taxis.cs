using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("taxes")]
public partial class Taxis
{
    [Key]
    [Column("taxid")]
    public int Taxid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("type")]
    [StringLength(30)]
    public string Type { get; set; } = null!;

    [Column("amount", TypeName = "money")]
    public decimal Amount { get; set; }

    [Column("isenable")]
    public bool? Isenable { get; set; }

    [Column("isdefault")]
    public bool? Isdefault { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }
}
