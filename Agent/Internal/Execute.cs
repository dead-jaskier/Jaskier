using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Internal
{
    public static class Execute
    {
        public static string ExecuteCommand(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(startInfo);

            string output = "";

            using (process.StandardOutput)
            {
                output += process.StandardOutput.ReadToEnd();
            }

            using (process.StandardError)
            {
                output += process.StandardError.ReadToEnd();
            }

            return output;
        }

        public static string ExecuteAssembly(byte[] asm, string[] arguments = null)
        {
            if (arguments is null)
                arguments = new string[] { };

            var currentOut = Console.Out;
            var ccurrentError = Console.Error;

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            Console.SetOut(writer);
            Console.SetError(writer);

            var assembly = Assembly.Load(asm);
            assembly.EntryPoint.Invoke(null, new object[] { arguments });

            Console.Out.Flush();
            Console.Error.Flush();

            var output = Encoding.UTF8.GetString(stream.ToArray());

            Console.SetOut(currentOut);
            Console.SetError(ccurrentError);

            stream.Dispose();
            writer.Dispose();

            return output;
        }
    }
}
