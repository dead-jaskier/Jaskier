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
            GenerateMetadata();

            _commModule = new HttpCommModule("localhost", 8080);
            _commModule.Init(_metadata);
            _commModule.Start();

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

            string userName = Environment.UserName;
            string integrity = "Medium";

            if (userName.Equals("SYSTEM"))
                integrity = "SYSTEM";

            using (var identity = WindowsIdentity.GetCurrent())
            {
                if (identity.User != identity.Owner)
                {
                    integrity = "High";
                }
            }

            _metadata = new AgentMetaData()
            {
                Id = Guid.NewGuid().ToString(),
                HostName = Environment.MachineName,
                UserName = userName,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity = integrity,
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86"
            };
        }
    }
}
