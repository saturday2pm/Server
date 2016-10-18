using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using WebSocketSharp;
using WebSocketSharp.Server;

using ProtocolCS;

namespace Server
{
    using Env;
    using MatchMaking;
    using Ingame;
     
    class Program
    {
        static WebSocketServer server { get; set; }

        static void BootstrapServices(string[] args)
        {
            if (server == null)
                throw new InvalidOperationException("server is null");

            foreach (var arg in args)
            {
                if (arg == "--matchmaking")
                    server.AddWebSocketService<MatchMakingService>(MatchMakingService.Path);
                if (arg == "--game")
                    server.AddWebSocketService<IngameService>(IngameService.Path);
            }
        }
        static void ProcessCmdOptions(string[] args)
        {
        }

        static void Shutdown()
        {
            Console.WriteLine("Close Server...");

            server.Stop(CloseStatusCode.Away, "close server");
        }

        static void Main(string[] args)
        {
            var ev = ServerEnv.selectedEnv;

            server = new WebSocketServer(ev.port)
            {
                ReuseAddress = true,
                KeepClean = true
            };
            
            if (args.Length == 0)
            {
                args = new string[]
                {
                    "--matchmaking",
                    "--game"
                };
            }

            BootstrapServices(args);
            ProcessCmdOptions(args);
            
            server.Start();

            while (true)
            {
                var ch = Console.Read();

                if (ch == 'q') break;
            }

            Shutdown();
        }
    }
}
