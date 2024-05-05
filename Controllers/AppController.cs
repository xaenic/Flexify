using Flexify.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Flexify.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        private readonly FlexifyDbContext dbContext;
        public AppController(FlexifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()

        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");
            int userId = 1;
            UserModel? user = new UserModel();
            if (userClaim != null)
            {
                if (int.TryParse(userClaim.Value, out userId))
                {
                    user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    return View(user);
                }
            }
            return View(user);
        }
        public IActionResult Appearance()

        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");
            int userId = 1;
            UserModel? user = new UserModel();
            if (userClaim != null)
            {
                if (int.TryParse(userClaim.Value, out userId))
                {
                    user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    return View(user);
                }
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Account()
        {


            ClaimsPrincipal claimUser = HttpContext.User;

            int userId = 2;
            var userClaim = claimUser.FindFirst("UserId");

            if(userClaim != null)
            {
                if (int.TryParse(userClaim.Value, out userId))
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    return View(user);
                }
            }
          
        
            return NotFound();
        }
        [HttpPost]
        public IActionResult Account(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Enter Your Password!";
                return View(user);
            }

            ClaimsPrincipal claimUser = HttpContext.User;

            int userId = 2;
            var userClaim = claimUser.FindFirst("UserId");


            if (userClaim != null)
            {
                if (!int.TryParse(userClaim.Value, out userId))
                {
                    return NotFound();
                }
            }

            var userUpdate = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (userUpdate == null)
            {
                // User not found, handle accordingly (e.g., return a 404 Not Found response)
                return NotFound();
            }

            if(userUpdate.Password != user.Password)
            {
                TempData["Error"] = "Incorrect Password";
                return View(user);
            }

                
            userUpdate.FirstName = user.FirstName;
            userUpdate.LastName = user.LastName;
            userUpdate.Email = user.Email;
            userUpdate.Username = user.Username;

            TempData["Success"] = "Updated successfully!";
            dbContext.SaveChanges();
            return View(user);
        }
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            int userId = 2;
            var userClaim = claimUser.FindFirst("UserId");


            if (userClaim != null)
            {
                if (!int.TryParse(userClaim.Value, out userId))
                {
                    return NotFound();
                }
            }
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

            return View();
        }
        [HttpPost]
        public IActionResult DeleteAccount(UserModel user)
        {

            ClaimsPrincipal claimUser = HttpContext.User;
            int userId = 2;
            var userClaim = claimUser.FindFirst("UserId");


            if (userClaim != null)
            {
                if (!int.TryParse(userClaim.Value, out userId))
                {
                    return NotFound();
                }
            }

            var users = dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (users == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(users);
            dbContext.SaveChanges();
            TempData["Success"] = "Deleted successfully!";
            return RedirectToAction("Logout", "Auth"); // Redirect to the home page or any other appropriate page
        }



    }
    }
