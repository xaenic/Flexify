using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Flexify.Controllers
{
    public class AuthController : Controller
    {
        private readonly FlexifyDbContext dbContext;
        public AuthController(FlexifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;


            if ( claimUser.Identity.IsAuthenticated)
                return RedirectToAction("", "App");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var userFound = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userFound == null)
            {
                TempData["Error"] = "Account Not Found";
                return View(user);
            }
            if (userFound.Password != user.Password)
            {
                TempData["Error"] = "Incorrect password";
                return View(user); // Return the view with an error message
            }
            
            List<Claim> claims =  new List<Claim>() { 
                new Claim(ClaimTypes.NameIdentifier, userFound.Email),
                new Claim("UserId", userFound.Id +""),
            };

            ClaimsIdentity claimsIdentity =  new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                 IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties
                );
            

            // Authentication successful, redirect to the home page
            return RedirectToAction("", "App");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ViewResult> Register(UserModel user)
        {
            if(!ModelState.IsValid) {
                return View();
            }
            var userFound = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if(userFound != null)
            {
                TempData["Error"] = "Email already exists";
                return View();
            }
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            TempData["Success"] = "Successfully Registered!";
            ModelState.Clear();
            return View(new UserModel());
           
        }
    }
}
