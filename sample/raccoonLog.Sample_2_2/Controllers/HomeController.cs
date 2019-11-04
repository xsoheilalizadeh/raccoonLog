using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using raccoonLog.Sample_2_2.Models;

namespace raccoonLog.Sample_2_2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Response.Cookies.Append("MyCooOnSet", Guid.NewGuid().ToString(), new CookieOptions
            {
                Expires = DateTime.Now,
                SameSite = SameSiteMode.Lax,
                HttpOnly = true
            });
            return Json(new { boo = "isBoo" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
