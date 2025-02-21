
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pizzashop_dotnet.Models;

public class UserVModel
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; }

    public bool? Status { get; set; }

    public List<SelectListItem> StatusList { get; set; }
    public int Role { get; set; }


    public int Country { get; set; }


    public int State { get; set; }

    public string? Profileimageurl { get; set; }


    public int City { get; set; }


    public string Address { get; set; } = null!;

    public string Zipcode { get; set; } = null!;


    public string Phone { get; set; } = null!;
}