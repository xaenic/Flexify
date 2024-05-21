﻿using Flexify.Models;
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
            Theme theme = new Theme();
            Socials[] socials = { new Socials() };
            PostModel[] post = new PostModel[0];
            PageModel[] page = new PageModel[0];
            AppearanceModel appearanceSettings = new AppearanceModel(user, page , socials, theme, post);
            if (userClaim != null)
            {
                if (int.TryParse(userClaim.Value, out userId))
                {
                    user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    var posts = dbContext.Posts.Where(p => p.UserId == userId).ToArray();
                    socials = dbContext.Socials.Where(p => p.user_id == userId).ToArray();
                    page = dbContext.page.Where(p => p.UserId == userId).ToArray();
                    appearanceSettings = new AppearanceModel(user, page, socials, theme, posts);
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
                var existingEntry = dbContext.Socials.FirstOrDefault(e => e.user_id == model.UserId && e.SocialType == model.SocialType);
                if (existingEntry != null)
                {

                    existingEntry.emailsocial = model.Emails;
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Email updated successfully!" });
                }
                var emailEntity = new Socials
                {
                    emailsocial = model.Emails,
                    user_id = model.UserId,
                    SocialType = model.SocialType
                };

                dbContext.Socials.Add(emailEntity);
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Email added successfully!" });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid data.", errors });
        }
        [HttpPost]
        public IActionResult DeleteSocialMediaEmail([FromBody] SocialMediaEmailModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var userExists = dbContext.Users.Any(u => u.Id == model.UserId);
            if (!userExists)
            {
                return Json(new { success = false, message = "User does not exist." });
            }

            var socialMediaEmail = dbContext.Socials.FirstOrDefault(e => e.Id == model.Id && e.user_id == model.UserId && e.SocialType == model.SocialType);
            if (socialMediaEmail == null)
            {
                return Json(new { success = false, message = "Email entry not found." });
            }

            dbContext.Socials.Remove(socialMediaEmail);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Email deleted successfully!" });
        }


        [HttpPost]

        public IActionResult Theme(Theme model)
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
                }
            }

            if (ModelState.IsValid)
            {
                var existingTheme = dbContext.themes.FirstOrDefault(t => t.user_Id == userId);
                if (existingTheme != null)
                {
                    existingTheme.SelectedTheme = model.SelectedTheme;
                }
                else
                {
                    var newTheme = new Theme
                    {
                        SelectedTheme = model.SelectedTheme,
                        user_Id = userId
                    };
                    dbContext.themes.Add(newTheme);
                }

                dbContext.SaveChanges();

                return RedirectToAction("Index", "App");
            }
            return View(model);
        }




        [HttpPost]
        public IActionResult UploadProfileImage(IFormFile profileImage)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");

            if (userClaim == null || !int.TryParse(userClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            if (profileImage != null && profileImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    profileImage.CopyTo(ms);
                    user.ProfileImage = ms.ToArray();
                }

                dbContext.SaveChanges();
            }

            return Ok(new { message = "Profile image uploaded successfully" });
        }

        [HttpDelete]
        public IActionResult RemoveProfileImage()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");

            if (userClaim == null || !int.TryParse(userClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            user.ProfileImage = null;
            dbContext.SaveChanges();

            return Ok(new { message = "Profile image removed successfully" });
        }
        [HttpPost]
        public async Task<IActionResult> Post(string post_content, IFormFile image_upload)
        {
            if (string.IsNullOrWhiteSpace(post_content) && (image_upload == null || image_upload.Length == 0))
            {
                TempData["Error"] = "Post content or image is required.";
                return RedirectToAction("Appearance");
            }

            ClaimsPrincipal claimUser = HttpContext.User;
            var userClaim = claimUser.FindFirst("UserId");

            if (userClaim != null && int.TryParse(userClaim.Value, out int userId))
            {
                var post = new PostModel
                {
                    Content = post_content,
                    UserId = userId
                };

                if (image_upload != null && image_upload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image_upload.CopyToAsync(memoryStream);
                        post.Image = memoryStream.ToArray();
                    }
                }

                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
                TempData["Success"] = "Post created successfully!";
            }
            else
            {
                TempData["Error"] = "User not found.";
            }

            return RedirectToAction("Appearance");
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                TempData["Error"] = "Post not found.";
            }
            else
            {
                dbContext.Posts.Remove(post);
                await dbContext.SaveChangesAsync();
                TempData["Success"] = "Post deleted successfully!";
            }

            return RedirectToAction("Appearance");
        }
        [HttpPost]
        public IActionResult AddContent(PageModel page)
        {
            if (ModelState.IsValid)
            {
             
                dbContext.page.Add(page);
                dbContext.SaveChanges();

                TempData["Success"] = "Content added successfully!";
                return RedirectToAction("Index", "App");
            }
            else
            {
                return View(page);
            }
        }
        [HttpPost]
        public IActionResult DeletePage(int pageId)
        {
            var page = dbContext.page.FirstOrDefault(p => p.Id == pageId);
            if (page == null)
            {
                return NotFound();
            }

            dbContext.page.Remove(page);
            dbContext.SaveChanges();

            TempData["Success"] = "Page deleted successfully!";
            return RedirectToAction("Appearance");
        }
    }
}