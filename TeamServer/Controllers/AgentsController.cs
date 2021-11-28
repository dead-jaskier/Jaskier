﻿using Microsoft.AspNetCore.Mvc;

using TeamServer.Services;

namespace TeamServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agents;

        public AgentsController(IAgentService agents)
        {
            _agents = agents;
        }

        [HttpGet]
        public IActionResult GetAgents()
        {
            var agents = _agents.GetAgents();
            return Ok(agents);
        }

        [HttpGet("{name}")]
        public IActionResult GetAgents(string name)
        {
            var agent = _agents.GetAgent(name);
            if (agent is null) return NotFound();

            return Ok(agent);
        }
    }
}
