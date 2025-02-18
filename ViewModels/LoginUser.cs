
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;


[Index("Email", Name = "users_email_key", IsUnique = true)]
[Index("Phone", Name = "users_phone_key", IsUnique = true)]
public partial class LoginUser
{
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("upassword")]
    [StringLength(150)]
    public string Upassword { get; set; } = null!;

   

    [Column("rememberMe")]
    public bool RememberMe { get; set; }


}
