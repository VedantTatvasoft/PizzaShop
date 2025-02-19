namespace Pizzashop_dotnet.Models;

public class ChangePassword

{

    public string Email { get; set; } = null!;
    public string CurrentPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;

    public string ConfirmNewPassword { get; set; } = null!;
}
