using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("state")]
public partial class State
{
    [Key]
    [Column("stateid")]
    public int Stateid { get; set; }

    [Column("countryid")]
    public int? Countryid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("Countryid")]
    [InverseProperty("States")]
    public virtual Country? Country { get; set; }

    [InverseProperty("StateNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
