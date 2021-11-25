using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;


namespace TeamServer.Models
{
    [Controller]
    public class HttpListenerController : ControllerBase
    {
        public IActionResult HandleImplant()
        {
            return Ok("Your Listener works");
        }
    }
}
