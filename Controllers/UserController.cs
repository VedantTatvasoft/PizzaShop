using Microsoft.AspNetCore.Mvc;
using Pizzashop_dotnet.Models;

namespace Pizzashop_dotnet.Controllers;

public class UserController : Controller
{



    private readonly PizzaShopContext _context;

    public UserController(PizzaShopContext context)
    {
        _context = context;
    }


    public IActionResult UserList()
    {
        return View();
    }


}
