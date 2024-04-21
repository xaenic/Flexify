using Flexify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flexify.Controllers
{
    public class AppController : Controller
    {
        private readonly FlexifyDbContext dbContext;
        public AppController(FlexifyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpGet]
        public IActionResult Account()
        {
            int userId = 1;
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }
        [HttpPost]
        public IActionResult Account(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            int userId = 1;

            var userUpdate = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (userUpdate == null)
            {
                // User not found, handle accordingly (e.g., return a 404 Not Found response)
                return NotFound();
            }

            userUpdate.FirstName = user.FirstName;
            userUpdate.LastName = user.LastName;    
            userUpdate.Email = user.Email;
            userUpdate.Username = user.Username;

            TempData["Success"] = "Updated successfully!";
            dbContext.SaveChanges();
            return View(user);
        }
    }
}
