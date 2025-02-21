using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop_dotnet.Models;

namespace Pizzashop_dotnet.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly PizzaShopContext _context;

    public UserController(PizzaShopContext context)
    {
        _context = context;
    }


    public IActionResult Index(string searchString = "", int page = 1)
    {
        int pageSize = 2;
        ViewBag.SearchString = searchString;

        var user = _context.Users.Include(r => r.Role).Where(d => d.Isdeleted == false).ToList();
        var totalItems = user.Count();
        var items = user.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        if (!string.IsNullOrEmpty(searchString))
        {
            items = user.Where(s => s.Firstname.Contains(searchString) || s.Email.Contains(searchString)).ToList();
        }
        ViewBag.StartAt = (page - 1) * pageSize + 1;
        ViewBag.EndAt = ((page - 1) * pageSize) + items.Count;
        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalItems = totalItems;

        ViewBag.TotalPage = (int)Math.Ceiling((double)totalItems / pageSize);
        return View(items);
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

    [HttpPost]
    public async Task<IActionResult> Delete(string email)
    {

        Console.WriteLine("Email : " + email);
        var user = _context.Users.FirstOrDefault(e => e.Email == email);
        user.Isdeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "User");
    }

    public async Task<IActionResult> Edit()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var user = _context.Users.FirstOrDefault(e => e.Email == userEmail);
        if (user == null)
        {
            return NotFound("User not found !!!");
        }



        ViewData["Status"] = user.Status;

        var model = new UserVModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Password = user.Upassword,
            Role = (int)user.Roleid,
            Username = user.Username,
            Status = user.Status,
            StatusList = new List<SelectListItem>{
                new SelectListItem{Value = "true" , Text = "Active"},
                new SelectListItem{Value = "false" , Text = "Inactive"}
            },
            Profileimageurl = user.Profileimageurl,
            Country = user.Country,
            State = user.State,
            City = user.City,
            Phone = user.Phone,
            Address = user.Address,
            Zipcode = user.Zipcode
        };

        ViewData["Country"] = new SelectList(_context.Countries, "Countryid", "Name");
        ViewData["State"] = new SelectList(_context.States.Where(c => c.Countryid == user.Country), "Stateid", "Name");
        ViewData["City"] = new SelectList(_context.Cities.Where(s => s.Stateid == user.State), "Cityid", "Name");
        ViewData["ROle"] = new SelectList(_context.Roles, "Roleid", "Name");

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(UserVModel user)
    {
        if (user == null)
        {
            return NotFound("User not found");
        }
        var findUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
        findUser.Firstname = user.Firstname;
        findUser.Lastname = user.Lastname;
        findUser.Email = user.Email;
        findUser.Upassword = user.Password;
        findUser.Roleid = user.Role;
        findUser.Username = user.Username;
        findUser.Status = user.Status;
        if (user.Profileimageurl != null)
        {
            findUser.Profileimageurl = user.Profileimageurl;
        }
        findUser.Country = user.Country;
        findUser.State = user.State;
        findUser.City = user.City;
        findUser.Phone = user.Phone;
        findUser.Address = user.Address;
        findUser.Zipcode = user.Zipcode;

        // _context.Users.Update(findUser);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "User");
    }
}
