using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO : 네임스페이스 옮길까

namespace Server.Ingame
{
    class MatchResolver
    {
        public static T Create<T>()
            where T : IMatchResolver, new()
        {
            return new T();
        }
    }
}
