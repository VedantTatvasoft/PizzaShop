namespace Pizzashop_dotnet.Services;

public interface IJwtService
{
     public string GenerateJwtToken(string name, string email, string role , string username);
}
