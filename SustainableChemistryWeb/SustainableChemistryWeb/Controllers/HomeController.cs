using Microsoft.AspNetCore.Mvc;
using SustainableChemistryWeb.Models;
using System.Diagnostics;

namespace SustainableChemistryWeb.Controllers
{
    public class HomeController : Controller
    {
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Index(string message)
        {
            ViewData["ErrorMessage"] = message;
            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult UsersGuide()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Exit_Epa(string URL)
        {
            ViewData["URL"] = URL;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
