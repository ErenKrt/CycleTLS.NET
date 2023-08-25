using CycleTLS.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleTLS
{
    public class CycleServer : ICycleServer
    {
        private readonly int Port;
        private readonly string CyclePath = "D:\\Tools\\cycleTLS\\server.exe";
        private Process ServerProcess { get; set; }


        // TODO: make CycleServerOptions
        // TODO: make multi platform (windows, linux, mac) support

        public CycleServer(int port= 9112) {
            Port = port;
        }

        public async Task<bool> Start()
        {

            ServerProcess = new Process();
            ServerProcess.StartInfo = new ProcessStartInfo()
            {
                FileName = CyclePath,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            ServerProcess.StartInfo.EnvironmentVariables.Add("WS_PORT", Port.ToString());
            ServerProcess.Start();

            /*
             TODO: need imp
             */
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
                if (ServerProcess.HasExited)
                {
                    return;
                }

                ServerProcess.Kill();
            };

            return true;
        }
    }
}
