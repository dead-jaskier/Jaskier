using Agent.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Commands
{
    /// <summary>
    /// Basic Print Working Directory command - Responds with the current working directory of Agent
    /// </summary>
    public class PrintWorkingDirectory : AgentCommand
    {
        public override string Name => "pwd";

        public override string Execute(AgentTask task)
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
