using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzashop_dotnet.Models;

namespace Pizzashop_dotnet.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly PizzaShopContext _context;

    public UserController(PizzaShopContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        var user = _context.Users.Include(r=>r.Role).ToList();
        return View(user);
    }

    public async Task<IActionResult> Create(){
        ViewData["Country"] = new SelectList(_context.Countries, "Countryid", "Name");
        ViewData["State"] = new SelectList(_context.States, "Stateid", "Name");
        ViewData["City"] = new SelectList(_context.Cities, "Cityid", "Name");
        ViewData["Role"] = new SelectList(_context.Roles, "Roleid", "Name");
        return View();
    }


}
