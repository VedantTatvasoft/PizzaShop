using Microsoft.AspNetCore.Mvc;
using Pizzashop_dotnet.Models;

namespace Pizzashop_dotnet.Controllers;

public class AuthenticationController : Controller
{



    private readonly PizzaShopContext _context;

    public AuthenticationController(PizzaShopContext context)
    {
        _context = context;
    }


    public IActionResult Login()
    {
        if(Request.Cookies.TryGetValue("Email", out String ? email)){
            ViewBag.RememberedEmail = email;
        }
         if(Request.Cookies.TryGetValue("Password", out String ? password)){
            ViewBag. RememberedPassword = password;
        }
       
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // [Route("/authentication/validate-user")]
    public ActionResult Login(LoginUser user)
    {
      

        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Upassword))
        {
          

            ViewBag.ErrorMsg = "Empty email and password";
            return View(user);
        }

        User newUser = new User();
        newUser.Email = user.Email;
        newUser.Upassword = user.Upassword;
        var registeredUser = _context.Users.FirstOrDefault(u => u.Email == newUser.Email);
        if (registeredUser == null || registeredUser.Upassword != user.Upassword) // Replace this with a secure hash check
        {
            Console.WriteLine("Invalid");

            ViewBag.ErrorMsg = "Invalid email and password";
            return View(user);
        }

        if(user.RememberMe==true){
            var options = new CookieOptions{
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("Email",newUser.Email);
            Response.Cookies.Append("Password",newUser.Upassword);
        }
        return RedirectToAction("UserList", "User");
    }
}
