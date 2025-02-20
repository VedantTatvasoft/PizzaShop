using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using Pizzashop_dotnet.Models;
// using Pizzashop_dotnet.ViewModels;

namespace Pizzashop_dotnet.Controllers;
public class ProfileController : Controller
{

    private readonly PizzaShopContext _context;

    public ProfileController(PizzaShopContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {

        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        // var user = _context.Users.FirstOrDefault(u=>u.Email == userEmail);
        var user = await _context.Users.Include(c => c.CountryNavigation).Include(s => s.StateNavigation).Include(ci => ci.CityNavigation).Include(r => r.Role).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("User not found");
        }

        var model = new MyProfileVModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Role = user.Role.Name,
            Username = user.Username,
            Phone = user.Phone,
            Country = user.Country,
            State = user.State,
            City = user.City,
            CountryName = user.CountryNavigation.Name,
            StateName = user.StateNavigation.Name,
            CityName = user.CityNavigation.Name,
            Address = user.Address,
            Zipcode = user.Zipcode
        };
        ViewData["Country"] = new SelectList(_context.Countries, "Countryid", "Name");
        ViewData["State"] = new SelectList(_context.States.Where(s => s.Countryid == model.Country), "Stateid", "Name");
        ViewData["City"] = new SelectList(_context.Cities.Where(c => c.Stateid == model.State), "Cityid", "Name");
        return View(model);
    }
    [HttpGet]
    public JsonResult GetStates(int countryId)
    {
        var states = _context.States.Where(s => s.Countryid == countryId).Select(s => new { s.Stateid, s.Name }).ToList();
        return Json(states);
    }

    [HttpGet]
    public JsonResult GetCities(int stateId)
    {
        var cities = _context.Cities.Where(c => c.Stateid == stateId).Select(c => new { c.Cityid, c.Name }).ToList();
        return Json(cities);
    }

    [HttpPost]
    public async Task<IActionResult> MyProfile(MyProfileVModel model)
    {
        var Email = User.FindFirstValue(ClaimTypes.Email);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        if (user == null)
        {
            NotFound("User not found");
        }

        Console.WriteLine("c1" + user?.Country);
        Console.WriteLine("c2" + model.Country);
        user.Firstname = model.Firstname;
        user.Lastname = model.Lastname;
        // user.Role.Name = model.Role;
        user.Username = model.Username;
        user.Phone = model.Phone;
        user.Country = model.Country;
        user.State = model.State;
        user.City = model.City;
        user.Address = model.Address;
        user.Zipcode = model.Zipcode;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "User");
    }

    public async Task<IActionResult> ChangePassword()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePassword change)
    {
        var Email = User.FindFirstValue(ClaimTypes.Email);
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        if (user == null)
        {
            NotFound("User not found");
        }
        if (!BCrypt.Net.BCrypt.Verify(change.CurrentPassword, user.Upassword))
        {
            ViewBag.ErrorMsg = "Current password is wrong";
            return View();
        }
        if (change.NewPassword != change.ConfirmNewPassword)
        {
            ViewBag.ErrorMsg = "New password doesn't match";
            return View();
        }

        user.Upassword = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "User");
    }

    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies["jwtToken"] != null)
        {
            Response.Cookies.Delete("jwtToken");
        }

        return RedirectToAction("Login", "Authentication");
    }
}
