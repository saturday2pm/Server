using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server
{
    static class BroadcastExt
    {
        public static void Broadcast<T>(
            this IEnumerable<Service<T>> sessions,
            PacketBase packet)
        {
            var json = Serializer.ToJson(packet);
            
            foreach(var session in sessions)
                session.SendRawPacket(json);
        }
    }
}
