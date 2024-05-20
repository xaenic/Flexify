using Flexify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flexify.Components
{
    public class NavbarMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var links = new List<NavLinkModel>
            {
                new NavLinkModel { Url = "/App", Name="Home", Controller="App"},
                new NavLinkModel { Url = "/App/Appearance", Name="Appearance",Controller="App"},
                new NavLinkModel { Url = "/App/Post", Name="Post", Controller = "App"},
                new NavLinkModel { Url = "/App/Account", Name="Settings",Controller="App"},
                new NavLinkModel { Url = "/auth/logout", Name="Logout", Controller = "Auth"},
            };
            return View(links);
        }
    }
}
