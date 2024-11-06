using CycleTLS.Interfaces;
using CycleTLS.Models;
using System;
using System.Diagnostics;

namespace CycleTLS
{
    public class CycleServer : ICycleServer
    {
        private readonly CycleServerOptions Options;
        private Process ServerProcess { get; set; }

        public CycleServer(CycleServerOptions options)
        {
            Options = options;
        }

        public bool Start()
        {

            ServerProcess = new Process();
            ServerProcess.StartInfo = new ProcessStartInfo()
            {
                FileName = Options.Path,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            ServerProcess.StartInfo.EnvironmentVariables.Add("WS_PORT", Options.Port.ToString());
            ServerProcess.Start();

            AppDomain.CurrentDomain.ProcessExit += ProcessExit;

            return true;
        }

        private void ProcessExit(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (ServerProcess != null)
            {
                if (!ServerProcess.HasExited)
                {
                    ServerProcess.Kill();
                }
                ServerProcess.Dispose();
                ServerProcess = null;
            }

            AppDomain.CurrentDomain.ProcessExit -= ProcessExit;
        }
    }
}
