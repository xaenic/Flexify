using Flexify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flexify.Controllers
{
    public class ProfileController : Controller
    {
        private readonly FlexifyDbContext dbContext;
        public ProfileController(FlexifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(string username)
        {

            if (string.IsNullOrEmpty(username))
            {

                return RedirectToAction("LandingPage", "Home");
            }

            UserModel user = new UserModel();
           
            
            user = dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) { return RedirectToAction("LandingPage", "Home"); }
            PostModel[] posts = dbContext.Posts.Where(p => p.UserId == user.Id).ToArray();
            Socials[] socials = dbContext.Socials.Where(p => p.user_id == user.Id).ToArray();
            Theme theme = dbContext.themes.FirstOrDefault(u => u.user_Id == user.Id);
            AppearanceModel appearance = new AppearanceModel(user, null, socials, theme, posts);

            return View(appearance);
        }
    }
}
