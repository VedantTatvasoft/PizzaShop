using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
[Index("Phone", Name = "users_phone_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("userid")]
    public int Userid { get; set; }

    [Column("firstname")]
    [StringLength(30)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(30)]
    public string Lastname { get; set; } = null!;

    [Column("username")]
    [StringLength(30)]
    public string Username { get; set; } = null!;

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("upassword")]
    [StringLength(150)]
    public string Upassword { get; set; } = null!;

    [Column("profileimageurl")]
    [StringLength(300)]
    public string? Profileimageurl { get; set; }

    [Column("country")]
    public int Country { get; set; }

    [Column("state")]
    public int State { get; set; }

    [Column("city")]
    public int City { get; set; }

    [Column("address")]
    [StringLength(300)]
    public string Address { get; set; } = null!;

    [Column("zipcode")]
    [StringLength(30)]
    public string Zipcode { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    [Required(ErrorMessage = "Phone Number Required!")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                       ErrorMessage = "Entered phone format is not valid.")]
    [Column("phone")]
    [StringLength(30)]
    public string Phone { get; set; } = null!;

    [Column("status")]
    public bool? Status { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    [StringLength(30)]
    public string? Createdby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedby")]
    [StringLength(30)]
    public string? Updatedby { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("City")]
    [InverseProperty("Users")]
    public virtual City CityNavigation { get; set; } = null!;

    [ForeignKey("Country")]
    [InverseProperty("Users")]
    public virtual Country CountryNavigation { get; set; } = null!;

    [ForeignKey("Roleid")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }

    [ForeignKey("State")]
    [InverseProperty("Users")]
    public virtual State StateNavigation { get; set; } = null!;
}
