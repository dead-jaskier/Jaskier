using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApiModels.Requests;

using Microsoft.AspNetCore.Mvc;

using TeamServer.Models;
using TeamServer.Services;

namespace TeamServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListenersController : ControllerBase
    {
        private readonly IListenerService _listeners;

        public ListenersController(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [HttpGet]
        public IActionResult GetListeners()
        {
            var listeners = _listeners.GetListeners();
            return Ok(listeners);
        }

        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener == null) return NotFound();

            return Ok(listener);
        }

        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {
            var listener = new HttpListener(request.Name, request.BindPort);
            listener.Start();
            
            _listeners.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";

            return Created(path, listener);
        }
    }
}
