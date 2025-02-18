
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Pizzashop_dotnet;
[Index("Email", Name = "users_email_key", IsUnique = true)]

public class ResetPaasswordVModel
{

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("upassword")]
    [StringLength(150)]
    public string Upassword { get; set; } = null!;

    [Column("cupassword")]
    [StringLength(150)]
    public string ConfirmUpassword { get; set; } = null!;
}

