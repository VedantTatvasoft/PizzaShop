using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzashop_dotnet.Models;
using X.PagedList;

namespace Pizzashop_dotnet.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly PizzaShopContext _context;

    public UserController(PizzaShopContext context)
    {
        _context = context;
    }


    public IActionResult Index(string search, int? page )
    {
        int pageSize = 3;
        int pageNumber = page ?? 1;
        // var user = _context.Users.Include(r => r.Role).ToList();
        var user = _context.Users.AsQueryable();
        if(!string.IsNullOrEmpty(search)){
            user = user.Where(u => u.Firstname.Contains(search) || u.Email.Contains(search));
        }
        var pagedUsers = user.OrderBy(u => u.Userid).ToPagedList(pageNumber, pageSize);
        return View(pagedUsers);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Country"] = new SelectList(_context.Countries, "Countryid", "Name");
        ViewData["Role"] = new SelectList(_context.Roles, "Roleid", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (user == null)
        {
            return NotFound("user not found");
        }
        user.Upassword = BCrypt.Net.BCrypt.HashPassword(user.Upassword);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "User");
    }
}
