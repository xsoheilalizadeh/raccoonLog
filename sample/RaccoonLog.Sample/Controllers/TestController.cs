using Microsoft.AspNetCore.Mvc;

namespace RaccoonLog.Sample.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Json()
        {
            return Json(new
            {
                FirstName = "soheil",
                LastName = "alizadeh",
                Age = 20
            });
        }
    }
}   