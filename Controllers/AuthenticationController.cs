using Microsoft.AspNetCore.Mvc;
using Pizzashop_dotnet.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;



namespace Pizzashop_dotnet.Controllers;

public class AuthenticationController : Controller
{



    private readonly PizzaShopContext _context;
    private readonly IConfiguration _config;

    public AuthenticationController(PizzaShopContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }


    public IActionResult Login()
    {

        if (Request.Cookies["Email"] != null)
        {
            return RedirectToAction("UserList", "User");
        }
        if (Request.Cookies.TryGetValue("Email", out String? email))
        {
            ViewBag.RememberedEmail = email;
        }
        if (Request.Cookies.TryGetValue("Password", out String? password))
        {
            ViewBag.RememberedPassword = password;
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // [Route("/authentication/validate-user")]
    public async Task<ActionResult> Login(LoginUser user)
    {


        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "data type is not valid";
            return View(user);
        }
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Upassword))
        {
            ViewBag.ErrorMsg = "Empty email and password";
            return View(user);
        }

        Console.WriteLine(user.Email);

        var registeredUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (registeredUser == null)
        {
            ViewBag.ErrorMsg = "user not found";
            return View(user);
        }
        // Console.WriteLine("regi  : "+ registeredUser.Upassword);
        bool verifyPassword = BCrypt.Net.BCrypt.Verify(user.Upassword, registeredUser.Upassword);
        // Console.WriteLine("bool  : "+ verifyPassword);

        if (registeredUser == null || !verifyPassword || user.Email != registeredUser.Email) // Replace this with a secure hash check
        {
            Console.WriteLine("Invalid");

            ViewBag.ErrorMsg = "Invalid email and password";
            return View(user);
        }

        if (user.RememberMe == true)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("Email", user.Email, options);
            Response.Cookies.Append("Password", user.Upassword, options);
        }
        return RedirectToAction("UserList", "User");
    }





    public IActionResult ForgotPassword()
    {
        if (Request.Cookies.TryGetValue("Email", out String? email))
        {
            ViewBag.RememberedEmail = email;
        }
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string Email)
    {
        Console.WriteLine("forgottt");
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        if (user == null)
        {
            return Content("Email not found");
        }

        // Store email in a cookie (valid for 10 minutes)
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(10),
            HttpOnly = true,  // Prevent JavaScript access
            Secure = true,    // Send only over HTTPS
            IsEssential = true
        };
        Response.Cookies.Append("ResetEmail", Email, options);

        // Generate reset link (without token)
        string resetLink = Url.Action("ResetPassword", "Authentication", null, Request.Scheme ) ?? "";

        // Send email with reset link
        await SendEmail(user.Email, resetLink);
        // return RedirectToAction("ResetPassword", "Authentication");

        return Content("Password reset link sent to your email.");
    }
    private async Task SendEmail(string email, string resetLink)
    {
        Console.WriteLine("send email");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Pizzashop", _config["SmtpSettings:SenderEmail"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Reset Request";

        message.Body = new TextPart("html")
        {
            Text = $"<p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>"
        };

        try
        {
            using var smtp = new SmtpClient();
            int smtpPort = int.Parse(_config["SmtpSettings:Port"] ?? "465");

            Console.WriteLine("Connecting to SMTP server...");

            await smtp.ConnectAsync(_config["SmtpSettings:Server"], smtpPort, SecureSocketOptions.StartTls);
            Console.WriteLine("Connected successfully!");

            await smtp.AuthenticateAsync(_config["SmtpSettings:SenderEmail"], _config["SmtpSettings:SenderPassword"]);
            Console.WriteLine("Authenticated successfully!");

            await smtp.SendAsync(message);
            Console.WriteLine("Email sent successfully!");

            await smtp.DisconnectAsync(true);
            Console.WriteLine("SMTP Disconnected!");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"SMTP Error: {ex.Message}");
        }
    }
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(String newPassword, String cnewPassword)
    {
        Console.WriteLine(newPassword);
        Console.WriteLine(cnewPassword);
        var workFactor = 13;
        if (newPassword != cnewPassword)
        {
            ViewBag.ErrorMsg = "Password doesn't match";
            return View();
        }
        string userEmail = Request.Cookies["Email"];
        Console.WriteLine(userEmail);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Content("User not found");
        }

        var hashed = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor);
        user.Upassword = hashed;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        Response.Cookies.Delete("Email");
        Response.Cookies.Delete("Password");
        Response.Cookies.Delete("ResetEmail");
        return Content("Password changed successfully");

    }

}
