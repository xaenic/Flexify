using Microsoft.AspNetCore.Mvc;

namespace Flexify.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
