using Agent.Models;

using System;
using System.Diagnostics;
using System.Security.Principal;

namespace Agent
{
    internal class Program
    {
        private static AgentMetaData _metadata;

        static void Main(string[] args)
        {
            GenerateMetadata();
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
