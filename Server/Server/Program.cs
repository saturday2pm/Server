using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using WebSocketSharp;
using WebSocketSharp.Server;

using ProtocolCS;

namespace Server
{
    public class Service : WebSocketBehavior
    {
        private Dictionary<Type, MethodInfo> handlers { get; set; }

        public Service()
        {
            handlers = new Dictionary<Type, MethodInfo>();

            var handlerCandidates = GetType().GetMethods(
                BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetParameters().Length == 1)
                .Where(x => x.GetParameters().First().ParameterType.Namespace == nameof(ProtocolCS))
                .Where(x => x.ReturnType == typeof(void));

            foreach (var candidate in handlerCandidates)
            {
                var packetType = candidate
                    .GetParameters().First()
                    .ParameterType;

                handlers[packetType] = candidate;
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var json = e.Data;
            object packet = null;

            try
            {
                packet = Serializer.ToObject(json);
            }
            catch(Exception ex)
            {
            }

            if (packet == null)
                Console.WriteLine($"Parsing Error : {e.Data}");
            else if (handlers.ContainsKey(packet.GetType()) == false)
                Console.WriteLine($"Unkown Packet : {e.Data}");
            else
            {
                var handler = handlers[packet.GetType()];

                try
                {
                    handler.Invoke(null, new object[] { packet });
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
    public sealed class MatchMakingService : Service
    {
        public static readonly string Path = "/mmaker";

        public void OnJoinQueue(JoinQueue p)
        {
            Console.WriteLine("JoinQueue");
        }
    }

    public sealed class IngameService : Service
    {
        public static readonly string Path = "/game";

        public void OnMoveEvent(MoveEvent p)
        {
            Console.WriteLine("OnMoveEvent");
        }
        public void OnUpgradeEvent(UpgradeEvent e)
        {
            Console.WriteLine("OnUpgradeEvent");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var server = new WebSocketServer();

            server.AddWebSocketService<MatchMakingService>(MatchMakingService.Path);
            server.AddWebSocketService<IngameService>(IngameService.Path);

            server.Start();
            Console.Read();
            server.Stop();
        }
    }
}
