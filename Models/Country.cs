using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("country")]
public partial class Country
{
    [Key]
    [Column("countryid")]
    public int Countryid { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [InverseProperty("Country")]
    public virtual ICollection<State> States { get; set; } = new List<State>();

    [InverseProperty("CountryNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
