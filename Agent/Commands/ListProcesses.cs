using Agent.Models;

using System.Collections.Generic;
using System.Diagnostics;

namespace Agent.Commands
{
    public class ListProcesses : AgentCommand
    {
        public override string Name => "ps";

        public override string Execute(AgentTask task)
        {
            var results = new SharpSploitResultList<ListProcessesResult>();
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                var result = new ListProcessesResult
                {
                    ProcessName = process.ProcessName,
                    ProcessId = process.Id,
                    SessionId = process.SessionId
                };
                result.ProcessPath = GetProcessPath(process);

                results.Add(result);
            }

            return results.ToString();
        }

        private string GetProcessPath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                return "-";
            }
        }

        public sealed class ListProcessesResult : SharpSploitResult
        {
            public string ProcessName { get; set; }
            public string ProcessPath { get; set; }
            public int ProcessId { get; set; }
            public int SessionId { get; set; }

            protected internal override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>()
            {
                new SharpSploitResultProperty { Name = nameof(ProcessName), Value = ProcessName },
                new SharpSploitResultProperty { Name = nameof(ProcessPath), Value = ProcessPath },
                new SharpSploitResultProperty { Name = "PID", Value = ProcessId },
                new SharpSploitResultProperty { Name = nameof(SessionId), Value = SessionId }
            };
        }
    }
}
