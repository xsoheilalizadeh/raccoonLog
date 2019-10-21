using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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