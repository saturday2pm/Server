using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using GSF;

namespace Server
{
    using Env;

    public class ServerBuilder
    {
        private GSF.Server server { get; set; }

        public static ServerBuilder Create()
        {
            return new ServerBuilder();
        }

        public ServerBuilder WithService(ServiceNames name)
        {
            switch(name)
            {
                case ServiceNames.Ingame:
                    server.WithService<Ingame.IngameService>(Ingame.IngameService.Path);
                    break;
                case ServiceNames.Matchmaking:
                    server.WithService<MatchMaking.MatchMakingService>(MatchMaking.MatchMakingService.Path);
                    break;
            }

            return this;
        }

        public void Start()
        {
            var ev = ServerEnv.selectedEnv;

            server = GSF.Server.Create(ev.port);
            server.Run();

            while (true)
            {
                var ch = Console.Read();

                if (ch == 'q') break;
            }
        }
    }
}
