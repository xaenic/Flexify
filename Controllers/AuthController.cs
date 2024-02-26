using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Flexify.Controllers
{
    public class AuthController : Controller
    {
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
        public ViewResult Register(UserModel user)
        {
            if(!ModelState.IsValid) {

                return View();
            }

            return View();

            
           
        }
    }
}
