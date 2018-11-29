using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Webapi.Controllers
{
    public class AController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("a");
        }

    }
}
