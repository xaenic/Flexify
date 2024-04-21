using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Data.SqlClient;

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
        public IActionResult Login()
        {
            return View();
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

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            TempData["Success"] = "Successfully Registered!";
            ModelState.Clear();
            return View(new UserModel());

            
           
        }
    }
}
