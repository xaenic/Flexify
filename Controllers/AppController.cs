using Flexify.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data.SqlClient;

namespace Flexify.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        private readonly FlexifyDbContext dbContext;
        private string connectionString;

        public AppController(FlexifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       

        public IActionResult Appearance()

        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");
            int userId = 1;
            UserModel? user = new UserModel();
            PageModel[] pageModel = { new PageModel() };
            PageLayoutModel layoutModel = new PageLayoutModel();
            Socials[] socials = { new Socials() };
            AppearanceModel appearanceSettings = new AppearanceModel(user, pageModel, socials, layoutModel);
            if (userClaim != null)
            {
                if (int.TryParse(userClaim.Value, out userId))
                {
                    user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    appearanceSettings = new AppearanceModel(user, pageModel, socials, layoutModel);
                    return View(appearanceSettings);
                }
            }
            return View(appearanceSettings);
        }
        [HttpGet]
        public IActionResult Account()
        {


            ClaimsPrincipal claimUser = HttpContext.User;

            int userId = 2;
            var userClaim = claimUser.FindFirst("UserId");

            if (userClaim != null)
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

            if (userUpdate.Password != user.Password)
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
        [HttpPost]
        public IActionResult EditBio(UserModel user)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");

            int userId;

            if (userClaim != null && int.TryParse(userClaim.Value, out userId))
            {
                var userToUpdate = dbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (userToUpdate == null)
                {
                    return NotFound();
                }

                userToUpdate.Bio = user.Bio;

                TempData["Success"] = "Bio updated successfully!";
                dbContext.SaveChanges();

                return RedirectToAction("Index", "App");
            }

            return NotFound();
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

        [HttpPost]
        public IActionResult SubmitSocialMediaEmail([FromBody] SocialMediaEmailModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = dbContext.Users.Any(u => u.Id == model.UserId);
                if (!userExists)
                {
                    return Json(new { success = false, message = "User does not exist." });
                }
                var existingEntry = dbContext.email.FirstOrDefault(e => e.user_id == model.UserId && e.SocialType == model.SocialType);
                if (existingEntry != null)
                {
 
                    existingEntry.emailsocial = model.Emails;
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Email updated successfully!" });
                }
                var emailEntity = new Email
                {
                    emailsocial = model.Emails,
                    user_id = model.UserId,
                    SocialType = model.SocialType
                };

                dbContext.email.Add(emailEntity);
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Email added successfully!" });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid data.", errors });
        }


    }
}