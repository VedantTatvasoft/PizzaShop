
namespace Pizzashop_dotnet.Models;

public class MyProfileVModel
{
  public string Firstname { get; set; } = null!;
  public string Lastname { get; set; } = null!;
  public string Username { get; set; } = null!;

  public string Email { get; set; } = null!;
  public string Role { get; set; } 

  public string CountryName { get; set; } 
  public int Country { get; set; }


  public int State { get; set; } 
  
  public string StateName { get; set; } 

  public int City { get; set; }

  public string CityName { get; set; } 
  public string Address { get; set; } = null!;

  public string Zipcode { get; set; } = null!;


  public string Phone { get; set; } = null!;
}