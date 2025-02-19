using Microsoft.AspNetCore.Mvc;
using Pizzashop_dotnet.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Pizzashop_dotnet.Services;

namespace Pizzashop_dotnet.Controllers;

public class AuthenticationController : Controller
{



    private readonly PizzaShopContext _context;
    private readonly IConfiguration _config;

    private IJwtService _jwtServices;

    public AuthenticationController(PizzaShopContext context, IConfiguration config, IJwtService jwtServices)
    {
        _context = context;
        _config = config;
        _jwtServices = jwtServices;
    }


    public IActionResult Login()
    {

        var token = Request.Cookies["jwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validate token using the same parameters as in your JWT middleware
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return RedirectToAction("Index", "User");
            }
            catch (SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwtToken");
            }
            catch (Exception)
            {
                // Token is invalid in some other way; remove it.
                Response.Cookies.Delete("jwtToken");
            }
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
        
        var registeredUser = await _context.Users.Include(u=> u.Role).FirstOrDefaultAsync();

        // var registeredUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (registeredUser == null)
        {
            ViewBag.ErrorMsg = "user not found";
            return View(user);
        }
        Console.WriteLine(registeredUser.Role.Name);




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
            // Response.Cookies.Append("Password", user.Upassword, options);
        }


        var tokenString = _jwtServices.GenerateJwtToken(registeredUser.Firstname + " " + registeredUser.Lastname, registeredUser.Email, registeredUser.Role.Name , registeredUser.Username);
        Response.Cookies.Append("jwtToken", tokenString);
        // return Ok(new { token = tokenString });
        return RedirectToAction("Index", "User");
    }



    public IActionResult ForgotPassword()
    {
        if (Request.Cookies.TryGetValue("Email", out String? email))
        {
            ViewBag.RememberedEmail = email;
            ViewBag.SendMail = "Reset password link has been send to your email account";
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
        string resetLink = Url.Action("ResetPassword", "Authentication", new { email = user.Email }, Request.Scheme) ?? "";

        // Send email with reset link
        await SendEmail(user.Email, resetLink);
        TempData["SuccessMessage"] = "A reset password link has been sent to your email account";
        // ViewBag.ErrorMsg = "Invalid email and password";
        return RedirectToAction("ForgotPassword", "Authentication");

        // return Content("Password reset link sent to your email.");
    }
    private async Task SendEmail(string email, string resetLink)
    {
        Console.WriteLine("send email");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Pizzashop", _config["SmtpSettings:SenderEmail"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Reset Request";

        // message.Body = new TextPart("html")
        // {

        //     Text = $"<p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>"
        // };

        var bodyBuilder = new BodyBuilder();

        var imagePath = "C:/Project/Pizzashop-dotnet/wwwroot/images/logos/pizzashop_logo.png";
        var image = bodyBuilder.LinkedResources.Add(imagePath);

        image.ContentId = "pizzashop_logo";

        bodyBuilder.HtmlBody = $@"
        <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Password Reset</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f8f9fa;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                width: 70%;
                                background: #ffffff;
                                margin: 20px 0;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                background-color: #1f73ae;
                                color: white;
                                text-align: center;
                                padding: 20px;
                                border-top-left-radius: 8px;
                                border-top-right-radius: 8px;
                                display: flex;
                                align-items: center;
                                justify-content: center;
                            }}
                            .header img {{
                                height: 80px;
                                width: 100px;
                                margin-right: 10px;
                            }}
                            .header h1 {{
                                display: inline;
                                font-size: 24px;
                                vertical-align: middle;
                            }}
                            .content {{
                                padding: 20px;
                            }}
                            .content p {{
                                font-size: 16px;
                                color: #333;
                                line-height: 1.5;
                            }}
                            .content a {{
                                color: #1f73ae;
                                text-decoration: none;
                                font-weight: bold;
                            }}
                            .content a:hover {{
                                text-decoration: underline;
                            }}
                            .note {{
                                font-size: 14px;
                                color: #555;
                                margin-top: 10px;
                            }}
                            .note span {{
                                color: #ffc107;
                                font-weight: bold;
                            }}

                            

                            @media (max-width: 450px) {{
                                .container {{
                                    width: 100%;
                                }}
                    
                                .header h1 {{
                                    font-size: 20px;
                                }}
                    
                                .header {{                
                                    display: flex;
                                    align-items: center;
                                    justify-content: start;
                                }}
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <img src='cid:pizzashop_logo' class='logo-img' alt='PizzaShop'>
                                <h1 class='pizzashop-header'>Pizza-Shop</h1>
                            </div>
                            <div class='content'>
                                <p>Pizza-Shop.</p>
                                <p>Please click <a href='{resetLink}'>here</a> to reset your password.</p>
                                <p>If you encounter any issues or have any questions, please do not hesitate to contact our support team.</p>
                                <p class='note'><span>Important Note:</span> For security reasons, this link will expire in 15 minutes. If you did not request a password reset, please ignore this email.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

        message.Body = bodyBuilder.ToMessageBody();

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
    public IActionResult ResetPassword(String? email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            return View("ForgotPassword");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPaasswordVModel logedUser)
    {
        // Console.WriteLine(newPassword);
        // Console.WriteLine(cnewPassword);
        var workFactor = 13;
        if (logedUser.Upassword != logedUser.ConfirmUpassword)
        {
            ViewBag.ErrorMsg = "Password doesn't match";
            return View();
        }
        // string userEmail = Request.Cookies["Email"];
        // Console.WriteLine(userEmail);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == logedUser.Email);
        if (user == null)
        {
            return Content("User not found");
        }

        var hashed = BCrypt.Net.BCrypt.HashPassword(logedUser.Upassword, workFactor);
        user.Upassword = hashed;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        Response.Cookies.Delete("Email");
        Response.Cookies.Delete("Password");
        Response.Cookies.Delete("ResetEmail");
        return Content("Password changed successfully");

    }

}
