using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var users = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (users != null)
            {
                if (users.Password == user.Password)
                {
                    TempData["Error"] = " Password or Email!";
                }
                else
                {
                    TempData["Error"] = "Wrong Password or Email!";
                }
            }
            else
            {
                TempData["Error"] = "Wrong Password or Email!";
            }
            
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
