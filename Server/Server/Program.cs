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

using GSF;

namespace Server
{
    using Env;
    using MatchMaking;
    using Ingame;
     
    class Program
    {
        static GSF.Server server { get; set; }

        static void BootstrapServices(string[] args)
        {
            if (server == null)
                throw new InvalidOperationException("server is null");

            foreach (var arg in args)
            {
                if (arg == "--matchmaking")
                    server.WithService<MatchMakingService>(MatchMakingService.Path);
                if (arg == "--game")
                    server.WithService<IngameService>(IngameService.Path);
            }
        }
        static void ProcessCmdOptions(string[] args)
        {
        }

        static void Shutdown()
        {
            Console.WriteLine("Close Server...");

            server.Stop("close server");
        }

        static void Main(string[] args)
        {
            var ev = ServerEnv.selectedEnv;

            server = GSF.Server.Create(ev.port);

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
            
            server.Run();

            while (true)
            {
                var ch = Console.Read();

                if (ch == 'q') break;
            }

            Shutdown();
        }
    }
}
