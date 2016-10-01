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
    public class Service<T> : WebSocketBehavior
    {
        private static Dictionary<Type, MethodInfo> handlers { get; set; }

        private static ConcurrentDictionary<int, T> sessions { get; set; }

        private int _currentPlayerId;
        public int currentPlayerId
        {
            get { return _currentPlayerId; }
            set
            {
                sessions[value] = (T)(object)this;
                _currentPlayerId = value;
            }
        }

        static Service()
        {
            handlers = new Dictionary<Type, MethodInfo>();

            var handlerCandidates = typeof(T).GetMethods(
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

        /// <summary>
        /// 숫자로된 플레이어 아이디로 세션을 가져온다.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>세션</returns>
        /// <exception cref="KeyNotFoundException">
        /// 주어진 playerId로 찾을 수 없을 때
        /// </exception>
        protected static T GetSessionById(int playerId)
        {
            return sessions[playerId];
        }

        protected void ErrorClose(CloseStatusCode code, string reason)
        {
            if (State != WebSocketState.Open)
            {
                Console.WriteLine("State != Open");
                return;
            }

            Sessions.CloseSession(this.ID, code, reason);
        }

        protected void SendPacket(PacketBase packet)
        {
            try
            {
                var json = Serializer.ToJson(packet);

                Send(Encoding.UTF8.GetBytes(json));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception ex)
            {
            }

            if (packet == null)
            {
                Console.WriteLine($"Parsing Error : {e.Data}");
                ErrorClose(CloseStatusCode.InvalidData, "parsing error");
            }
            else if (handlers.ContainsKey(packet.GetType()) == false)
            {
                Console.WriteLine($"Unkown Packet : {e.Data}");
                ErrorClose(CloseStatusCode.InvalidData, "unknown packet");
            }
            else
            {
                var handler = handlers[packet.GetType()];

                try
                {
                    handler.Invoke(null, new object[] { packet });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorClose(CloseStatusCode.ServerError, "internal server error");
                }
            }
        }
    }
}
