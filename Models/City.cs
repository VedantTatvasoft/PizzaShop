using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("city")]
public partial class City
{
    [Key]
    [Column("cityid")]
    public int Cityid { get; set; }

    [Column("stateid")]
    public int? Stateid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [ForeignKey("Stateid")]
    [InverseProperty("Cities")]
    public virtual State? State { get; set; }

    [InverseProperty("CityNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
